using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using FFmpeg.AutoGen;

namespace VMSViewer
{
    public sealed unsafe class VideoStreamDecoder : IDisposable
    {
        private AVCodecContext* _pCodecContext;
        private AVFormatContext* _pFormatContext;
        private AVFrame* _pFrame;
        private AVFrame* _receivedFrame;
        private AVPacket* _pPacket;
        private int _streamIndex;

        /// <summary>
        /// 해당 카메라 코덱
        /// </summary>
        public string CodecName { get; set; }
        /// <summary>
        /// 해당 카메라 사이즈
        /// </summary>
        public Size FrameSize { get; set; }
        /// <summary>
        /// 해당 카메라 픽셀포맷
        /// </summary>
        public AVPixelFormat PixelFormat { get; set; }

        /// <summary>
        /// 해당 카메라 연결상태
        /// </summary>
        private bool IsConnected;

        public VideoStreamDecoder() { }

        public void Dispose()
        {
            if (IsConnected == false) return;

            ffmpeg.av_frame_unref(_pFrame);
            ffmpeg.av_free(_pFrame);

            ffmpeg.av_packet_unref(_pPacket);
            ffmpeg.av_free(_pPacket);

            ffmpeg.avcodec_close(_pCodecContext);
            var pFormatContext = _pFormatContext;

            if(pFormatContext != null)
                ffmpeg.avformat_close_input(&pFormatContext);
        }

        public bool Connect(string RTSPAddress)
        {
            try
            {
                /* 연결할때 옵션 avformat_open_input() 사용할때 4번째 매개변수에 넣는다. */
                AVDictionary* opts = null;

                /* Default udp. Set tcp Interleaved Mode */
                ffmpeg.av_dict_set(&opts, "rtsp_transport", "tcp", 0);
                /* 연결 타임아웃(6초) */
                ffmpeg.av_dict_set(&opts, "stimeout", "6000000", 0);

                _pFormatContext = ffmpeg.avformat_alloc_context();

                var pFormatContext = _pFormatContext;
                /* 파일(스트림)을 열고 해더를 읽습니다. */
                ffmpeg.avformat_open_input(&pFormatContext, RTSPAddress, null, &opts).ThrowExceptionIfError();

                /* 열어놓은 미디어 파일의 스트림 정보를 가져온다 */
                ffmpeg.avformat_find_stream_info(_pFormatContext, null).ThrowExceptionIfError();
                AVCodec* codec = null;
                /* AVMeidaType의 스트림 정보를 가져온다. */
                _streamIndex = ffmpeg.av_find_best_stream(_pFormatContext, AVMediaType.AVMEDIA_TYPE_VIDEO, -1, -1, &codec, 0).ThrowExceptionIfError();
                /* 코덱을 이용하여 AVCodecContext를 할당 필드를 디폴트로 설정하고 포인터를 반환합니다. */
                _pCodecContext = ffmpeg.avcodec_alloc_context3(codec);

                /* AVCodecParameters을 기반으로 AVCodecContext를 채웁니다. */
                ffmpeg.avcodec_parameters_to_context(_pCodecContext, _pFormatContext->streams[_streamIndex]->codecpar).ThrowExceptionIfError();
                /* AVCodecContext를 주어진 AVCodec을 이용해 초기화 합니다. 이 작업을 하기전에 avcodec_alloc_context3()가 먼저 처리되야합니다. */
                ffmpeg.avcodec_open2(_pCodecContext, codec, null).ThrowExceptionIfError();

                /* 연결된 코덱정보를 가져옵니다.*/
                CodecName = ffmpeg.avcodec_get_name(codec->id);
                /* 해당 카메라 사이즈 */
                FrameSize = new Size(_pCodecContext->width, _pCodecContext->height);
                /* 해당 카메라 픽셀 포맷 */
                PixelFormat = _pCodecContext->pix_fmt;

                /* AVPacket을 메모리에 할당하고 필드를 기본값으로 설정합니다. */
                _pPacket = ffmpeg.av_packet_alloc();

                /* AVFrame을 메모리에 할당하고 필드를 기본값으로 설정합니다. */
                _pFrame = ffmpeg.av_frame_alloc();
                _receivedFrame = ffmpeg.av_frame_alloc();

                IsConnected = true;

                return true;
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));

                IsConnected = false;

