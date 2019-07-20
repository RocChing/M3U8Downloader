using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Downloader.Cli
{
    public class CmdInfo
    {
        public const string HelpName = "Help";
        public const string ExitName = "Exit";
        public const string ClearName = "Clear";

        public string ShortName { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public CmdInfo() { }

        public CmdInfo(string name, string desc)
        {
            if (!string.IsNullOrEmpty(name))
            {
                ShortName = name.Substring(0, 1).ToLower();
                Name = name;
                Desc = desc;
            }
        }

        public CmdInfo(string shortName, string name, string desc)
        {
            ShortName = shortName;
            Name = name;
            Desc = desc;
        }

        public static List<CmdInfo> GetDefaultCmdList()
        {
            List<CmdInfo> cmds = new List<CmdInfo>();
            cmds.Add(new CmdInfo(HelpName, "帮助文档"));
            cmds.Add(new CmdInfo("Input", "输入路径,支持本地路径(相对或绝对)或网络路径"));
            cmds.Add(new CmdInfo("Output", "输出路径"));
            cmds.Add(new CmdInfo("y", "IsOverrideFile", "是否要覆盖原文件/覆盖输出文件"));
            cmds.Add(new CmdInfo("Format", "转换格式 avi/asf/wav/3gp/mkv/flv/rm/mp4 以及更多"));
            cmds.Add(new CmdInfo("Duration", "设置纪录时间 hh:mm:ss[.xxx]格式的记录时间也支持"));
            cmds.Add(new CmdInfo("Bitrate", "设置比特率，缺省200kb/s"));
            cmds.Add(new CmdInfo("r", "Fps", "设置帧频 缺省25"));
            cmds.Add(new CmdInfo("Size", "设置帧大小 格式为WXH 缺省160X128. Sqcif 128X96 qcif 176X144 cif 252X288 4cif 704X576"));
            cmds.Add(new CmdInfo("Cmd", "如果使用此参数 则覆盖所有参数 除了 Input , Output, 优先级最高"));
            cmds.Add(new CmdInfo("z", ClearName, "清理屏幕"));
            cmds.Add(new CmdInfo(ExitName, "退出程序"));
            return cmds;
        }

        public override string ToString()
        {
            return $"-{ShortName}   | --{Name}    {Desc}";
        }
    }
}
