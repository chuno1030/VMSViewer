using System;
using System.Windows;

using VMSViewer.Module;

namespace VMSViewer
{
    /// <summary>
    /// EditDeviceWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditDeviceWindow : Window
    {
        /// <summary>
        /// Device 변수가 NULL 아니고, DeviceID가 0보다 클 시 TRUE
        /// </summary>
        private readonly bool IsEdit;

        /// <summary>
        /// 등록/수정의 그룹 ID
        /// </summary>
        private readonly int DeviceGroupID;

        /// <summary>
        /// 수정 시 사용할 Device
        /// </summary>
        private Device Device { get; }

        public EditDeviceWindow(int DeviceGroupID, Device Device = null)
        {
            InitializeComponent();

            this.DeviceGroupID = DeviceGroupID;

            if(Device != null)
            {
                this.Device = Device;
                IsEdit = true;
            }
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            InitProc();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void InitProc()
        {
            if (IsEdit)
            {
                if (Device != null && DeviceGroupID != Device.DeviceGroupID)
                { 
                    System.Windows.MessageBox.Show("장치그룹ID와 장치ID가 일치하지 않습니다.", "장치수정", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }

                this.Title = "장치수정";

                txtDeviceName.Text = Device.DeviceName.Trim();
                txtDeviceIP.Text = Device.DeviceIP.Trim();
                txtRTSPAddress.Text = Device.RTSPAddress.Trim();
            }
            else 
                this.Title = "장치생성";
        }

        private void btnClick(object sender, RoutedEventArgs e)
        {
            if (IsEdit)
            {
                Device EditDevice = Device;
                EditDevice.DeviceIP = txtDeviceIP.Text.Trim();
                EditDevice.DeviceName = txtDeviceName.Text.Trim();
                EditDevice.RTSPAddress = txtRTSPAddress.Text.Trim();

                if (DatabaseManager.Shared.IsUseDeviceName(IsEdit, EditDevice) == false)
                {
                    if (DatabaseManager.Shared.UPDATE_TB_Device(EditDevice))
                    {
                        EventManager.RefreshDeviceEvent(EditDevice);
                        this.Close();
                    }
                    else
                        System.Windows.MessageBox.Show("수정에 실패했습니다.", "장치수정", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    System.Windows.MessageBox.Show("입력하신 장치명은 해당 그룹에서 사용 중입니다.", "장치수정", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Device NewDevice = new Device(this.DeviceGroupID);
                NewDevice.DeviceIP = txtDeviceIP.Text.Trim();
                NewDevice.DeviceName = txtDeviceName.Text.Trim();
                NewDevice.RTSPAddress = txtRTSPAddress.Text.Trim();

                if(DatabaseManager.Shared.IsUseDeviceName(IsEdit, NewDevice) == false)
                {
                    if (DatabaseManager.Shared.INSERT_TB_Device(NewDevice))
                    {
                        EventManager.AddDeviceEvent(NewDevice);
                        this.Close();
                    }
                    else
                        System.Windows.MessageBox.Show("생성에 실패했습니다.", "장치생성", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    System.Windows.MessageBox.Show("입력하신 장치명은 해당 그룹에서 사용 중입니다.", "장치생성", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
