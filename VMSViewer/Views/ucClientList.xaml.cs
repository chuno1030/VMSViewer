using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using VMSViewer.Module;

namespace VMSViewer
{
    /// <summary>
    /// DeviceList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ucDeviceList : UserControl
    {
        /// <summary>
        /// 장치리스트가 OPEN 시 TRUE
        /// </summary>
        public bool IsOpenDeviceList = false;

        /// <summary>
        /// 드래그할 컨트롤을 선택했을때
        /// </summary>
        private bool IsDrag = true;

        public ucDeviceList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitProc();

            EventManager.onRefreshDevice += EventManager_onRefreshDevice;
            EventManager.onRefreshDeviceGroup += EventManager_onRefreshDeviceGroup;
            EventManager.onAddDevice += EventManager_onAddDevice;
            EventManager.onAddDeviceGroup += EventManager_onAddDeviceGroup;
        }

        private void InitProc()
        {
            RefreshDeviceGroupList();
        }

        private void EventManager_onAddDevice(Device NewDevice)
        {
            Expander TargetDeviceList = null;
            DeviceGroup TargetDeviceGroup = null;

            foreach (UIElement item in spDeviceGroupList.Children)
            {
                Expander expander = item as Expander;

                if (expander == null) continue;
                if (expander.Tag == null) continue;

                DeviceGroup DeviceGroup = (DeviceGroup)expander.Tag;

                if (DeviceGroup == null || DeviceGroup is DeviceGroup == false) continue;
                if (DeviceGroup.DeviceGroupID != NewDevice.DeviceGroupID) continue;

                TargetDeviceGroup = DeviceGroup;
                TargetDeviceList = expander;
            }

            if (TargetDeviceList == null || TargetDeviceGroup == null)
            {
                System.Windows.MessageBox.Show("생성에 실패했습니다.", "장치생성", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ListBox listBox = (ListBox)TargetDeviceList.Content;

            if (listBox.Items.Count > 0) listBox.Items.Clear();

            TargetDeviceList.Content = RefreshDeviceList(DatabaseManager.Shared.SELECT_TB_Device(TargetDeviceGroup));
        }

        private void EventManager_onAddDeviceGroup(DeviceGroup NewDeviceGroup)
        {
            RefreshDeviceGroupList();

            //Expander expander = GetExpander(NewDeviceGroup);

            //if (expander != null)
            //    spDeviceGroupList.Children.Add(expander);
            //else
            //    System.Windows.MessageBox.Show("생성에 실패했습니다.", "그룹생성", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void EventManager_onRefreshDevice(Device RefreshDevice = null)
        {
            ListBox TargetDeviceList = null;
            ListBoxItem TargetDevice = null;

            foreach (UIElement item in spDeviceGroupList.Children)
            {
                Expander expander = item as Expander;

                if (expander == null) continue;
                if (expander.Tag == null) continue;

                DeviceGroup DeviceGroup = (DeviceGroup)expander.Tag;

                if (DeviceGroup == null || DeviceGroup is DeviceGroup == false) continue;
                if (DeviceGroup.DeviceGroupID != RefreshDevice.DeviceGroupID) continue;

                TargetDeviceList = (ListBox)expander.Content;
            }

            if (TargetDeviceList == null)
            {
                System.Windows.MessageBox.Show("편집에 실패했습니다.", "장치편집", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (ListBoxItem item in TargetDeviceList.Items)
            {
                if (item.Tag == null) continue;
                if (item.Tag is Device == false) continue;

                Device Device = (Device)item.Tag;

                if (Device == null) continue;

                if (Device.DeviceID != RefreshDevice.DeviceID) continue;

                TargetDevice = item;
            }

            if (TargetDevice == null)
            {
                System.Windows.MessageBox.Show("편집에 실패했습니다.", "장치편집", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TargetDevice.Tag = RefreshDevice;
            TargetDevice.Content = RefreshDevice.DeviceName;
        }

        private void EventManager_onRefreshDeviceGroup(DeviceGroup RefreshDeviceGroup)
        {
            Expander RefreshExpander = null;

            foreach (UIElement item in spDeviceGroupList.Children)
            {
                Expander expander = item as Expander;

                if (expander == null) continue;
                if (expander.Tag == null) continue;
                if (RefreshDeviceGroup == null) continue;

                DeviceGroup DeviceGroup = (DeviceGroup)expander.Tag;

                if (DeviceGroup == null || DeviceGroup is DeviceGroup == false) continue;
                if (RefreshDeviceGroup.DeviceGroupID != DeviceGroup.DeviceGroupID) continue;

                RefreshExpander = expander;
            }

            RefreshExpander.Tag = RefreshDeviceGroup;
            RefreshExpander.Header = RefreshDeviceGroup.DeviceGroupName.Trim();
        }

        private Expander GetExpander(DeviceGroup DeviceGroup)
        {
            if (DeviceGroup == null) return null;

            var expander = new Expander();

            expander.Tag = DeviceGroup;
            expander.Header = DeviceGroup.DeviceGroupName;
            expander.ContextMenu = new ContextMenu();

            var menuItem1 = new MenuItem();
            menuItem1.Tag = DeviceGroup;
            menuItem1.Header = "장치생성";
            menuItem1.Click += Expander_MenuItem_AddDevice_Click;

            var menuItem2 = new MenuItem();
            menuItem2.Tag = DeviceGroup;
            menuItem2.Header = "그룹편집";
            menuItem2.Click += Expander_MenuItem_Edit_Click;

            var menuItem3 = new MenuItem();
            menuItem3.Tag = DeviceGroup;
            menuItem3.Header = "그룹삭제";
            menuItem3.Click += Expander_MenuItem_Remove_Click;

            expander.ContextMenu.Items.Add(menuItem1);
            expander.ContextMenu.Items.Add(new Separator());
            expander.ContextMenu.Items.Add(menuItem2);
            expander.ContextMenu.Items.Add(menuItem3);

            return expander;
        }

        private ListBoxItem GetListBoxItem(Device Device)
        {
            if (Device == null) return null;

            var listBoxItem = new ListBoxItem() { Content = Device.DeviceName, Tag = Device };
            listBoxItem.ContextMenu = new ContextMenu();

            var menuItem1 = new MenuItem() { Header = "장치편집", Tag = Device };
            menuItem1.Click += Device_MenuItem_Edit_Click;

            var menuItem2 = new MenuItem() { Header = "장치삭제", Tag = Device };
            menuItem2.Click += Device_MenuItem_Remove_Click;

            listBoxItem.ContextMenu.Items.Add(menuItem1);
            listBoxItem.ContextMenu.Items.Add(menuItem2);

            listBoxItem.PreviewMouseLeftButtonDown += ListBoxItem_PreviewMouseLeftButtonDown;
            listBoxItem.PreviewMouseMove += ListBoxItem_PreviewMouseMove;

            return listBoxItem;
        }

        private void ListBoxItem_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed && IsDrag)
            {
                var listboxItem = sender as ListBoxItem;

                if (listboxItem == null) return;
                if (listboxItem.Tag == null) return;
                if (listboxItem.Tag is Device == false) return;

                var Device = (Device)listboxItem.Tag;

                var dataObject = new DataObject("Device", Device);
                DragDrop.DoDragDrop(listboxItem, dataObject, DragDropEffects.Move);
            }
        }

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsDrag = true;
        }

        private void ClearList()
        {
            foreach (UIElement item in spDeviceGroupList.Children)
            {
                Expander expander = item as Expander;

                if (expander == null) continue;
                if (expander.Tag == null) continue;

                ListBox listBox = (ListBox)expander.Content;

                if (listBox == null || listBox.Items.Count < 1) continue;

                listBox.Items.Clear();
            }

            spDeviceGroupList.Children.Clear();
        }

        /// <summary>
        /// 그룹리스트 불러오기
        /// </summary>
        private void RefreshDeviceGroupList()
        {
            ClearList();

            List<DeviceGroup> DeviceGroupList = DatabaseManager.Shared.SELECT_TB_DeviceGroup();

            //DeviceGroup DeviceGroup1 = new DeviceGroup();
            //DeviceGroup1.DeviceGroupID = 1;
            //DeviceGroup1.DeviceGroupName = "MariaDB";

            //List<DeviceGroup> DeviceGroupList = new List<DeviceGroup>();

            //DeviceGroupList.Add(DeviceGroup1);

            if (DeviceGroupList == null || DeviceGroupList.Count < 0) return;

            foreach (var DeviceGroup in DeviceGroupList)
            {
                if (DeviceGroup == null) continue;

                Expander expander = GetExpander(DeviceGroup);

                if (expander == null) continue;

                expander.Content = RefreshDeviceList(DatabaseManager.Shared.SELECT_TB_Device(DeviceGroup));
                spDeviceGroupList.Children.Add(expander);
            }
        }

        private ListBox RefreshDeviceList(List<Device> DeviceList)
        {
            ListBox listBox = new ListBox();
            listBox.AllowDrop = true;
            listBox.ContextMenu = new ContextMenu();

            foreach (var Device in DeviceList)
            {
                if (Device == null) continue;

                ListBoxItem listBoxItem = GetListBoxItem(Device);

                if (listBoxItem == null) continue;

                listBox.Items.Add(listBoxItem);
            }

            return listBox;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            WindowManager.Shared.ShowEditDeviceGroupWindow();
        }

        private void Expander_MenuItem_AddDevice_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (menuItem == null) return;
            if (menuItem.Tag == null) return;

            DeviceGroup DeviceGroup = (DeviceGroup)menuItem.Tag;

            WindowManager.Shared.ShowEditDeviceWindow(DeviceGroup.DeviceGroupID);
        }

        private void Expander_MenuItem_Edit_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (menuItem == null) return;
            if (menuItem.Tag == null) return;

            DeviceGroup EditDeviceGroup = (DeviceGroup)menuItem.Tag;

            WindowManager.Shared.ShowEditDeviceGroupWindow(EditDeviceGroup);
        }

        private void Expander_MenuItem_Remove_Click(object sender, RoutedEventArgs e)
        {
            if(System.Windows.MessageBox.Show("삭제될 경우 해당 그룹의 장치들도 같이 삭제됩니다\n삭제하시겠습니까?", "그룹삭제", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                MenuItem menuItem = sender as MenuItem;

                if (menuItem == null) return;
                if (menuItem.Tag == null) return;

                DeviceGroup RemoveDeviceGroup = (DeviceGroup)menuItem.Tag;

                RemoveGroupList(RemoveDeviceGroup);
            }
        }

        private void Device_MenuItem_Edit_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (menuItem == null) return;
            if (menuItem.Tag == null) return;

            Device EditDevice = (Device)menuItem.Tag;

            WindowManager.Shared.ShowEditDeviceWindow(EditDevice.DeviceGroupID, EditDevice);
        }

        private void Device_MenuItem_Remove_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (menuItem == null) return;
            if (menuItem.Tag == null) return;

            Device RemoveDevice = (Device)menuItem.Tag;

            if (DatabaseManager.Shared.DELETE_TB_Device(RemoveDevice))
                RemoveDeviceList(RemoveDevice);
            else
                System.Windows.MessageBox.Show("삭제에 실패했습니다.", "장치삭제", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// 그룹삭제
        /// </summary>
        private void RemoveGroupList(DeviceGroup RemoveDeviceGroup)
        {
            Expander RemoveExpander = null;

            foreach (UIElement item in spDeviceGroupList.Children)
            {
                Expander expander = item as Expander;

                if (expander == null) continue;
                if (expander.Tag == null) continue;

                DeviceGroup DeviceGroup = (DeviceGroup)expander.Tag;

                if (DeviceGroup == null || DeviceGroup is DeviceGroup == false) continue;
                if (RemoveDeviceGroup.DeviceGroupID != DeviceGroup.DeviceGroupID) continue;

                RemoveExpander = expander;
            }

            if (DatabaseManager.Shared.DELETE_TB_DeviceGroup((DeviceGroup)RemoveExpander.Tag) && DatabaseManager.Shared.ALL_DELETE_TB_Device((DeviceGroup)RemoveExpander.Tag))
            {
                ListBox listbox = (ListBox)RemoveExpander.Content;

                if (listbox != null && listbox.Items.Count > 0) listbox.Items.Clear();

                spDeviceGroupList.Children.Remove(RemoveExpander);
            }
            else
                System.Windows.MessageBox.Show("삭제에 실패했습니다.", "그룹삭제", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void RemoveDeviceList(Device RemoveDevice)
        {
            ListBox TargetDeviceList = null;
            ListBoxItem TargetDevice = null;

            foreach (UIElement item in spDeviceGroupList.Children)
            {
                Expander expander = item as Expander;

                if (expander == null) continue;
                if (expander.Tag == null) continue;

                DeviceGroup DeviceGroup = (DeviceGroup)expander.Tag;

                if (DeviceGroup == null || DeviceGroup is DeviceGroup == false) continue;
                if (DeviceGroup.DeviceGroupID != RemoveDevice.DeviceGroupID) continue;

                TargetDeviceList = (ListBox)expander.Content;
            }

            if (TargetDeviceList == null)
            {
                System.Windows.MessageBox.Show("삭제에 실패했습니다.", "장치삭제", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (ListBoxItem item in TargetDeviceList.Items)
            {
                if (item.Tag == null) continue;
                if (item.Tag is Device == false) continue;

                Device Device = (Device)item.Tag;

                if (Device == null) continue;

                if (Device.DeviceID != RemoveDevice.DeviceID) continue;

                TargetDevice = item;
            }

            if (TargetDevice == null)
            {
                System.Windows.MessageBox.Show("삭제에 실패했습니다.", "장치삭제", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TargetDeviceList.Items.Remove(TargetDevice);
        }
    }
}
