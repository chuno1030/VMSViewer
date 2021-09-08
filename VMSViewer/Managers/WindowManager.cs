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
        private EditClientGroupWindow EditClientGroupWindow = null;

        /// <summary>
        /// 장치 생성/수정 윈도우
        /// </summary>
        private EditClientWindow EditClientWindow = null;

        public WindowManager()
        {

        }

        public void AllWindowClose()
        {
            if (EditClientWindow != null) EditClientWindow.Close();
            if (EditClientGroupWindow != null) EditClientGroupWindow.Close();
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

        public void ShowEditClientGroupWindow(ClientGroup ClientGroup = null)
        {
            if (DatabaseManager.Shared.IsOverCountClientGroup() == false)
            {
                if (EditClientGroupWindow == null)
                {
                    EditClientGroupWindow = new EditClientGroupWindow(ClientGroup);
                    EditClientGroupWindow.Closed += EditClientGroupWindow_Closed;
                    EditClientGroupWindow.Show();
                }
            }
            else
                System.Windows.MessageBox.Show("최대 그룹생성 개수는 50개입니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void EditClientGroupWindow_Closed(object sender, EventArgs e)
        {
            if (EditClientGroupWindow != null)
            {
                EditClientGroupWindow.Closed -= EditClientGroupWindow_Closed;
                EditClientGroupWindow = null;
            }

            GC.Collect();
        }

        public void ShowEditClientWindow(int ClientGroupID, Client Client = null)
        {
            if (DatabaseManager.Shared.IsOverCountClient(ClientGroupID) == false)
            {
                if (EditClientWindow == null)
                {
                    EditClientWindow = new EditClientWindow(ClientGroupID, Client);
                    EditClientWindow.Closed += EditClientWindow_Closed;
                    EditClientWindow.Show();
                }
            }
            else
                System.Windows.MessageBox.Show("최대 그룹별 장치생성 개수는 50개입니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void EditClientWindow_Closed(object sender, EventArgs e)
        {
            if (EditClientWindow != null)
            {
                EditClientWindow.Closed -= EditClientWindow_Closed;
                EditClientWindow = null;
            }

            GC.Collect();
        }
    }
}
