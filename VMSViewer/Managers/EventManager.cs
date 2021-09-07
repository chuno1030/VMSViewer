using System;

namespace VMSViewer
{
    public class EventManager
    {
        /// <summary>
        /// 그룹 등록 시
        /// </summary>
        public delegate void AddClientGroupDelegate(ClientGroup NewClientGroup);
        public static event AddClientGroupDelegate onAddClientGroup;
        public static void AddClientGroupEvent(ClientGroup NewClientGroup)
        {
            if (onAddClientGroup != null) onAddClientGroup(NewClientGroup);
        }

        /// <summary>
        /// 그룹 수정 시
        /// </summary>
        public delegate void RefreshClientGroupDelegate(ClientGroup RefreshClientGroup = null);
        public static event RefreshClientGroupDelegate onRefreshClientGroup;
        public static void RefreshClientGroupEvent(ClientGroup RefreshClientGroup = null)
        {
            if (onRefreshClientGroup != null) onRefreshClientGroup(RefreshClientGroup);
        }

        /// <summary>
        /// 장치 등록 시
        /// </summary>
        public delegate void AddClientDelegate(Client NewClient);
        public static event AddClientDelegate onAddClient;
        public static void AddClientEvent(Client NewClient)
        {
            if (onAddClient != null) onAddClient(NewClient);
        }

        /// <summary>
        /// 장치 수정 시
        /// </summary>
        public delegate void RefreshClientDelegate(Client RefreshClient = null);
        public static event RefreshClientDelegate onRefreshClient;
        public static void RefreshClientEvent(Client RefreshClient = null)
        {
            if (onRefreshClient != null) onRefreshClient(RefreshClient);
        }
    }
}
