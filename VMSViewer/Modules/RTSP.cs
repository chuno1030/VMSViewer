using System;
using System.Drawing;
using System.Threading;

using FFmpeg.AutoGen;

namespace VMSViewer
{
    public class RTSP : IDisposable
    {
        #region Events
        public delegate void DisplayStreamDelegate(Bitmap Bitmap);
        public event DisplayStreamDelegate onDisplayStream;

        public delegate void ConnectionStatusDelegate(ConnectionStatus ConnectionStatus);
        public event ConnectionStatusDelegate onConnectionStatus;
        #endregion

        /// <summary>
        /// 클라이언트
        /// </summary>
        private readonly Device Device;

        /// <summary>
        /// TRUE 시 전체화면에 영상표출
        /// </summary>
        public bool IsFullScreen = false;

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

        public RTSP(Device Device)
        {
            this.Device = Device;
        }

        public void Dispose()
        {
            ClearRTSP();
        }

        public void InitRTSP()
        {
            if(StreamingThread == null)
                StreamingThread = new Thread(Streaming);

            StreamingThread.Start();
        }

        public void ClearRTSP()
        {
            IsDisconnect = false;
            IsFullScreen = false;

            if (ConnectTimer != null)
            {
                if (ConnectTimer.Enabled) ConnectTimer.Stop();
                ConnectTimer.Close();
                ConnectTimer.Dispose();
                ConnectTimer = null;
            }

            if (VideoStreamDecoder != null)
            {
                VideoStreamDecoder.Dispose();
                VideoStreamDecoder = null;
            }

            if (StreamingThread != null)
            {
                if (StreamingThread.IsAlive) StreamingThread.Abort();
                StreamingThread = null;
            }

            GC.Collect();
        }

        /// <summary>
        /// 현재 카메라가 연결 중인지 확인
        /// </summary>
        public bool IsConnect() { return VideoStreamDecoder.IsConnect(); }

        /// <summary>
        /// RTSP 연결
        /// </summary>
        private bool Connect()
        {
            if (onConnectionStatus != null) onConnectionStatus(ConnectionStatus.Connecting);

            if (ConnectTimer == null)
            {
                ConnectTimer = new System.Timers.Timer();
                //ConnectTimer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;
                ConnectTimer.Interval = TimeSpan.FromSeconds(15).TotalMilliseconds;
                ConnectTimer.Elapsed += ConnectTimer_Elapsed;
            }

            if (VideoStreamDecoder == null) VideoStreamDecoder = new VideoStreamDecoder();
            if (VideoStreamDecoder.Connect(Device.RTSPAddress))
            {
                if (onConnectionStatus != null) onConnectionStatus(ConnectionStatus.Connected);
                return true;
            }
            else
            {
                if (onConnectionStatus != null) onConnectionStatus(ConnectionStatus.Disconnected);
                ConnectTimer.Start();
                return false;
            }    
        }

        /// <summary>
        /// RTSP 연결끊기
        /// </summary>
        public void Disconnect()
        {
            Console.WriteLine($"### 클라이언트 {Device.DeviceID}번 카메라 연결해제 ###");
            IsDisconnect = true;
        }

        /// <summary>
        /// RTSP 카메라가 끊어졌을 시 작동하는 타이머
        /// </summary>
        private void ConnectTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

        }

        /// <summary>
        /// RTSP 스트리밍
        /// </summary>
        public unsafe void Streaming()
        {
            Console.WriteLine($"### 클라이언트 {Device.DeviceID}번 카메라 연결..... -> {Device.RTSPAddress} ###");

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
                            ClearRTSP();
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
                            Bitmap bitmap;
                            FailedDecodeNextFrame = 0;

                            try
                            {
                                AVFrame targetFrame = VideoFrameConverter.Convert(Decodeframe);

                                bitmap = new Bitmap
                                (
                                    targetFrame.width, 
                                    targetFrame.height, 
                                    targetFrame.linesize[0], 
                                    System.Drawing.Imaging.PixelFormat.Format24bppRgb, 
                                    (IntPtr)targetFrame.data[0]
                                );
                            }
                            catch (Exception ee)
                            {
                                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                                throw;
                            }

                            if (bitmap == null) continue;
                            if (onDisplayStream != null) onDisplayStream(bitmap);

                            Thread.Sleep(33);
                        }
                    }
                }
            }
        }
    }
}
