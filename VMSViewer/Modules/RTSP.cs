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
        /// TRUE 시 카메라 끊기
        /// </summary>
        private bool IsDisconnect = false;

        /// <summary>
        /// 스트리밍 쓰레드
        /// </summary>
        private Thread StreamingThread = null;

        /// <summary>
        /// 디코더
        /// </summary>
        private VideoStreamDecoder VideoStreamDecoder = null;

        /// <summary>
        /// 카메라가 끊어졌을 시 재시작하는 타이머
        /// </summary>
        private System.Timers.Timer ConnectTimer = null;

        public event EventHandler OnDisplayStream;

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
        /// 현재 카메라가 연결 중인지 확인
        /// </summary>
        public bool IsConnect()
        {
            return false;
        }

        /// <summary>
        /// RTSP 연결
        /// </summary>
        private bool Connect()
        {
            if (VideoStreamDecoder == null) VideoStreamDecoder = new VideoStreamDecoder();

            if (VideoStreamDecoder.Connect(Client.RTSPAddress))
            {
                return true;
            }
            else
            {
                return false;
            }    
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
        public unsafe void Streaming()
        {
            Console.WriteLine($"카메라 연결..... -> {Client.RTSPAddress}");

            if(Connect())
            {
                /* VideoStreamDecoder.TryDecodeNextFrame() => False시 카운트증가 */
                int FailedDecodeNextFrame = 0;
                /* 연결된 카메라정보(해상도, 픽셀포맷 등)*/
                var sourceSize = VideoStreamDecoder.FrameSize;
                var sourcePixelFormat = VideoStreamDecoder.PixelFormat;
                var destinationSize = sourceSize;
                var destinationPixelFormat = AVPixelFormat.AV_PIX_FMT_BGR24;

                using (var VideoFrameConverter = new VideoFrameConverter(sourceSize, sourcePixelFormat, destinationSize, destinationPixelFormat))
                {
                    while (true)
                    {
                        if (IsDisconnect)
                        {
                            break;
                        }

                        if (VideoStreamDecoder.TryDecodeNextFrame(out var Decodeframe) == false)
                        {
                            FailedDecodeNextFrame++;

                            if (FailedDecodeNextFrame >= 100)
                            {
                                FailedDecodeNextFrame = 0;
                                ConnectTimer.Start();
                                break;
                            }
                        }

                        else
                        {
                            FailedDecodeNextFrame = 0;

                            AVFrame targetFrame = VideoFrameConverter.Convert(Decodeframe);

                            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap
                            (
                                targetFrame.width, 
                                targetFrame.height, 
                                targetFrame.linesize[0], 
                                System.Drawing.Imaging.PixelFormat.Format24bppRgb, 
                                (IntPtr)targetFrame.data[0]
                            );
                        }
                    }
                }
            }
        }
    }
}
