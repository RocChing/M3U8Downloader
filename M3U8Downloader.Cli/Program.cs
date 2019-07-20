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
            string docString = CmdLine.GetHelpString();
            Console.WriteLine(docString);

            IVideoHandle handler = new FFmpegVideoHandler();

            while (true)
            {
                Console.Write("请输入命令:");
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }
                if (CmdLine.IsExitCmd(input))
                {
                    break;
                }
                if (CmdLine.IsClearCmd(input))
                {
                    Console.Clear();
                    continue;
                }
                if (CmdLine.IsHelpCmd(input))
                {
                    Console.WriteLine(docString);
                    continue;
                }
                var opt = CmdLine.Parse<VideoOptions>(input);
                if (opt != null)
                {
                    try
                    {
                        handler.Download(opt);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
        }
    }
}
