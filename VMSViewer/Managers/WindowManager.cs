using System;
using System.Windows;
using System.Linq;
using System.Collections.Generic;

using VMSViewer.Module;

namespace VMSViewer
{
    public class WindowManager
    {
        private static WindowManager _shared;

        public static WindowManager Shared
        {
            get
            {
                if(_shared == null)
                    _shared = new WindowManager();

                return _shared;
            }
        }

        /// <summary>
        /// 로그인 윈도우
        /// </summary>
        private LoginWindow LoginWindow = null;

        /// <summary>
        /// 그룹 생성/수정 윈도우
        /// </summary>
        private EditDeviceGroupWindow EditDeviceGroupWindow = null;

        /// <summary>
        /// 장치 생성/수정 윈도우
        /// </summary>
        private EditDeviceWindow EditDeviceWindow = null;

        public WindowManager()
        {

        }

        public void AllWindowClose()
        {
            if (EditDeviceWindow != null) EditDeviceWindow.Close();
            if (EditDeviceGroupWindow != null) EditDeviceGroupWindow.Close();
        }

        #region LoginWindow
        public void ShowLoginWindow()
        {
            if(LoginWindow == null)
            {
                LoginWindow = new LoginWindow();
                LoginWindow.Closed += LoginWindow_Closed;
                LoginWindow.Show();
            }
        }

        private void LoginWindow_Closed(object sender, EventArgs e)
        {
            if(LoginWindow != null)
            {
                LoginWindow.Closed -= LoginWindow_Closed;
                LoginWindow = null;
            }

            GC.Collect();
        }

        #endregion

        public void ShowEditDeviceGroupWindow(DeviceGroup DeviceGroup = null)
        {
            if (DatabaseManager.Shared.IsOverCountDeviceGroup() == false)
            {
                if (EditDeviceGroupWindow == null)
                {
                    EditDeviceGroupWindow = new EditDeviceGroupWindow(DeviceGroup);
                    EditDeviceGroupWindow.Closed += EditDeviceGroupWindow_Closed;
                    EditDeviceGroupWindow.Show();
                }
            }
            else
                System.Windows.MessageBox.Show("최대 그룹생성 개수는 50개입니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void EditDeviceGroupWindow_Closed(object sender, EventArgs e)
        {
            if (EditDeviceGroupWindow != null)
            {
                EditDeviceGroupWindow.Closed -= EditDeviceGroupWindow_Closed;
                EditDeviceGroupWindow = null;
            }

            GC.Collect();
        }

        public void ShowEditDeviceWindow(int DeviceGroupID, Device Device = null)
        {
            if (DatabaseManager.Shared.IsOverCountDevice(DeviceGroupID) == false)
            {
                if (EditDeviceWindow == null)
                {
                    EditDeviceWindow = new EditDeviceWindow(DeviceGroupID, Device);
                    EditDeviceWindow.Closed += EditDeviceWindow_Closed;
                    EditDeviceWindow.Show();
                }
            }
            else
                System.Windows.MessageBox.Show("최대 그룹별 장치생성 개수는 50개입니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void EditDeviceWindow_Closed(object sender, EventArgs e)
        {
            if (EditDeviceWindow != null)
            {
                EditDeviceWindow.Closed -= EditDeviceWindow_Closed;
                EditDeviceWindow = null;
            }

            GC.Collect();
        }
    }
}
