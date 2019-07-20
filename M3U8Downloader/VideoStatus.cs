using System;
using System.Collections.Generic;
using System.Text;

namespace M3U8Downloader
{
    public class VideoStatus
    {
        public long Frame { get; set; }
        public float Fps { get; set; }
        public float Quantizer { get; set; }
        public string Size { get; set; } = "";
        public TimeSpan Time { get; set; }
        public string Bitrate { get; set; } = "";
        public float Speed { get; set; }

        public override string ToString()
        {
            return $"Size:{Size},Time:{Time},Bitrate:{Bitrate},Speed:{Speed}";
        }
    }
}
