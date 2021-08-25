using System;
using System.Windows;
using System.Windows.Controls;

namespace VMSViewer
{
    /// <summary>
    /// ClientList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ClientList : UserControl
    {
        public ClientList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitProc();
        }

        private void InitProc()
        {

        }
    }
}
