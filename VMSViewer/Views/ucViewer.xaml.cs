using System;
using System.Windows;
using System.Windows.Controls;

using VMSViewer.Module;

namespace VMSViewer
{
    /// <summary>
    /// ucViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ucViewer : UserControl
    {
        /// <summary>
        /// RTSP 스트리밍 
        /// </summary>
        private RTSP RTSP = null;

        public ucViewer()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitProc();
        }

        /// <summary>
        /// 드래그 앤 드랍 처리
        /// </summary>
        private void UserControl_PreviewDrop(object sender, DragEventArgs e)
        {
           if(e.Data.GetDataPresent("Client"))
           {
                var client = e.Data.GetData("Client") as Client;

                if (client == null) return;

                SetViewer(MoniterType.RTSP);
                //InitClient(client);
           }
        }

        private void InitProc()
        {
            SetViewer(MoniterType.NONE);
        }

        /// <summary>
        /// 뷰어화면 SET
        /// </summary>
        private void SetViewer(MoniterType MoniterType)
        {
            imgBackGround.Visibility = Visibility.Hidden;

            gridViewer.Visibility = Visibility.Hidden;
            imgViewer.Visibility = Visibility.Hidden;

            switch (MoniterType)
            {
                case MoniterType.NONE:
                    imgBackGround.Visibility = Visibility.Visible;
                    break;
                case MoniterType.RTSP:
                    gridViewer.Visibility = Visibility.Visible;
                    imgViewer.Visibility = Visibility.Visible;
                    break;
            }
        }

        /// <summary>
        /// 클라이언트 스트리밍
        /// </summary>
        private void InitClient(Client Client)
        {
            if(RTSP == null)
            {
                RTSP = new RTSP(Client);
                RTSP.OnDisplayStream += RTSP_OnDisplayStream;
                RTSP.InitRTSP();
            }
        }

        /// <summary>
        /// 카메라 영상을 화면에 표출
        /// </summary>
        private void RTSP_OnDisplayStream(object sender, EventArgs e)
        {

        }
    }
}
