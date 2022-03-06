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

                InitClient(client);
           }
        }

        private void InitProc()
        {

        }

        /// <summary>
        /// 클라이언트 
        /// </summary>
        private void InitClient(Client Client)
        {
            if(RTSP == null)
            {
                RTSP = new RTSP(Client);
                RTSP.InitRTSP();
            }
        }
    }
}
