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
    public class ServiceDao
    {
        public static List<Service> GetServicesByPriceListId(int priceListId)
        {
            List<Service> services = new List<Service>();
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("dohvati_usluge", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pIdCj", priceListId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ServiceType st = new ServiceType(reader.GetInt32("TIP_USLUGE_IdTipaUsluge"), reader.GetString("TipNaziv"));
                        Service service = new Service(
                            reader.GetInt32("IdUsluge"),
                            reader.GetString("Naziv"),
                            reader.GetDouble("Cijena"),
                            reader.GetTimeSpan("PotrebnoVrijeme"),
                            reader.GetString("Opis"),
                            priceListId,
                            st);
                        services.Add(service);
                    }
                }
            }
            return services;
        }

        public static bool AddService(string name, double price, TimeSpan durationTime, string description, int priceListId, int serviceTypeId)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("dodaj_uslugu", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pNaziv", name);
                command.Parameters.AddWithValue("@pCijena", price);
                command.Parameters.AddWithValue("@pPotrebnoVrijeme", durationTime);
                command.Parameters.AddWithValue("@pOpis", description);
                command.Parameters.AddWithValue("@pIdCjenovnika", priceListId);
                command.Parameters.AddWithValue("@pIdTipaUsluge", serviceTypeId);
                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }

        public static Service GetServiceById(int serviceId)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM usluga WHERE IdUsluge=@serviceId", connection))
            {
                command.Parameters.AddWithValue("@serviceId", serviceId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ServiceType st = ServiceTypeDao.GetServiceTypeById(reader.GetInt32("TIP_USLUGE_IdTipaUsluge"));
                        return new Service(
                            reader.GetInt32("IdUsluge"),
                            reader.GetString("Naziv"),
                            reader.GetDouble("Cijena"),
                            reader.GetTimeSpan("PotrebnoVrijeme"),
                            reader.GetString("Opis"),
                            reader.GetInt32("CJENOVNIK_IdCjenovnika"),
                            st);
                    }
                }
            }
            return null;
        }

        public static bool UpdateService(int id, string name, double price, TimeSpan time, string description, int priceListId, int serviceTypeId)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("azuriraj_uslugu", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pId", id);
                command.Parameters.AddWithValue("@pNaziv", name);
                command.Parameters.AddWithValue("@pCijena", price);
                command.Parameters.AddWithValue("@pVrijeme", time);
                command.Parameters.AddWithValue("@pOpis", description);
                command.Parameters.AddWithValue("@pIdCjenovnika", priceListId);
                command.Parameters.AddWithValue("@pIdTipaUsluge", serviceTypeId);
                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }

        public static bool DeleteService(int id)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("obrisi_uslugu", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pId", id);
                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }
    }
}
