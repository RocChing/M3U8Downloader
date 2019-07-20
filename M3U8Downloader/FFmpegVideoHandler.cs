namespace M3U8Downloader
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    public class FFmpegVideoHandler : IVideoHandle
    {
        private int ffmpegId;

        public FFmpegVideoHandler()
        {
            DataReceived += (str) =>
            {
                if (str.StartsWith("Press [q] to stop") || str.StartsWith("frame="))
                {
                    var status = ParseFFmpegProgress(str);
                    Console.WriteLine(status);
                }
            };
        }

        public event Action<string> DataReceived;

        public void Convert(VideoOptions opt)
        {
            Console.WriteLine(opt);
            Strart(opt);
        }

        public void Download(VideoOptions opt)
        {
            Convert(opt);
        }

        private void Strart(VideoOptions opt)
        {
            Console.WriteLine("开始处理...");
            using (var p = new Process())
            {
                p.StartInfo.FileName = opt.FFmpegPath;      // 命令  
                p.StartInfo.Arguments = opt.ToString();      // 参数  

                p.StartInfo.CreateNoWindow = true;         // 不创建新窗口  
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;  // 重定向输入  
                p.StartInfo.RedirectStandardOutput = true; // 重定向标准输出  
                p.StartInfo.RedirectStandardError = true;  // 重定向错误输出
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                p.OutputDataReceived += P_OutputDataReceived;
                p.ErrorDataReceived += P_ErrorDataReceived;

                p.EnableRaisingEvents = true;// 启用Exited事件  
                p.Exited += P_Exited;   // 注册进程结束事件  

                p.Start();

                ffmpegId = p.Id;//获取ffmpeg.exe的进程ID

                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();
            }
            Console.WriteLine("处理完成...");
        }

        private void P_Exited(object sender, EventArgs e)
        {

        }

        private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                DataReceived?.Invoke(e.Data);
            }
        }

        private void P_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                DataReceived?.Invoke(e.Data);
            }
        }

        public VideoStatus ParseFFmpegProgress(string text)
        {
            VideoStatus res = new VideoStatus();
            if (string.IsNullOrEmpty(text))
                return res;
            // frame=  929 fps=0.0 q=-0.0 size=   68483kB time=00:00:37.00 bitrate=15162.6kbits/s speed=  74x    
            //string[] Values = text.Split('=');
            try
            {
                res.Frame = long.Parse(ParseAttribute(text, "frame"), CultureInfo.InvariantCulture);
                res.Fps = float.Parse(ParseAttribute(text, "fps"), CultureInfo.InvariantCulture);
                res.Quantizer = float.Parse(ParseAttribute(text, "q"), CultureInfo.InvariantCulture);
                res.Size = ParseAttribute(text, "size");
                res.Time = TimeSpan.Parse(ParseAttribute(text, "time"), CultureInfo.InvariantCulture);
                res.Bitrate = ParseAttribute(text, "bitrate");
                string SpeedString = ParseAttribute(text, "speed");
                if (SpeedString != "N/A")
                    res.Speed = float.Parse(SpeedString.TrimEnd('x'), CultureInfo.InvariantCulture);
            }
            catch
            {
            }
            return res;
        }

        public string ParseAttribute(string text, string key)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(key))
                return null;
            int Pos = text.IndexOf(key + "=");
            if (Pos >= 0)
            {
                // Find first non-space character.
                Pos += key.Length + 1;
                while (Pos < text.Length && text[Pos] == ' ')
                {
                    Pos++;
                }
                // Find space after value.
                int PosEnd = text.IndexOf(' ', Pos);
                if (PosEnd == -1)
                    PosEnd = text.Length;
                return text.Substring(Pos, PosEnd - Pos);
            }
            else
                return null;
        }
    }
}