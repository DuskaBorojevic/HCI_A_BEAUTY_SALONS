using HCI_A.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Dao
{
    public class PhoneDao
    {
        public static bool DeletePhoneNumber(int beautySalonId, string phoneNumber)
        {
            bool result = false;
            MySqlConnection connection = null;
            MySqlCommand command = null;
            try
            {
                connection = DBUtil.GetConnection();
                command = new MySqlCommand("obrisi_broj_telefona", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pId", beautySalonId);
                command.Parameters.AddWithValue("@pBrojTelefona", phoneNumber);
                connection.Open();
                result = command.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
            }
            finally
            {
                DBUtil.Close(command, connection);
            }
            return result;
        }

        public static bool UpdatePhone(int beautySalonId, string phoneNumber, string newPhoneNumber)
        {
            bool result = false;
            MySqlConnection connection = null;
            MySqlCommand command = null;
            try
            {
                connection = DBUtil.GetConnection();
                command = new MySqlCommand("azuriraj_telefon", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pId", beautySalonId);
                command.Parameters.AddWithValue("@pBrojTelefona", phoneNumber);
                command.Parameters.AddWithValue("@pNoviBrojTelefona", newPhoneNumber);
                connection.Open();
                result = command.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
            }
            finally
            {
                DBUtil.Close(command, connection);
            }
            return result;
        }

        public static List<Phone> GetPhoneNumbersBySalonId(int salonId)
        {
            List<Phone> phoneNumbers = new List<Phone>();

            try
            {
                using (MySqlConnection connection = DBUtil.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT * FROM telefon WHERE KOZMETICKI_SALON_IdKozmetickogSalona = @salonId";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@salonId", salonId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Phone phone = new Phone
                                {
                                    PhoneNumber = reader.GetString("BrojTelefona"),
                                    BeautySalonId = reader.GetInt32("KOZMETICKI_SALON_IdKozmetickogSalona")
                                };

                                phoneNumbers.Add(phone);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting phone numbers: {ex.Message}");
            }

            return phoneNumbers;
        }

        public static bool AddPhone(int beautySalonId, string phoneNumber)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("dodaj_telefon", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pBrojTelefona", phoneNumber);
                command.Parameters.AddWithValue("@pId", beautySalonId);
                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }
    }
}
