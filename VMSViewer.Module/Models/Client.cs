using System;

namespace VMSViewer
{
    public class Client
    {
        /// <summary>
        /// 장치 ID
        /// </summary>
        public int ClientID { get; set; }

        /// <summary>
        /// 장치그룹 ID
        /// </summary>
        public int ClientGroupID { get; set; }

        /// <summary>
        /// 장치명
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 장치 IP
        /// </summary>
        public string ClientIP { get; set; }

        /// <summary>
        /// 장치 RTSP 주소
        /// </summary>
        public string RTSPAddress { get; set; }

        public Client() { }

        public Client(int ClientGroupID)
        {
            this.ClientGroupID = ClientGroupID;
        }
    }
}
