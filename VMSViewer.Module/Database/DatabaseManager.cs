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

        #region TB_DeviceGroup(SELECT/INSERT/UPDATE/DELETE/COUNT/중복체크)
        public List<DeviceGroup> SELECT_TB_DeviceGroup()
        {
            string query = "SELECT * FROM TB_DEVICEGROUP";
            List<DeviceGroup> DeviceGroupList = new List<DeviceGroup>();

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
                                DeviceGroup item = new DeviceGroup();

                                item.DeviceGroupID = Convert.ToInt32(rdr["DEVICE_GROUP_ID"]);
                                item.DeviceGroupName = Convert.ToString(rdr["DEVICE_GROUP_NAME"]);

                                DeviceGroupList.Add(item);
                            }
                        }
                    }
                }

                return DeviceGroupList;
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return null;
            }
        }

        public bool INSERT_TB_DeviceGroup(DeviceGroup DeviceGroup)
        {
            string query = "INSERT INTO TB_DEVICEGROUP(DEVICE_GROUP_NAME) VALUES(@p1)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@p1", DeviceGroup.DeviceGroupName);
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

        public bool UPDATE_TB_DeviceGroup(DeviceGroup DeviceGroup)
        {
            string query = $"UPDATE TB_DEVICEGROUP SET DEVICE_GROUP_NAME = @p1 WHERE DEVICE_GROUP_ID = {DeviceGroup.DeviceGroupID}";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@p1", DeviceGroup.DeviceGroupName);
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

        public bool DELETE_TB_DeviceGroup(DeviceGroup DeviceGroup)
        {
            string query = $"DELETE FROM TB_DEVICEGROUP WHERE DEVICE_GROUP_ID = {DeviceGroup.DeviceGroupID}";

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

        public bool IsOverCountDeviceGroup()
        {
            bool IsOver = false;
            string query = $"SELECT COUNT(*) FROM TB_DEVICEGROUP";

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

        public bool IsUseDeviceGroupName(bool IsEdit, DeviceGroup DeviceGroup)
        {
            string query = "";

            if (IsEdit)
                query = $"SELECT COUNT(*) FROM TB_DEVICEGROUP WHERE DEVICE_GROUP_NAME = '{DeviceGroup.DeviceGroupName}' AND DEVICE_GROUP_ID <> {DeviceGroup.DeviceGroupID}";
            else
                query = $"SELECT COUNT(*) FROM TB_DEVICEGROUP WHERE DEVICE_GROUP_NAME = '{DeviceGroup.DeviceGroupName}'";

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

        #region TB_Device(SELECT/INSERT/UPDATE/DELETE/DELETE_ALL/COUNT/중복체크)
        public List<Device> SELECT_TB_Device(DeviceGroup DeviceGroup)
        {
            string query = $"SELECT * FROM TB_DEVICE WHERE DEVICE_GROUP_ID = {DeviceGroup.DeviceGroupID}";
            List<Device> DeviceList = new List<Device>();

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
                                Device item = new Device();

                                item.DeviceID = Convert.ToInt32(rdr["DEVICE_ID"]);
                                item.DeviceGroupID = Convert.ToInt32(rdr["DEVICE_GROUP_ID"]);
                                item.DeviceName = Convert.ToString(rdr["DEVICE_NAME"]);
                                item.DeviceIP = Convert.ToString(rdr["DEVICE_IP"]);
                                item.RTSPAddress = Convert.ToString(rdr["DEVICE_RTSP_ADDRESS"]);

                                DeviceList.Add(item);
                            }
                        }
                    }
                }

                return DeviceList;
            }
            catch (Exception ee)
            {
                LogManager.Shared.AddLog($"{ee.StackTrace}\r\n{ee.Message}");
                return null;
            }
        }

        public bool INSERT_TB_Device(Device Device)
        {
            string query = "INSERT INTO TB_DEVICE(DEVICE_GROUP_ID, DEVICE_NAME, DEVICE_IP, DEVICE_RTSP_ADDRESS) VALUES(@p1, @p2, @p3, @p4)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@p1", Device.DeviceGroupID);
                        cmd.Parameters.AddWithValue("@p2", Device.DeviceName);
                        cmd.Parameters.AddWithValue("@p3", Device.DeviceIP);
                        cmd.Parameters.AddWithValue("@p4", Device.RTSPAddress);
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

        public bool UPDATE_TB_Device(Device Device)
        {
            string query = $"UPDATE TB_DEVICE SET DEVICE_NAME = @p1, DEVICE_IP = @p2, DEVICE_RTSP_ADDRESS = @p3 WHERE DEVICE_ID = {Device.DeviceID}";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatebaseInformation.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.AddWithValue("@p1", Device.DeviceName);
                        cmd.Parameters.AddWithValue("@p2", Device.DeviceIP);
                        cmd.Parameters.AddWithValue("@p3", Device.RTSPAddress);
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

        public bool DELETE_TB_Device(Device Device)
        {
            string query = $"DELETE FROM TB_DEVICE WHERE DEVICE_ID = {Device.DeviceID}";

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

        public bool ALL_DELETE_TB_Device(DeviceGroup DeviceGroup)
        {
            string query = $"DELETE FROM TB_DEVICE WHERE DEVICE_GROUP_ID = {DeviceGroup.DeviceGroupID}";

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

        public bool IsOverCountDevice(int DeviceGroupID)
        {
            bool IsOver = false;
            string query = $"SELECT COUNT(*) FROM TB_DEVICE WHERE DEVICE_GROUP_ID = {DeviceGroupID}";

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

        public bool IsUseDeviceName(bool IsEdit, Device Device)
        {
            string query = "";

            if(IsEdit)
                query = $"SELECT COUNT(*) FROM TB_DEVICE WHERE DEVICE_NAME = '{Device.DeviceName}' AND DEVICE_GROUP_ID = {Device.DeviceGroupID} AND DEVICE_ID <> {Device.DeviceID}";
            else
                query = $"SELECT COUNT(*) FROM TB_DEVICE WHERE DEVICE_NAME = '{Device.DeviceName}' AND DEVICE_GROUP_ID = {Device.DeviceGroupID}";

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
