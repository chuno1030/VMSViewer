using System;
using System.Linq;
using System.Collections.Generic;

namespace VMSViewer.Module
{
    public class DatabaseManager
    {
        private static DatabaseManager _shared;

        public static DatabaseManager Shared
        {
            get
            {
                if (_shared == null)
                    _shared = new DatabaseManager();

                return _shared;
            }
        }

        public bool IsAccount(string LoginID, string LoginPassword)
        {
            return true;

            ///추후 작업

            string query = $"SELECT COUNT(*) FROM TB_USER WHERE ID = {LoginID} AND PASSWORD = {LoginPassword}";

            try
            {
                
            }
            catch (Exception ee)
            {

                return false;
            }
        }
    }
}
