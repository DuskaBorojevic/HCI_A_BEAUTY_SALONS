using HCI_A.Models;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace HCI_A.Dao
{
    public class ClientDao
    {
        public static bool RegisterClient(string username, string password, string firstName, string lastName, string email, string type)
        {
            bool result = false;

            using (MySqlConnection connection = DBUtil.GetConnection())
            {
                try
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();

                    using (MySqlCommand command = new MySqlCommand("dodaj_klijenta", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@pKorisnickoIme", username);
                        command.Parameters.AddWithValue("@pLozinka", password);
                        command.Parameters.AddWithValue("@pIme", firstName);
                        command.Parameters.AddWithValue("@pPrezime", lastName);
                        command.Parameters.AddWithValue("@pEmailAdresa", email);
                        command.Parameters.AddWithValue("@pTipNaloga", type);

                        command.ExecuteNonQuery();
                        result = true;
                    }
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine("MySqlException in Register: " + ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("General exception in Register: " + ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }
            }

            return result;
        }


    }
}
