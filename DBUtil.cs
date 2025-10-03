using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace HCI_A
{

    public static class DBUtil
    {
        private static string connectionString = "Server=localhost;Database=kozmeticki_salon_hci;Uid=root;Pwd=bduska3709;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public static void Close(MySqlCommand command, MySqlConnection connection)
        {
            if (command != null)
            {
                command.Dispose();
            }
            if (connection != null)
            {
                connection.Close();
            }
        }
    }
}
