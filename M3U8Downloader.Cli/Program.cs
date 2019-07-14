using System;
using System.IO;
using M3U8Downloader;
using System.Collections.Generic;

namespace M3U8Downloader.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            IVideoHandle handler = new FFmpegVideoHandler();

            List<string> list = new List<string>();
            list.Add("-c copy");
            list.Add("-bsf:a aac_adtstoasc -movflags +faststart");

            VideoOptions opt = new VideoOptions();
            opt.Input=@"http://mvmusic2.meitudata.com/video-replace.mp4";
            //opt.Input = @"videos\video.ts";
            //opt.Input = "http://edge.ivideo.sina.com.cn/143292565.hlv?KID=sina,viask&Expires=1563206400&ssig=YoiJrpJpMs";
            opt.Output = @"videos\v147.mp4";
            opt.Parameters  = list;
            //opt.Format = "m";

            handler.Download(opt);

            Console.Read();
        }
    }
}
