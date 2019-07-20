using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace M3U8Downloader.Cli
{
    public class CmdLine
    {
        public static List<CmdInfo> _allCmds = CmdInfo.GetDefaultCmdList();

        public static string GetHelpString()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append("******************************************************************************************************************").AppendLine();
            sb.AppendFormat($"*M3U8下载器 版本: {version}").AppendLine();
            sb.Append("*帮助文档").AppendLine();
            sb.Append("***").AppendLine();

            foreach (var item in _allCmds)
            {
                sb.Append($"*** {item}").AppendLine();
            }
            sb.Append("***").AppendLine();
            sb.Append("*****************************************************************************************************************").AppendLine();
            return sb.ToString();
        }

        public static bool IsHelpCmd(string input)
        {
            string pattern = @"(-h\b)|(--Help\b)";
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        public static bool IsExitCmd(string input)
        {
            string pattern = @"(-e\b)|(--exit\b)";
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        public static bool IsClearCmd(string input)
        {
            string pattern = @"(-z\b)|(--clear\b)";
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        public static List<CmdInputInfo> Parse(string input)
        {
            List<CmdInputInfo> list = new List<CmdInputInfo>();
            if (string.IsNullOrEmpty(input))
            {
                return list;
            }

            bool error = false;
            string pattern = @"([-]{1,2})([\w]+)?([ ]+)?([\S]+)?";

            var matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);
            if (matches != null && matches.Count > 0)
            {
                foreach (Match item in matches)
                {
                    if (item.Success)
                    {
                        string pre = item.Groups[1].Value;
                        string cmd = item.Groups[2].Value;
                        string value = item.Groups[4].Value;

                        var cmdInfo = _allCmds.Find(m => m.ShortName.Equals(cmd, StringComparison.InvariantCultureIgnoreCase) || m.Name.Equals(cmd, StringComparison.InvariantCultureIgnoreCase));
                        if (cmdInfo != null)
                        {
                            if (string.IsNullOrEmpty(value))
                            {
                                error = true;
                                Console.WriteLine($"命令{cmd}输入的值不能为空");
                                continue;
                            }
                            list.Add(new CmdInputInfo(cmdInfo, value));
                        }
                        else
                        {
                            error = true;
                            Console.WriteLine($"命令输入有误{cmd}有误");
                        }
                    }
                }
            }
            if (list.Count < 1)
            {
                Console.WriteLine($"命令输入有误 {input}");
            }
            if (error)
            {
                list.Clear();
            }
            return list;
        }

        public static T Parse<T>(string input) where T : class, new()
        {
            var list = Parse(input);
            if (list != null && list.Count > 0)
            {
                T instance = new T();
                Type type = instance.GetType();

                foreach (var item in list)
                {
                    var p = type.GetProperty(item.Name);
                    if (p != null)
                    {
                        p.SetValue(instance, item.Value, null);
                    }
                }

                return instance;
            }
            return default(T);
        }
    }
}
