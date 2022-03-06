using System;
using System.Threading;

using FFmpeg.AutoGen;

namespace VMSViewer
{
    public class RTSP : IDisposable
    {
        /// <summary>
        /// 클라이언트
        /// </summary>
        private readonly Client Client;

        /// <summary>
        /// 스트리밍 쓰레드
        /// </summary>
        private Thread StreamingThread = null;

        public RTSP(Client Client)
        {
            this.Client = Client;
        }

        public void Dispose()
        {

        }

        public void InitRTSP()
        {
            if(StreamingThread == null)
            {
                StreamingThread = new Thread(Streaming);
                StreamingThread.Start();
            }
        }

        /// <summary>
        /// RTSP 연결
        /// </summary>
        private void Connect()
        {

        }

        /// <summary>
        /// RTSP 연결끊기
        /// </summary>
        public void Disconnect()
        {

        }

        /// <summary>
        /// RTSP 스트리밍
        /// </summary>
        public void Streaming()
        {

        }
    }
}
