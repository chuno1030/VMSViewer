using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        /// 장치
        /// </summary>
        private Device SelectedDevice;

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
            ClearDevice();
        }

        /// <summary>
        /// 드래그 앤 드랍 처리
        /// </summary>
        private void UserControl_PreviewDrop(object sender, DragEventArgs e)
        {
           if(e.Data.GetDataPresent("Device"))
           {
                var Device = e.Data.GetData("Device") as Device;
                if (Device == null) return;

                InitDevice(Device);
           }
        }

        private void InitProc()
        {
            SetViewer(MoniterType.NONE);
        }

        /// <summary>
        /// 클라이언트 스트리밍
        /// </summary>
        private void InitDevice(Device Device)
        {
            SelectedDevice = Device;
            SetViewer(MoniterType.RTSP, Device);

            if (RTSP == null)
            {
                RTSP = new RTSP(Device);
                RTSP.onDisplayStream += RTSP_onDisplayStream;
                RTSP.onConnectionStatus += RTSP_onConnectionStatus;
                RTSP.InitRTSP();
            }
        }

        private void ClearDevice()
        {
            if (RTSP != null)
            {
                RTSP.onDisplayStream -= RTSP_onDisplayStream;
                RTSP.onConnectionStatus -= RTSP_onConnectionStatus;
                RTSP.Disconnect();
                RTSP = null;
            }

            ClearViewer();
            GC.Collect();
        }

        private void RTSP_onDisplayStream(System.Drawing.Bitmap Bitmap)
        {
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
            {
                imgViewer.Source = ConvertBitmapImage(Bitmap);
            }));
        }

        private void RTSP_onConnectionStatus(ConnectionStatus ConnectionStatus)
        {
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
            {
                txtConnectionStatus.Text = "";

                switch (ConnectionStatus)
                {
                    case ConnectionStatus.Connecting:
                        txtConnectionStatus.Text = "[연결 중]";
                        break;
                    case ConnectionStatus.Connected:
                        txtConnectionStatus.Text = "[연결]";
                        txtConnectionStatus.Foreground = Brushes.Green;
                        break;
                    case ConnectionStatus.Disconnected:
                        txtConnectionStatus.Text = "[연결실패]";
                        txtConnectionStatus.Foreground = Brushes.Red;
                        break;
                }
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetViewer(MoniterType MoniterType, Device Device = null)
        {
            imgBackGround.Visibility = Visibility.Hidden;

            gridViewer.Visibility = Visibility.Hidden;
            imgViewer.Visibility = Visibility.Hidden;

            txtConnectionStatus.Text = "";

            switch (MoniterType)
            {
                case MoniterType.NONE:
                    txtDeviceName.Text = "";
                    MoniterType = MoniterType.NONE;
                    imgBackGround.Visibility = Visibility.Visible;
                    break;
                case MoniterType.RTSP:
                    txtDeviceName.Text = Device.DeviceName;
                    gridViewer.Visibility = Visibility.Visible;
                    imgViewer.Visibility = Visibility.Visible;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearViewer()
        {
            SelectedDevice = null;
            imgViewer.Source = null;
            SetViewer(MoniterType.NONE);
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
                case "DeviceInfo":
                    ShowDeviceInfo();
                    break;
                case "FullScreen":
                    ShowFullScreen();
                    break;
                case "DeleteDevice":
                    DeleteDevice();
                    break;
            }
        }

        private void ShowDeviceInfo()
        {
            if (RTSP != null && RTSP.IsConnect())
            {

            }
            else
                MessageBox.Show("연결된 장치가 없습니다.", "장치정보", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowFullScreen()
        {
            if (RTSP != null && RTSP.IsConnect())
            {
                RTSP.IsFullScreen = true;
            }
            else
                MessageBox.Show("연결된 장치가 없습니다.", "전체화면", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DeleteDevice()
        {
            if (RTSP != null)
                ClearDevice();
        }

        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            menuGrid.Visibility = Visibility.Visible;
        }

        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            menuGrid.Visibility = Visibility.Hidden;
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
    }
}
