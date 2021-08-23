using System;
using System.Windows;

namespace VMSViewer
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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

        private void InitProc()
        {

        }

        private void DoFinal()
        {

        }
    }
}
