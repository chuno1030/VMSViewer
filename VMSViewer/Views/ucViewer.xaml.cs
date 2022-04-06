using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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

        /// <summary>
        /// 
        /// </summary>
        private MoniterType MoniterType;

        public ucViewer()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Closing += UserControl_Closing;

            InitProc();
        }

        private void UserControl_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(RTSP != null)
            {
                RTSP.onDisplayStream -= RTSP_onDisplayStream;
                RTSP.Disconnect();
                RTSP.Dispose();
                RTSP = null;
            }

            GC.Collect();
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
                InitClient(client);
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
                    MoniterType = MoniterType.NONE;
                    imgBackGround.Visibility = Visibility.Visible;
                    break;
                case MoniterType.RTSP:
                    MoniterType = MoniterType.RTSP;
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
                RTSP.onDisplayStream += RTSP_onDisplayStream;
                RTSP.InitRTSP();
            }
        }

        private BitmapImage ConvertBitmapImage(System.Drawing.Bitmap bitmap)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, System.IO.SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        private void RTSP_onDisplayStream(System.Drawing.Bitmap Bitmap)
        {
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
            {
                imgViewer.Source = ConvertBitmapImage(Bitmap);
            }));
        }

        private void imgViewer_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        private void imgViewer_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        private void btnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn == null) return;
            if (btn.Tag == null) return;

            string tag = btn.Tag.ToString();

            switch (tag)
            {
                case "GetClientInfo":
                    break;
                case "FullScreen":
                    break;
                case "DeleteClient":
                    break;
            }
        }

        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            menuGrid.Visibility = Visibility.Visible;

        }

        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            menuGrid.Visibility = Visibility.Hidden;

        }
    }
}
