using System;
using System.Windows;

using VMSViewer.Module;

namespace VMSViewer
{
    /// <summary>
    /// EditClientGroupWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditClientGroupWindow : Window
    {
        public EditClientGroupWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void btnClick(object sender, RoutedEventArgs e)
        {
            ClientGroup NewClientGroup = new ClientGroup();
            NewClientGroup.ClientGroupName = txtGroupName.Text.Trim();

            if(DatabaseManager.Shared.INSERT_TB_ClientGroup(NewClientGroup))
            {
                EventManager.RefreshClientGroupEvent();
            }
            else
            {

            }
        }
    }
}
