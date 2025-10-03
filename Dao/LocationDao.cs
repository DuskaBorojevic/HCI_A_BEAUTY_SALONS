using HCI_A.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Dao
{
    public class LocationDao
    {
        public static int GetLocationIdByName(string name)
        {
            int id = -1;
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("SELECT BrojPoste FROM mjesto WHERE Naziv=@Name", connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                connection.Open();
                id = (int?)command.ExecuteScalar() ?? -1;
            }
            return id;
        }

        public static Location GetLocationById(int id)
        {
            Location location = null;
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM mjesto WHERE BrojPoste=@Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int postNumber = reader.GetInt32(reader.GetOrdinal("BrojPoste"));
                        string name = reader.GetString(reader.GetOrdinal("Naziv"));
                        location = new Location(postNumber, name);
                    }
                }
            }
            return location;
        }

        public static List<Location> GetCities()
        {
            List<Location> locations = new List<Location>();
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("SELECT BrojPoste, Naziv FROM mjesto ORDER BY Naziv", connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        locations.Add(new Location(reader.GetInt32("BrojPoste"), reader.GetString("Naziv")));
                    }
                }
            }
            return locations;
        }
    }
}
