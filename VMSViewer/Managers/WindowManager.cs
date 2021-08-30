using System;
using System.Windows;
using System.Linq;
using System.Collections.Generic;

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
        /// 그룹 등록/수정 윈도우
        /// </summary>
        private EditClientGroupWindow EditClientGroupWindow = null;

        public WindowManager()
        {

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
        public void ShowEditClientGroupWindow()
        {
            if (EditClientGroupWindow == null)
            {
                EditClientGroupWindow = new EditClientGroupWindow();
                EditClientGroupWindow.Closed += EditClientGroupWindow_Closed;
                EditClientGroupWindow.Show();
            }
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
    }
}
