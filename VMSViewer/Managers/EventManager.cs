using System;

namespace VMSViewer
{
    public class EventManager
    {
        public delegate void RefreshClientGroupDelegate(ClientGroup RefreshClientGroup = null);
        public static event RefreshClientGroupDelegate onRefreshClientGroup;
        public static void RefreshClientGroupEvent(ClientGroup RefreshClientGroup = null)
        {
            if (onRefreshClientGroup != null) onRefreshClientGroup(RefreshClientGroup);
        }

        public delegate void RefreshClientDelegate(Client RefreshClient = null);
        public static event RefreshClientDelegate onRefreshClient;
        public static void RefreshClientEvent(Client RefreshClient = null)
        {
            if (onRefreshClient != null) onRefreshClient(RefreshClient);
        }
    }
}
