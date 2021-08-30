using System;

namespace VMSViewer
{
    public class EventManager
    {
        public delegate void RefreshClientGroupDelegate(int RefreshID = -1);
        public static event RefreshClientGroupDelegate onRefreshClientGroup;
        public static void RefreshClientGroupEvent(int RefreshID = -1)
        {
            if (onRefreshClientGroup != null) onRefreshClientGroup(RefreshID);
        }
    }
}
