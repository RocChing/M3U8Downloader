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
            //opt.Input = @"videos\video.ts";
            opt.Input = "http://free.ghxcy.cn/free/MF20190526RTEW/MF20190526RTEW.m3u8";
            opt.Output = @"videos\v5.mp4";
            opt.Parameters  = list;
            //opt.Format = "m";

            handler.Download(opt);

            Console.Read();
        }
    }
}
