namespace M3U8Downloader
{
    using System;
    using System.Diagnostics;

    public class FFmpegVideoHandler : IVideoHandle
    {
        private int ffmpegId;

        public FFmpegVideoHandler()
        {
            DataReceived += (str) =>
            {
                Console.WriteLine("end");
                Console.WriteLine(str);
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

            var p = new Process();
            p.StartInfo.FileName = opt.FFmpegPath;      // 命令  
            p.StartInfo.Arguments = opt.ToString();      // 参数  

            p.StartInfo.CreateNoWindow = false;         // 不创建新窗口  
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = false;  // 重定向输入  
            p.StartInfo.RedirectStandardOutput = false; // 重定向标准输出  
            p.StartInfo.RedirectStandardError = false;  // 重定向错误输出  
                                                       //CmdProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;  
            p.OutputDataReceived += new DataReceivedEventHandler(P_OutputDataReceived);
            p.ErrorDataReceived += new DataReceivedEventHandler(P_ErrorDataReceived);

            //p.EnableRaisingEvents = true;                      // 启用Exited事件  
            //p.Exited += new EventHandler(CmdProcess_Exited);   // 注册进程结束事件  

            p.Start();
            ffmpegId = p.Id;//获取ffmpeg.exe的进程ID
            //p.BeginOutputReadLine();
            //p.BeginErrorReadLine();
            Console.WriteLine(ffmpegId);
        }

        private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                //Console.WriteLine("end");
                //Console.WriteLine(e.Data);

                DataReceived?.Invoke(e.Data);
            }
        }

        private void P_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {

            Console.WriteLine(e.Data);
        }
    }
}