using System;

namespace VMSViewer
{
    /// <summary>
    /// 프로그램 타이틀 바 버튼타입
    /// </summary>
    public enum TitleBarButtonType
    {
        Minimize = 1,
        Maxmize,
        Close
    }

    /// <summary>
    /// 뷰어화면타입
    /// </summary>
    public enum MoniterType
    {
        /* 미등록 */
        NONE = 0,
        /* 등록*/
        RTSP = 1
    }

    /// <summary>
    /// 카메라 연결타입
    /// </summary>
    public enum ConnectionStatus
    {
        Connecting = 0,
        Connected,
        Disconnected,
    }
}
