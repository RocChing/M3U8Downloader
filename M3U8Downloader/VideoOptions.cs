namespace M3U8Downloader
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Linq;
    using System.IO;

    public class VideoOptions
    {
        private readonly string[] _notAllowFormats;
        public VideoOptions()
        {
            _notAllowFormats = new string[] { "3gp", "mp4" };
            IsOverrideFile = true;
            FFmpegPath = GetAbsPath(@"Tools\ffmpeg.exe");
        }

        public VideoOptions(string input, string output) : this()
        {
            Input = input;
            Output = output;
        }

        /// <summary>
        /// FFmpeg 路径
        /// </summary>
        /// <value></value>
        public string FFmpegPath { get; private set; }

        /// <summary>
        /// 输出路径
        /// </summary>
        /// <value></value>
        public string Output { get; set; }

        /// <summary>
        /// 输入路径 -i
        /// </summary>
        /// <value></value>
        public string Input { get; set; }

        /// <summary>
        /// 是否要覆盖原文件/覆盖输出文件 -y
        /// </summary>
        /// <value></value>
        public bool IsOverrideFile { get; set; }

        /// <summary>
        /// -f fmt 强迫采用格式fmt avi/asf/wav/3gp/mkv/flv/rm/mp4
        /// </summary>
        /// <value></value>
        public string Format { get; set; }

        /// <summary>
        /// -t duration 设置纪录时间 hh:mm:ss[.xxx]格式的记录时间也支持
        /// </summary>
        /// <value></value>
        public string Duration { get; set; }

        /// <summary>
        /// -b bitrate 设置比特率，缺省200kb/s
        /// </summary>
        /// <value></value>
        public string Bitrate { get; set; }

        /// <summary>
        /// -r fps 设置帧频 缺省25
        /// </summary>
        /// <value></value>
        public string Fps { get; set; }

        /// <summary>
        /// -s size 设置帧大小 格式为WXH 缺省160X128.下面的简写也可以直接使用：
        /// Sqcif 128X96 qcif 176X144 cif 252X288 4cif 704X576
        /// </summary>
        /// <value></value>
        public string Size { get; set; }

        /// <summary>
        /// 命令行 如果使用此参数 则覆盖所有参数 除了 Input , Output ，优先级次之
        /// </summary>
        /// <value></value>
        public IEnumerable<string> Parameters { get; set; }

        /// <summary>
        /// 命令行 如果使用此参数 则覆盖所有参数 除了 Input , Output, 优先级最高
        /// </summary>
        /// <value></value>
        public string Cmd { get; set; }

        public virtual bool Check()
        {
            if (string.IsNullOrEmpty(Input))
            {
                throw new ArgumentNullException(nameof(Input));
            }

            bool isUrl = IsURL(Input);
            if (!isUrl)
            {
                if (!File.Exists(Input))
                {
                    string input_path = GetAbsPath(Input);
                    if (!File.Exists(input_path))
                    {
                        throw new FileNotFoundException(input_path);
                    }
                    Input = input_path;
                }
            }

            if (string.IsNullOrEmpty(Output))
            {
                throw new ArgumentNullException(nameof(Output));
            }

            if (!File.Exists(Output))
            {
                string output_path = GetAbsPath(Output);
                Output = output_path;
            }

            CreateDirectory(Output);

            return true;
        }

        public override string ToString()
        {
            if (!this.Check()) return "";

            StringBuilder sb = new StringBuilder();
            sb.Append($"-i {Input}");

            if (!string.IsNullOrEmpty(Cmd))
            {
                sb.Append($" {Cmd}");
            }
            else
            {
                if (IsOverrideFile) sb.Append(" -y");
                if (!string.IsNullOrEmpty(Format))
                {
                    if (!_notAllowFormats.Contains(Format.ToLower())) sb.Append($" -f {Format}");
                }
                if (!string.IsNullOrEmpty(Duration)) sb.Append($" -t {Duration}");
                if (!string.IsNullOrEmpty(Bitrate)) sb.Append($" -b {Bitrate}");
                if (!string.IsNullOrEmpty(Fps)) sb.Append($" -r {Fps}");
                if (!string.IsNullOrEmpty(Size)) sb.Append($" -s {Size}");
            }

            if (Parameters != null && Parameters.Count() > 0)
            {
                foreach (var item in Parameters)
                {
                    sb.Append($" {item}");
                }
            }

            sb.Append($" {Output}");

            return sb.ToString();
        }

        private string GetAbsPath(string relPath)
        {
            //string path = Directory.GetCurrentDirectory();
            //string path = Environment.CurrentDirectory;
            var executablePath = Environment.GetCommandLineArgs()[0];
            string path = Path.GetDirectoryName(executablePath);

            return Path.Combine(path, relPath);
        }

        private void CreateDirectory(string path)
        {
            string dire = Path.GetDirectoryName(path);
            if (!Directory.Exists(dire))
            {
                Directory.CreateDirectory(dire);
            }
        }

        private bool IsURL(string url)
        {
            string pattern = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$";
            return Regex.IsMatch(url, pattern, RegexOptions.Singleline);
        }
    }
}