using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VMSViewer.Module;

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

            EventManager.onRefreshClientGroup += EventManager_onRefreshClientGroup;
        }

        private void EventManager_onRefreshClientGroup(int RefreshID = -1)
        {
            if (RefreshID == -1) GetClientGroupList();
        }

        private void InitProc()
        {
            GetClientGroupList();
        }

        private void GetClientGroupList()
        {
            //List<ClientGroup> ClientGroupList = DatabaseManager.Shared.SELECT_TB_ClientGroup();

            ClientGroup clientGroup1 = new ClientGroup();
            clientGroup1.ClientGroupID = 1;
            clientGroup1.ClientGroupName = "MariaDB";

            List<ClientGroup> ClientGroupList = new List<ClientGroup>();

            ClientGroupList.Add(clientGroup1);

            if (ClientGroupList == null || ClientGroupList.Count < 0) return;

            foreach (var ClientGroup in ClientGroupList)
            {
                Expander expander = new Expander();
                expander.Height = 30;
                expander.Header = ClientGroup.ClientGroupName;

                expander.ContextMenu = new ContextMenu();

                MenuItem menuItem1 = new MenuItem();
                menuItem1.Tag = ClientGroup.ClientGroupID;
                menuItem1.Header = "그룹편집";
                menuItem1.Click += Expander_MenuItem_Edit_Click;

                expander.ContextMenu.Items.Add(menuItem1);

                expander.ContextMenu.Items.Add(new Separator());

                MenuItem menuItem2 = new MenuItem();
                menuItem1.Tag = ClientGroup.ClientGroupID;
                menuItem2.Header = "그룹삭제";
                menuItem2.Click += Expander_MenuItem_Remove_Click;

                expander.ContextMenu.Items.Add(menuItem2);

                List<Client> ClientList = GetClientList(ClientGroup.ClientGroupID);

                if (ClientList != null || ClientList.Count > 0)
                {
                    ListBox listBox = new ListBox();
                    listBox.AllowDrop = true;
                    listBox.ContextMenu = new ContextMenu();
                }

                spClientList.Children.Add(expander);
            }
        }

        private List<Client> GetClientList(int Group_ID)
        {
            return null;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            WindowManager.Shared.ShowEditClientGroupWindow();
        }

        private void Expander_MenuItem_Edit_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (menuItem == null) return;
            if (menuItem.Tag == null) return;

            int Tag = Convert.ToInt32(menuItem.Tag);
        }

        private void Expander_MenuItem_Remove_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (menuItem == null) return;
            if (menuItem.Tag == null) return;

            int Tag = Convert.ToInt32(menuItem.Tag);
        }
    }
}
