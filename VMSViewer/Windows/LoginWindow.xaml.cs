using System;
using System.Windows;
using System.Windows.Input;

using VMSViewer.Module;

namespace VMSViewer
{
    /// <summary>
    /// LoginWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitProc();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DoFinal();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            /* 엔터 키 입력 시 로그인 */
            if(e.Key == Key.Enter)
            {
                Login();
                base.OnKeyDown(e);
            }
        }

        private void InitProc()
        {
            this.Title = Environment.MachineName;
        }

        private void DoFinal()
        {

        }

        private void Login()
        {
            string LoginID = txtID.Text.Trim();
            string LoginPassword = pwbPassword.Password.Trim();

            if(string.IsNullOrWhiteSpace(LoginID))
            {
                System.Windows.MessageBox.Show("아이디를 입력해주세요.", "로그인", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (string.IsNullOrWhiteSpace(LoginPassword))
            {
                System.Windows.MessageBox.Show("비밀번호를 입력해주세요.", "로그인", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            DatabaseManager.Shared.IsAccount(LoginID, LoginPassword);
        }

        private void btnClick(object sender, RoutedEventArgs e)
        {
            Login();
        }
    }
}
