using System;
using System.Windows;

using VMSViewer.Module;

namespace VMSViewer
{
    /// <summary>
    /// EditClientWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditClientWindow : Window
    {
        /// <summary>
        /// Client 변수가 NULL 아니고, ClientID가 0보다 클 시 TRUE
        /// </summary>
        private readonly bool IsEdit;

        /// <summary>
        /// 등록/수정의 그룹 ID
        /// </summary>
        private readonly int ClientGroupID;

        /// <summary>
        /// 수정 시 사용할 Client
        /// </summary>
        private Client Client { get; }

        public EditClientWindow(int ClientGroupID, Client Client = null)
        {
            InitializeComponent();

            this.ClientGroupID = ClientGroupID;

            if(Client != null)
            {
                this.Client = Client;
                IsEdit = true;
            }
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
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
                if (Client != null && ClientGroupID != Client.ClientGroupID)
                { 
                    System.Windows.MessageBox.Show("장치그룹ID와 장치ID가 일치하지 않습니다.", "장치수정", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }

                this.Title = "장치수정";

                txtClientName.Text = Client.ClientName.Trim();
                txtClientIP.Text = Client.ClientIP.Trim();
                txtRTSPAddress.Text = Client.RTSPAddress.Trim();
            }
            else 
                this.Title = "장치생성";
        }

        private void btnClick(object sender, RoutedEventArgs e)
        {
            if (IsEdit)
            {
                Client EditClient = Client;
                EditClient.ClientIP = txtClientIP.Text.Trim();
                EditClient.ClientName = txtClientName.Text.Trim();
                EditClient.RTSPAddress = txtRTSPAddress.Text.Trim();

                if (DatabaseManager.Shared.IsUseClientName(IsEdit, EditClient) == false)
                {
                    if (DatabaseManager.Shared.UPDATE_TB_Client(EditClient))
                        EventManager.RefreshClientEvent(EditClient);
                    else
                        System.Windows.MessageBox.Show("수정에 실패했습니다.", "장치수정", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    System.Windows.MessageBox.Show("입력하신 장치명은 해당 그룹에서 사용 중입니다.", "장치수정", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Client NewClient = new Client(this.ClientGroupID);
                NewClient.ClientIP = txtClientIP.Text.Trim();
                NewClient.ClientName = txtClientName.Text.Trim();
                NewClient.RTSPAddress = txtRTSPAddress.Text.Trim();

                if(DatabaseManager.Shared.IsUseClientName(IsEdit, NewClient) == false)
                {
                    if (DatabaseManager.Shared.INSERT_TB_Client(NewClient))
                        EventManager.AddClientEvent(NewClient);
                    else
                        System.Windows.MessageBox.Show("생성에 실패했습니다.", "장치생성", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    System.Windows.MessageBox.Show("입력하신 장치명은 해당 그룹에서 사용 중입니다.", "장치생성", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
