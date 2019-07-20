using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Downloader.Cli
{
    public class CmdInputInfo : CmdInfo
    {
        public string Value { get; set; }

        public CmdInputInfo() { }

        public CmdInputInfo(CmdInfo info, string value)
        {
            ShortName = info.ShortName;
            Name = info.Name;
            Desc = info.Desc;
            Value = value;
        }

        public override string ToString()
        {
            return $"-{ShortName}   | --{Name}    {Value}   #{Desc}";
        }
    }
}
