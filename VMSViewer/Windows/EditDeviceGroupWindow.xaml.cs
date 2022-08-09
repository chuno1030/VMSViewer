using System;
using System.Windows;

using VMSViewer.Module;

namespace VMSViewer
{
    /// <summary>
    /// EditDeviceGroupWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditDeviceGroupWindow : Window
    {
        /// <summary>
        /// DeviceGroup 변수가 NULL 아니고, GroupID가 0보다 클 시 TRUE
        /// </summary>
        private readonly bool IsEdit;

        /// <summary>
        /// 수정 시 사용할 DeviceGroup
        /// </summary>
        private DeviceGroup DeviceGroup { get; }

        public EditDeviceGroupWindow(DeviceGroup DeviceGroup)
        {
            InitializeComponent();

            if (DeviceGroup != null && DeviceGroup.DeviceGroupID > 0)
            {
                this.DeviceGroup = DeviceGroup;
                IsEdit = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
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
                this.Title = "그룹수정";
                txtGroupName.Text = DeviceGroup.DeviceGroupName.Trim();
            }
            else
                this.Title = "그룹생성";
        }

        private void btnClick(object sender, RoutedEventArgs e)
        {

            if(IsEdit)
            {
                DeviceGroup EditDeviceGroup = DeviceGroup;
                EditDeviceGroup.DeviceGroupName = txtGroupName.Text.Trim();

                if(DatabaseManager.Shared.IsUseDeviceGroupName(IsEdit, EditDeviceGroup) == false)
                {
                    if (DatabaseManager.Shared.UPDATE_TB_DeviceGroup(EditDeviceGroup))
                        EventManager.RefreshDeviceGroupEvent(EditDeviceGroup);
                    else
                        System.Windows.MessageBox.Show("수정에 실패했습니다.", "그룹수정", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    System.Windows.MessageBox.Show("입력하신 그룹명은 사용 중입니다.", "그룹수정", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DeviceGroup NewDeviceGroup = new DeviceGroup();
                NewDeviceGroup.DeviceGroupName = txtGroupName.Text.Trim();

                if (DatabaseManager.Shared.IsUseDeviceGroupName(IsEdit, NewDeviceGroup) == false)
                {
                    if (DatabaseManager.Shared.INSERT_TB_DeviceGroup(NewDeviceGroup))
                        EventManager.AddDeviceGroupEvent(NewDeviceGroup);
                    else
                        System.Windows.MessageBox.Show("생성에 실패했습니다.", "그룹생성", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    System.Windows.MessageBox.Show("입력하신 그룹명은 사용 중입니다.", "그룹생성", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
