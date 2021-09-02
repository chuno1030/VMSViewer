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

        #region TB_ClientGroup(SELECT/INSERT/UPDATE/DELETE)
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
                                item.ClientGroupName = rdr["GROUP_NAME"].ToString();

                                ClientGroupList.Add(item);
                            }
                        }
                    }
                }

                return ClientGroupList;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.StackTrace);
                Console.WriteLine($"### {ee.Message} ###");
                return null;
            }
        }

        public bool INSERT_TB_ClientGroup(ClientGroup NewClientGroup)
        {
            string query = "INSERT INTO TB_CLIENTGROUP(GROUP_NAME) VALUES(@p1)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@p1", NewClientGroup.ClientGroupName);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

                return false;
            }
        }

        public bool UPDATE_TB_ClientGroup(ClientGroup UpdateClientGroup)
        {
            string query = $"UPDATE TB_CLIENTGROUP SET GROUP_NAME = @p1 WHERE GROUP_ID = {UpdateClientGroup.ClientGroupID}";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@p1", UpdateClientGroup.ClientGroupName);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

                return false;
            }
        }

        public bool DELETE_TB_ClientGroup(ClientGroup DeleteClientGroup)
        {
            string query = $"DELETE FROM TB_CLIENTGROUP WHERE GROUP_ID = {DeleteClientGroup.ClientGroupID}";

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
                Console.WriteLine(ee.Message);
                return false;
            }
        }
        #endregion

        #region TB_Client(SELECT/INSERT/UPDATE/DELETE)
        public List<ClientGroup> SELECT_TB_Client()
        {
            string query = "SELECT * FROM TB_Client";
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

                            }
                        }
                    }
                }

                return ClientGroupList;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.StackTrace);
                Console.WriteLine($"### {ee.Message} ###");
                return null;
            }
        }

        public bool INSERT_TB_Client(Client NewClient)
        {
            string query = "INSERT INTO TB_CLIENT(GROUP_ID, CLIENT_NAME, CLIENT_IP, RTSP_ADDRESS) VALUES(@p1, @p2, @p3, @p4)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@p1", NewClient.ClientGroupID);
                        cmd.Parameters.AddWithValue("@p2", NewClient.ClientName);
                        cmd.Parameters.AddWithValue("@p3", NewClient.ClientIP);
                        cmd.Parameters.AddWithValue("@p4", NewClient.RTSPAddress);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

                return false;
            }
        }

        //public bool IsUseClientName(Client NewClient)
        //{
        //    bool IsUseClientName = false;
        //    string query = $"SELECT COUNT(*) FROM TB_CLIENT WHERE ";

        //    try
        //    {
        //        using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
        //        {
        //            using (MySqlCommand cmd = new MySqlCommand(query, conn))
        //            {
        //                conn.Open();

        //                cmd.Parameters.AddWithValue("@p1", NewClient.ClientGroupID);
        //                cmd.Parameters.AddWithValue("@p2", NewClient.ClientName);
        //                cmd.Parameters.AddWithValue("@p3", NewClient.ClientIP);
        //                cmd.Parameters.AddWithValue("@p4", NewClient.RTSPAddress);
        //                cmd.ExecuteNonQuery();
        //            }
        //        }

        //        return IsUseClientName;
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);

        //        return false;
        //    }
        //}

        public bool UPDATE_TB_Client(Client UpdateClient)
        {
            string query = $"UPDATE TB_CLIENT SET GROUP_NAME = @p1 WHERE GROUP_ID = {UpdateClient.ClientGroupID}";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@p1", UpdateClient.ClientName);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

                return false;
            }
        }

        public bool DELETE_TB_Client(ClientGroup DeleteClient)
        {
            string query = $"DELETE FROM TB_CLIENT WHERE CLIENT_ID = {DeleteClient.ClientGroupID}";

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
                Console.WriteLine(ee.Message);
                return false;
            }
        }
        #endregion
    }
}
