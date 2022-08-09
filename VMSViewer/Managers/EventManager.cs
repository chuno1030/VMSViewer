using System;

namespace VMSViewer
{
    public class EventManager
    {
        /// <summary>
        /// 그룹등록 이벤트
        /// </summary>
        public delegate void AddDeviceGroupDelegate(DeviceGroup NewDeviceGroup);
        public static event AddDeviceGroupDelegate onAddDeviceGroup;
        public static void AddDeviceGroupEvent(DeviceGroup NewDeviceGroup)
        {
            if (onAddDeviceGroup != null) onAddDeviceGroup(NewDeviceGroup);
        }

        /// <summary>
        /// 그룹수정 이벤트
        /// </summary>
        public delegate void RefreshDeviceGroupDelegate(DeviceGroup RefreshDeviceGroup = null);
        public static event RefreshDeviceGroupDelegate onRefreshDeviceGroup;
        public static void RefreshDeviceGroupEvent(DeviceGroup RefreshDeviceGroup = null)
        {
            if (onRefreshDeviceGroup != null) onRefreshDeviceGroup(RefreshDeviceGroup);
        }

        /// <summary>
        /// 장치등록 이벤트
        /// </summary>
        public delegate void AddDeviceDelegate(Device NewDevice);
        public static event AddDeviceDelegate onAddDevice;
        public static void AddDeviceEvent(Device NewDevice)
        {
            if (onAddDevice != null) onAddDevice(NewDevice);
        }

        /// <summary>
        /// 장치수정 이벤트
        /// </summary>
        public delegate void RefreshDeviceDelegate(Device RefreshDevice = null);
        public static event RefreshDeviceDelegate onRefreshDevice;
        public static void RefreshDeviceEvent(Device RefreshDevice = null)
        {
            if (onRefreshDevice != null) onRefreshDevice(RefreshDevice);
        }
    }
}
