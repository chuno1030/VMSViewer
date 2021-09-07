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
        /// <summary>
        /// ClientGroup 변수가 NULL 아니고, GroupID가 0보다 클 시 TRUE
        /// </summary>
        private readonly bool IsEdit;

        /// <summary>
        /// 수정 시 사용할 ClientGroup
        /// </summary>
        private ClientGroup ClientGroup { get; }

        public EditClientGroupWindow(ClientGroup ClientGroup)
        {
            InitializeComponent();

            if (ClientGroup != null && ClientGroup.ClientGroupID > 0)
            {
                this.ClientGroup = ClientGroup;
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
                txtGroupName.Text = ClientGroup.ClientGroupName.Trim();
            }
            else
                this.Title = "그룹생성";
        }

        private void btnClick(object sender, RoutedEventArgs e)
        {

            if(IsEdit)
            {
                ClientGroup EditClientGroup = ClientGroup;
                EditClientGroup.ClientGroupName = txtGroupName.Text.Trim();

                if (DatabaseManager.Shared.UPDATE_TB_ClientGroup(EditClientGroup))
                    EventManager.RefreshClientGroupEvent(EditClientGroup);
                else
                    System.Windows.MessageBox.Show("수정에 실패했습니다.", "그룹수정", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ClientGroup NewClientGroup = new ClientGroup();
                NewClientGroup.ClientGroupName = txtGroupName.Text.Trim();

                if (DatabaseManager.Shared.INSERT_TB_ClientGroup(NewClientGroup))
                    EventManager.AddClientGroupEvent(NewClientGroup);
                else
                    System.Windows.MessageBox.Show("생성에 실패했습니다.", "그룹생성", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
