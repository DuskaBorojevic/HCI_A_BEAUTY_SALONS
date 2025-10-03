using HCI_A.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HCI_A.Dao
{
    public class BeautySalonDao
    {
        public static List<List<string>> GetBeautySalonsPrimaryInfo()
        {
            List<List<string>> salons = new List<List<string>>();
            MySqlConnection connection = null;
            MySqlCommand command = null;
            MySqlDataReader reader = null;

            string query = "SELECT ks.IdKozmetickogSalona, ks.Naziv, m.Naziv as Mjesto FROM kozmeticki_salon ks INNER JOIN mjesto m ON ks.MJESTO_BrojPoste = m.BrojPoste";
            try
            {
                connection = DBUtil.GetConnection();
                command = new MySqlCommand(query, connection);

                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    List<string> salon = new List<string>
                {
                    reader.GetInt32("IdKozmetickogSalona").ToString(),
                    reader.GetString("Naziv"),
                    reader.GetString("Mjesto")
                };
                    salons.Add(salon);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error retrieving salons: {ex.Message}");
            }
            finally
            {
                DBUtil.Close(command, connection);
            }
            return salons;
        }



        public static BeautySalon GetBeautySalonById(int id)
        {
            BeautySalon beautySalon = null;
            MySqlConnection connection = null;
            MySqlCommand command = null;
            MySqlDataReader reader = null;

            try
            {
                connection = DBUtil.GetConnection();
                command = new MySqlCommand("dohvati_kozmeticki_salon", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pIdKS", id);

                connection.Open();
                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int beautySalonId = reader.GetInt32("IdKozmetickogSalona");
                    string name = reader.GetString("Naziv");
                    string address = reader.GetString("Adresa");
                    string phone = reader.GetString("Telefon");

                    string workTime;
                    try
                    {
                        workTime = reader.GetString("RadnoVrijeme");
                    }
                    catch
                    {
                        object rawValue = reader["RadnoVrijeme"];
                        if (rawValue == DBNull.Value)
                        {
                            workTime = "Not specified";
                        }
                        else if (rawValue is DateTime)
                        {
                            workTime = "Hours not properly formatted";
                        }
                        else
                        {
                            workTime = rawValue.ToString();
                        }
                    }

                    int locationId = reader.GetInt32("MJESTO_BrojPoste");
                    int priceListId = reader.GetInt32("CJENOVNIK_IdCjenovnika");


                    Location location = LocationDao.GetLocationById(locationId);
                    PriceList priceList = PriceListDao.GetPriceListById(priceListId);


                    List<Employee> employees = EmployeeDao.GetEmployeesBySalonId(beautySalonId);
                    beautySalon = new BeautySalon(beautySalonId, name, address, workTime, location, priceList, phone, employees);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error retrieving beauty salon: {ex.Message}");
                MessageBox.Show($"Error retrieving beauty salon: {ex.Message}");
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
                DBUtil.Close(command, connection);
            }
            return beautySalon;
        }

        public static List<BeautySalon> GetBeautySalonsForClient()
        {
            List<BeautySalon> salons = new List<BeautySalon>();
            MySqlConnection connection = null;
            MySqlCommand command = null;
            MySqlDataReader reader = null;

            string query = @"SELECT ks.IdKozmetickogSalona, ks.Naziv, ks.Adresa, m.Naziv as Mjesto 
                            FROM kozmeticki_salon ks 
                            INNER JOIN mjesto m ON ks.MJESTO_BrojPoste = m.BrojPoste";
            try
            {
                connection = DBUtil.GetConnection();
                command = new MySqlCommand(query, connection);

                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BeautySalon salon = new BeautySalon();
                    salon.BeautySalonId = reader.GetInt32("IdKozmetickogSalona");
                    salon.Name = reader.GetString("Naziv");
                    salon.Address = reader.GetString("Adresa");

                    // Create location object
                    Location location = new Location();
                    location.Name = reader.GetString("Mjesto");
                    salon.Location = location;

                    salons.Add(salon);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error retrieving salons: {ex.Message}");
            }
            finally
            {
                reader?.Close();
                DBUtil.Close(command, connection);
            }
            return salons;
        }

        public static List<BeautySalon> GetBeautySalonsWithFilter(string city, string serviceType)
        {
            List<BeautySalon> salons = new List<BeautySalon>();
            MySqlConnection connection = null;
            MySqlCommand command = null;
            MySqlDataReader reader = null;

            try
            {
                connection = DBUtil.GetConnection();
                command = new MySqlCommand("dohvati_kozmeticki_salon_filter", connection);
                command.CommandType = CommandType.StoredProcedure;


            command.Parameters.AddWithValue("@grad", string.IsNullOrWhiteSpace(city) ? (object)DBNull.Value : city.Trim());
            command.Parameters.AddWithValue("@tip_usluge", string.IsNullOrWhiteSpace(serviceType) ? (object)DBNull.Value : serviceType.Trim());
  

                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BeautySalon salon = new BeautySalon
                    {
                        BeautySalonId = reader.GetInt32("IdKozmetickogSalona"),
                        Name = reader.GetString("Naziv"),
                        Address = reader.GetString("Adresa"),
                        Location = new Location
                        {
                            Name = reader.GetString("Mjesto")
                        }
                    };

                    salons.Add(salon);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error retrieving salons with filter: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"City: {city}, ServiceType: {serviceType}");
                throw;
            }
            finally
            {
                reader?.Close();
                connection?.Close();
                command?.Dispose();
                connection?.Dispose();
            }

            return salons;
        }
        public static bool UpdateBeautySalonInfo(int beautySalonId, string name, string address, string workTime, int locationId, string phone)
        {
            bool result = false;
            MySqlConnection connection = null;
            MySqlCommand command = null;

            string query = "CALL azuriraj_kozmeticki_salon(@pId, @pNaziv, @pAdresa, @pRadnoVrijeme, @pBrojPoste, @pTelefon)";
            try
            {
                connection = DBUtil.GetConnection();
                command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@pId", beautySalonId);
                command.Parameters.AddWithValue("@pNaziv", name);
                command.Parameters.AddWithValue("@pAdresa", address);
                command.Parameters.AddWithValue("@pRadnoVrijeme", workTime);
                command.Parameters.AddWithValue("@pBrojPoste", locationId);
                command.Parameters.AddWithValue("@pTelefon", phone);

                connection.Open();
                result = command.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating salon: {ex.Message}");
            }
            finally
            {
                DBUtil.Close(command, connection);
            }
            return result;
        }

        public static BeautySalon GetBeautySalonByPriceListId(int priceListId)
        {
            MySqlConnection connection = null;
            MySqlCommand command = null;
            MySqlDataReader reader = null;
            int id = -1;

            string query = "SELECT IdKozmetickogSalona FROM kozmeticki_salon WHERE CJENOVNIK_IdCjenovnika=@PriceListId";
            try
            {
                connection = DBUtil.GetConnection();
                command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CJENOVNIK_IdCjenovnika", priceListId);

                connection.Open();
                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    id = reader.GetInt32("IdKozmetickogSalona");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error retrieving beauty salon: {ex.Message}");
            }
            finally
            {
                DBUtil.Close(command, connection);
            }
            return GetBeautySalonById(id);
        }
    }
}
