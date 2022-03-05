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
    public partial class ucClientList : UserControl
    {
        /// <summary>
        /// 장치리스트가 보여질 경우 TRUE
        /// </summary>
        private bool IsOpenClientList = false;

        public ucClientList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitProc();

            EventManager.onAddClient += EventManager_onAddClient;
            EventManager.onAddClientGroup += EventManager_onAddClientGroup;
            EventManager.onRefreshClient += EventManager_onRefreshClient;
            EventManager.onRefreshClientGroup += EventManager_onRefreshClientGroup;
        }

        private void InitProc()
        {
            RefreshClientGroupList();
        }

        private void EventManager_onAddClient(Client NewClient)
        {
            Expander TargetClientList = null;
            ClientGroup TargetClientGroup = null;

            foreach (UIElement item in spClientGroupList.Children)
            {
                Expander expander = item as Expander;

                if (expander == null) continue;
                if (expander.Tag == null) continue;

                ClientGroup ClientGroup = (ClientGroup)expander.Tag;

                if (ClientGroup == null || ClientGroup is ClientGroup == false) continue;
                if (ClientGroup.ClientGroupID != NewClient.ClientGroupID) continue;

                TargetClientGroup = ClientGroup;
                TargetClientList = expander;
            }

            if (TargetClientList == null || TargetClientGroup == null)
            {
                System.Windows.MessageBox.Show("생성에 실패했습니다.", "장치생성", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ListBox listBox = (ListBox)TargetClientList.Content;

            if (listBox.Items.Count > 0) listBox.Items.Clear();

            TargetClientList.Content = RefreshClientList(DatabaseManager.Shared.SELECT_TB_Client(TargetClientGroup));
        }

        private void EventManager_onAddClientGroup(ClientGroup NewClientGroup)
        {
            RefreshClientGroupList();

            //Expander expander = GetExpander(NewClientGroup);

            //if (expander != null)
            //    spClientGroupList.Children.Add(expander);
            //else
            //    System.Windows.MessageBox.Show("생성에 실패했습니다.", "그룹생성", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void EventManager_onRefreshClient(Client RefreshClient = null)
        {
            ListBox TargetClientList = null;
            ListBoxItem TargetClient = null;

            foreach (UIElement item in spClientGroupList.Children)
            {
                Expander expander = item as Expander;

                if (expander == null) continue;
                if (expander.Tag == null) continue;

                ClientGroup ClientGroup = (ClientGroup)expander.Tag;

                if (ClientGroup == null || ClientGroup is ClientGroup == false) continue;
                if (ClientGroup.ClientGroupID != RefreshClient.ClientGroupID) continue;

                TargetClientList = (ListBox)expander.Content;
            }

            if (TargetClientList == null)
            {
                System.Windows.MessageBox.Show("편집에 실패했습니다.", "장치편집", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (ListBoxItem item in TargetClientList.Items)
            {
                if (item.Tag == null) continue;
                if (item.Tag is Client == false) continue;

                Client client = (Client)item.Tag;

                if (client == null) continue;

                if (client.ClientID != RefreshClient.ClientID) continue;

                TargetClient = item;
            }

            if (TargetClient == null)
            {
                System.Windows.MessageBox.Show("편집에 실패했습니다.", "장치편집", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TargetClient.Tag = RefreshClient;
            TargetClient.Content = RefreshClient.ClientName;
        }

        private void EventManager_onRefreshClientGroup(ClientGroup RefreshClientGroup)
        {
            Expander RefreshExpander = null;

            foreach (UIElement item in spClientGroupList.Children)
            {
                Expander expander = item as Expander;

                if (expander == null) continue;
                if (expander.Tag == null) continue;
                if (RefreshClientGroup == null) continue;

                ClientGroup ClientGroup = (ClientGroup)expander.Tag;

                if (ClientGroup == null || ClientGroup is ClientGroup == false) continue;
                if (RefreshClientGroup.ClientGroupID != ClientGroup.ClientGroupID) continue;

                RefreshExpander = expander;
            }

            RefreshExpander.Tag = RefreshClientGroup;
            RefreshExpander.Header = RefreshClientGroup.ClientGroupName.Trim();
        }

        private Expander GetExpander(ClientGroup ClientGroup)
        {
            if (ClientGroup == null) return null;

            Expander expander = new Expander();

            expander.Tag = ClientGroup;
            expander.Header = ClientGroup.ClientGroupName;
            expander.ContextMenu = new ContextMenu();

            MenuItem menuItem1 = new MenuItem();
            menuItem1.Tag = ClientGroup;
            menuItem1.Header = "장치생성";
            menuItem1.Click += Expander_MenuItem_AddClient_Click;

            MenuItem menuItem2 = new MenuItem();
            menuItem2.Tag = ClientGroup;
            menuItem2.Header = "그룹편집";
            menuItem2.Click += Expander_MenuItem_Edit_Click;

            MenuItem menuItem3 = new MenuItem();
            menuItem3.Tag = ClientGroup;
            menuItem3.Header = "그룹삭제";
            menuItem3.Click += Expander_MenuItem_Remove_Click;

            expander.ContextMenu.Items.Add(menuItem1);
            expander.ContextMenu.Items.Add(new Separator());
            expander.ContextMenu.Items.Add(menuItem2);
            expander.ContextMenu.Items.Add(menuItem3);

            return expander;
        }

        private ListBoxItem GetListBoxItem(Client Client)
        {
            if (Client == null) return null;

            ListBoxItem listBoxItem = new ListBoxItem() { Content = Client.ClientName, Tag = Client };
            listBoxItem.ContextMenu = new ContextMenu();

            MenuItem menuItem4 = new MenuItem() { Header = "장치편집", Tag = Client };
            menuItem4.Click += Client_MenuItem_Edit_Click;

            MenuItem menuItem5 = new MenuItem() { Header = "장치삭제", Tag = Client };
            menuItem5.Click += Client_MenuItem_Remove_Click;

            listBoxItem.ContextMenu.Items.Add(menuItem4);
            listBoxItem.ContextMenu.Items.Add(menuItem5);

            return listBoxItem;
        }

        private void ClearList()
        {
            foreach (UIElement item in spClientGroupList.Children)
            {
                Expander expander = item as Expander;

                if (expander == null) continue;
                if (expander.Tag == null) continue;

                ListBox listBox = (ListBox)expander.Content;

                if (listBox == null || listBox.Items.Count < 1) continue;

                listBox.Items.Clear();
            }

            spClientGroupList.Children.Clear();
        }

        private void RefreshClientGroupList()
        {
            ClearList();

            List<ClientGroup> ClientGroupList = DatabaseManager.Shared.SELECT_TB_ClientGroup();

            //ClientGroup clientGroup1 = new ClientGroup();
            //clientGroup1.ClientGroupID = 1;
            //clientGroup1.ClientGroupName = "MariaDB";

            //List<ClientGroup> ClientGroupList = new List<ClientGroup>();

            //ClientGroupList.Add(clientGroup1);

            if (ClientGroupList == null || ClientGroupList.Count < 0) return;

            foreach (var ClientGroup in ClientGroupList)
            {
                if (ClientGroup == null) continue;

                Expander expander = GetExpander(ClientGroup);

                if (expander == null) continue;

                expander.Content = RefreshClientList(DatabaseManager.Shared.SELECT_TB_Client(ClientGroup));
                spClientGroupList.Children.Add(expander);
            }
        }

        private ListBox RefreshClientList(List<Client> ClientList)
        {
            ListBox listBox = new ListBox();
            listBox.AllowDrop = true;
            listBox.ContextMenu = new ContextMenu();

            foreach (var Client in ClientList)
            {
                if (Client == null) continue;

                ListBoxItem listBoxItem = GetListBoxItem(Client);

                if (listBoxItem == null) continue;

                listBox.Items.Add(listBoxItem);
            }

            return listBox;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            WindowManager.Shared.ShowEditClientGroupWindow();
        }

        private void Expander_MenuItem_AddClient_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (menuItem == null) return;
            if (menuItem.Tag == null) return;

            ClientGroup ClientGroup = (ClientGroup)menuItem.Tag;

            WindowManager.Shared.ShowEditClientWindow(ClientGroup.ClientGroupID);
        }

        private void Expander_MenuItem_Edit_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (menuItem == null) return;
            if (menuItem.Tag == null) return;

            ClientGroup EditClientGroup = (ClientGroup)menuItem.Tag;

            WindowManager.Shared.ShowEditClientGroupWindow(EditClientGroup);
        }

        private void Expander_MenuItem_Remove_Click(object sender, RoutedEventArgs e)
        {
            if(System.Windows.MessageBox.Show("삭제될 경우 해당 그룹의 장치들도 같이 삭제됩니다\n삭제하시겠습니까?", "그룹삭제", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                MenuItem menuItem = sender as MenuItem;

                if (menuItem == null) return;
                if (menuItem.Tag == null) return;

                ClientGroup RemoveClientGroup = (ClientGroup)menuItem.Tag;

                RemoveGroupList(RemoveClientGroup);
            }
        }

        private void Client_MenuItem_Edit_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (menuItem == null) return;
            if (menuItem.Tag == null) return;

            Client EditClient = (Client)menuItem.Tag;

            WindowManager.Shared.ShowEditClientWindow(EditClient.ClientGroupID, EditClient);
        }

        private void Client_MenuItem_Remove_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (menuItem == null) return;
            if (menuItem.Tag == null) return;

            Client RemoveClient = (Client)menuItem.Tag;

            if (DatabaseManager.Shared.DELETE_TB_Client(RemoveClient))
                RemoveClientList(RemoveClient);
            else
                System.Windows.MessageBox.Show("삭제에 실패했습니다.", "장치삭제", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// 그룹삭제
        /// </summary>
        private void RemoveGroupList(ClientGroup RemoveClientGroup)
        {
            Expander RemoveExpander = null;

            foreach (UIElement item in spClientGroupList.Children)
            {
                Expander expander = item as Expander;

                if (expander == null) continue;
                if (expander.Tag == null) continue;

                ClientGroup ClientGroup = (ClientGroup)expander.Tag;

                if (ClientGroup == null || ClientGroup is ClientGroup == false) continue;
                if (RemoveClientGroup.ClientGroupID != ClientGroup.ClientGroupID) continue;

                RemoveExpander = expander;
            }

            if (DatabaseManager.Shared.DELETE_TB_ClientGroup((ClientGroup)RemoveExpander.Tag) && DatabaseManager.Shared.ALL_DELETE_TB_Client((ClientGroup)RemoveExpander.Tag))
            {
                ListBox listbox = (ListBox)RemoveExpander.Content;

                if (listbox != null && listbox.Items.Count > 0) listbox.Items.Clear();

                spClientGroupList.Children.Remove(RemoveExpander);
            }
            else
                System.Windows.MessageBox.Show("삭제에 실패했습니다.", "그룹삭제", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void RemoveClientList(Client RemoveClient)
        {
            ListBox TargetClientList = null;
            ListBoxItem TargetClient = null;

            foreach (UIElement item in spClientGroupList.Children)
            {
                Expander expander = item as Expander;

                if (expander == null) continue;
                if (expander.Tag == null) continue;

                ClientGroup ClientGroup = (ClientGroup)expander.Tag;

                if (ClientGroup == null || ClientGroup is ClientGroup == false) continue;
                if (ClientGroup.ClientGroupID != RemoveClient.ClientGroupID) continue;

                TargetClientList = (ListBox)expander.Content;
            }

            if (TargetClientList == null)
            {
                System.Windows.MessageBox.Show("삭제에 실패했습니다.", "장치삭제", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (ListBoxItem item in TargetClientList.Items)
            {
                if (item.Tag == null) continue;
                if (item.Tag is Client == false) continue;

                Client client = (Client)item.Tag;

                if (client == null) continue;

                if (client.ClientID != RemoveClient.ClientID) continue;

                TargetClient = item;
            }

            if (TargetClient == null)
            {
                System.Windows.MessageBox.Show("삭제에 실패했습니다.", "장치삭제", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TargetClientList.Items.Remove(TargetClient);
        }
    }
}
