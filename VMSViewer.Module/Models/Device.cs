using System;

namespace VMSViewer
{
    public class Device
    {
        /// <summary>
        /// 장치 ID
        /// </summary>
        public int DeviceID { get; set; }

        /// <summary>
        /// 장치그룹 ID
        /// </summary>
        public int DeviceGroupID { get; set; }

        /// <summary>
        /// 장치명
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 장치 IP
        /// </summary>
        public string DeviceIP { get; set; }

        /// <summary>
        /// 장치 RTSP 주소
        /// </summary>
        public string RTSPAddress { get; set; }

        public Device() { }

        public Device(int DeviceGroupID)
        {
            this.DeviceGroupID = DeviceGroupID;
        }
    }
}
