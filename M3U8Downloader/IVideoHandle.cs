using System;

namespace M3U8Downloader
{
    public interface IVideoHandle
    {
        void Convert(VideoOptions opt);

        void Download(VideoOptions opt);

        event Action<string> DataReceived;
    }
}