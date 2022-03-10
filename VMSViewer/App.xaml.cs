using System;
using System.Windows;
using System.Diagnostics;

namespace VMSViewer
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                if (IsProcess())
                {
                    if (FFMpegHelper.FFMpegInitialize() == false)
                        throw new Exception("FFMpeg 초기화 작업 오류.");

                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                else
                    throw new Exception("프로그램이 이미 실행 중입니다.");
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog(ee.StackTrace, ee.Message);
                System.Windows.MessageBox.Show($"{ee.Message}\n프로그램을 종료합니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// 중복실행 체크
        /// </summary>
        private bool IsProcess()
        {
            try
            {
                Process[] process = Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName);

                if (process.Length > 1)
                    return false;
                else
                    return true;
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog(ee.StackTrace, ee.Message);
                return false;
            }
        }
    }
}