                return false;
            }
        }

        public bool IsConnect()
        {
            try
            {
                /* 연결 실패시 널포인터이므로 비교 후 False 리턴 */
                if (_pCodecContext == null) return false;

                int IsConnect = ffmpeg.avcodec_is_open(_pCodecContext);
                return Convert.ToBoolean(IsConnect);
            }
            catch (AccessViolationException ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));

                return false;
            }
        }

        /// <summary>
        /// 다음 프레임 읽기_OLD
        /// </summary>
        public bool TryDecodeNextFrame_OLD(out AVFrame frame)
        {
            /* 프레임이 참조하는 버퍼를 참조 해제하고 나머지 프레임 필드를 기본값으로 재설정 */
            ffmpeg.av_frame_unref(_pFrame);
            ffmpeg.av_frame_unref(_receivedFrame);

            int error;

            do
            {
                try
                {
                    do
                    {
                        /* 스트림의 다음의 프레임을 리턴합니다. */
                        error = ffmpeg.av_read_frame(_pFormatContext, _pPacket);
                        if (error == ffmpeg.AVERROR_EOF)
                        {
                            frame = *_pFrame;
                            return false;
                        }

                        error.ThrowExceptionIfError();
                    } while (_pPacket->stream_index != _streamIndex);

                    /* 패킷 데이터를 디코더에 입력합니다. */
                    ffmpeg.avcodec_send_packet(_pCodecContext, _pPacket).ThrowExceptionIfError();

                }
                finally
                {
                    /* 패킷이 참조하는 버퍼를 참조 해제하고 나머지 패킷 필드를 기본값으로 재설정 */
                    ffmpeg.av_packet_unref(_pPacket);
                }

                /* 디코더에서 디코딩 된 프레임을 반환합니다. */
                error = ffmpeg.avcodec_receive_frame(_pCodecContext, _pFrame);
            } while (error == ffmpeg.AVERROR(ffmpeg.EAGAIN));
            error.ThrowExceptionIfError();
            if (_pCodecContext->hw_device_ctx != null)
            {
                ffmpeg.av_hwframe_transfer_data(_receivedFrame, _pFrame, 0).ThrowExceptionIfError();
                frame = *_receivedFrame;
            }
            else
            {
                frame = *_pFrame;
            }

            return true;
        }

        /// <summary>
        /// 다음 프레임 읽기
        /// </summary>
        public bool TryDecodeNextFrame(out AVFrame frame)
        {
            ffmpeg.av_frame_unref(this._pFrame);
            int error;

            // repeated-try avcodec_receive_frame until enough packets haven been send
            while (true)
            {
                //읽기 프레임을 시도합니다. 마지막 avcodec_send_packet으로 인해 여러 프레임을 읽을 수 있습니다. 더 많은 패킷이 필요한 경우 EGAIN으로 응답합니다.
                error = ffmpeg.avcodec_receive_frame(this._pCodecContext, this._pFrame);
                if (error != ffmpeg.AVERROR(ffmpeg.EAGAIN))
                {
                    frame = *this._pFrame;
                    if (error == ffmpeg.AVERROR_EOF)
                        return false;

                    error.ThrowExceptionIfError();
                    return true;
                }

                try
                {
                    // feed all stream-matching packets to decoder
                    while (true)
                    {
                        error = ffmpeg.av_read_frame(this._pFormatContext, this._pPacket);

                        if (error == ffmpeg.AVERROR_EOF)
                        {
                            // no more packets to read -> trigger draining/flushing of remaining frames
                            ffmpeg.avcodec_send_packet(this._pCodecContext, null).ThrowExceptionIfError();
                            break; // don't throw error, just exit packet-feed-loop
                        }

                        error.ThrowExceptionIfError();

                        if (this._pPacket->stream_index == this._streamIndex)
                        {
                            var sendPacketResult = ffmpeg.avcodec_send_packet(this._pCodecContext, this._pPacket);
                            if (sendPacketResult == 0)
                                break;

                            // no reason to abort/crash (it was just an invalid packet from demuxer) -> just retrieve and feed next packet; most of time the codec will resume without problems
                            //var errorMsg = FFmpegHelper.av_strerror(sendPacketResult);
                            //Trace.TraceError(errorMsg);
                        }

                        // unref all packets, no matter if sent to avcodec or ignored
                        ffmpeg.av_packet_unref(this._pPacket);
                    }
                }
                finally
                {
                    ffmpeg.av_packet_unref(this._pPacket);
                }
            }
        }

        public bool TryDecodeNextPacket(bool IsFrame)
        {
            bool IsRead = false;
            int error;

            try
            {
                error = ffmpeg.av_read_frame(this._pFormatContext, this._pPacket);
                // feed all stream-matching packets to decoder
                while (true)
                {
                    if (error == ffmpeg.AVERROR_EOF)
                    {
                        //no more packets to read -> trigger draining/flushing of remaining frames
                        ffmpeg.avcodec_send_packet(this._pCodecContext, null).ThrowExceptionIfError();
                        break; // don't throw error, just exit packet-feed-loop
                    }

                    error.ThrowExceptionIfError();

                    if (this._pPacket->stream_index == this._streamIndex && IsFrame)
                    {
                        var sendPacketResult = ffmpeg.avcodec_send_packet(this._pCodecContext, this._pPacket);
                        
                        if(sendPacketResult == 0)
                        {
                            IsRead = true;
                            break;
                        }
                    }
                    else
                    {
                        ffmpeg.av_packet_unref(this._pPacket);
                        IsRead = false;
                        break;
                    }

                    //unref all packets, no matter if sent to avcodec or ignored
                    ffmpeg.av_packet_unref(this._pPacket);
                }

                return IsRead;
            }
            finally
            {
                ffmpeg.av_packet_unref(this._pPacket);
            }
        }

        public bool TryDecodeNextFrame_Test(out AVFrame frame)
        {
            ffmpeg.av_frame_unref(this._pFrame);
            int error = 0;

            error = ffmpeg.avcodec_receive_frame(this._pCodecContext, this._pFrame);
            frame = *this._pFrame;

            if (error != ffmpeg.AVERROR(ffmpeg.EAGAIN))
            {
                if (error == ffmpeg.AVERROR_EOF)
                    return false;

                error.ThrowExceptionIfError();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 연결된 카메라정보 
        /// </summary>
        public IReadOnlyDictionary<string, string> GetContextInfo()
        {
            AVDictionaryEntry* tag = null;
            var result = new Dictionary<string, string>();
            while ((tag = ffmpeg.av_dict_get(_pFormatContext->metadata, "", tag, ffmpeg.AV_DICT_IGNORE_SUFFIX)) != null)
            {
                var key = Marshal.PtrToStringAnsi((IntPtr)tag->key);
                var value = Marshal.PtrToStringAnsi((IntPtr)tag->value);
                result.Add(key, value);
            }

            return result;
        }
    }
}