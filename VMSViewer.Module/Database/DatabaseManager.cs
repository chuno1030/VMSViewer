using System;
using System.Linq;
using System.Collections.Generic;

using MySqlConnector;

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

        #region TB_ClientGroup(SELECT/INSERT/UPDATE/DELETE/COUNT/중복체크)
        public List<ClientGroup> SELECT_TB_ClientGroup()
        {
            string query = "SELECT * FROM TB_ClientGroup";
            List<ClientGroup> ClientGroupList = new List<ClientGroup>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                ClientGroup item = new ClientGroup();

                                item.ClientGroupID = Convert.ToInt32(rdr["GROUP_ID"]);
                                item.ClientGroupName = Convert.ToString(rdr["GROUP_NAME"]);

                                ClientGroupList.Add(item);
                            }
                        }
                    }
                }

                return ClientGroupList;
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return null;
            }
        }

        public bool INSERT_TB_ClientGroup(ClientGroup ClientGroup)
        {
            string query = "INSERT INTO TB_CLIENTGROUP(GROUP_NAME) VALUES(@p1)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@p1", ClientGroup.ClientGroupName);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return false;
            }
        }

        public bool UPDATE_TB_ClientGroup(ClientGroup ClientGroup)
        {
            string query = $"UPDATE TB_CLIENTGROUP SET GROUP_NAME = @p1 WHERE GROUP_ID = {ClientGroup.ClientGroupID}";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@p1", ClientGroup.ClientGroupName);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return false;
            }
        }

        public bool DELETE_TB_ClientGroup(ClientGroup ClientGroup)
        {
            string query = $"DELETE FROM TB_CLIENTGROUP WHERE GROUP_ID = {ClientGroup.ClientGroupID}";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return false;
            }
        }

        public bool IsOverCountClientGroup()
        {
            bool IsOver = false;
            string query = $"SELECT COUNT(*) FROM TB_ClientGroup";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        int Count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (Count >= 50)
                            IsOver = true;
                        else
                            IsOver = false;

                        return IsOver;
                    }
                }
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return false;
            }
        }

        public bool IsUseClientGroupName(bool IsEdit, ClientGroup ClientGroup)
        {
            string query = "";

            if (IsEdit)
                query = $"SELECT COUNT(*) FROM TB_ClientGroup WHERE GROUP_NAME = '{ClientGroup.ClientGroupName}' AND GROUP_ID <> {ClientGroup.ClientGroupID}";
            else
                query = $"SELECT COUNT(*) FROM TB_ClientGroup WHERE GROUP_NAME = '{ClientGroup.ClientGroupName}'";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        var Result = cmd.ExecuteScalar();

                        return Convert.ToBoolean(Result);
                    }
                }
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return false;
            }
        }
        #endregion

        #region TB_Client(SELECT/INSERT/UPDATE/DELETE/DELETE_ALL/COUNT/중복체크)
        public List<Client> SELECT_TB_Client(ClientGroup ClientGroup)
        {
            string query = $"SELECT * FROM TB_Client WHERE GROUP_ID = {ClientGroup.ClientGroupID}";
            List<Client> ClientList = new List<Client>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                Client item = new Client();

                                item.ClientID = Convert.ToInt32(rdr["CLIENT_ID"]);
                                item.ClientGroupID = Convert.ToInt32(rdr["GROUP_ID"]);
                                item.ClientName = Convert.ToString(rdr["CLIENT_NAME"]);
                                item.ClientIP = Convert.ToString(rdr["CLIENT_IP"]);
                                item.RTSPAddress = Convert.ToString(rdr["RTSP_ADDRESS"]);

                                ClientList.Add(item);
                            }
                        }
                    }
                }

                return ClientList;
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return null;
            }
        }

        public bool INSERT_TB_Client(Client Client)
        {
            string query = "INSERT INTO TB_CLIENT(GROUP_ID, CLIENT_NAME, CLIENT_IP, RTSP_ADDRESS) VALUES(@p1, @p2, @p3, @p4)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@p1", Client.ClientGroupID);
                        cmd.Parameters.AddWithValue("@p2", Client.ClientName);
                        cmd.Parameters.AddWithValue("@p3", Client.ClientIP);
                        cmd.Parameters.AddWithValue("@p4", Client.RTSPAddress);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return false;
            }
        }

        public bool UPDATE_TB_Client(Client Client)
        {
            string query = $"UPDATE TB_CLIENT SET CLIENT_NAME = @p1, CLIENT_IP = @p2, RTSP_ADDRESS = @p3 WHERE CLIENT_ID = {Client.ClientID}";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@p1", Client.ClientName);
                        cmd.Parameters.AddWithValue("@p2", Client.ClientIP);
                        cmd.Parameters.AddWithValue("@p3", Client.RTSPAddress);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return false;
            }
        }

        public bool DELETE_TB_Client(Client Client)
        {
            string query = $"DELETE FROM TB_CLIENT WHERE CLIENT_ID = {Client.ClientID}";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return false;
            }
        }

        public bool ALL_DELETE_TB_Client(ClientGroup ClientGroup)
        {
            string query = $"DELETE FROM TB_CLIENT WHERE GROUP_ID = {ClientGroup.ClientGroupID}";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return false;
            }
        }

        public bool IsOverCountClient(int ClientGroupID)
        {
            bool IsOver = false;
            string query = $"SELECT COUNT(*) FROM TB_Client WHERE GROUP_ID = {ClientGroupID}";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        int Count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (Count >= 50)
                            IsOver = true;
                        else
                            IsOver = false;

                        return IsOver;
                    }
                }
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return false;
            }
        }

        public bool IsUseClientName(bool IsEdit, Client Client)
        {
            string query = "";

            if(IsEdit)
                query = $"SELECT COUNT(*) FROM TB_CLIENT WHERE CLIENT_NAME = '{Client.ClientName}' AND GROUP_ID = {Client.ClientGroupID} AND CLIENT_ID <> {Client.ClientID}";
            else
                query = $"SELECT COUNT(*) FROM TB_CLIENT WHERE CLIENT_NAME = '{Client.ClientName}' AND GROUP_ID = {Client.ClientGroupID}";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        var Result = cmd.ExecuteScalar();

                        return Convert.ToBoolean(Result);
                    }
                }
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return false;
            }
        }
        #endregion
    }
}
