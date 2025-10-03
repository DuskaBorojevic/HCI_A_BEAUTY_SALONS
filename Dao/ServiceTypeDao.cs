using HCI_A.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace HCI_A.Dao
{
    public class ServiceTypeDao
    {
        public static List<ServiceType> GetServiceTypes()
        {
            List<ServiceType> types = new List<ServiceType>();
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("SELECT IdTipaUsluge, Naziv FROM tip_usluge", connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        types.Add(new ServiceType(reader.GetInt32("IdTipaUsluge"), reader.GetString("Naziv")));
                    }
                }
            }
            return types;
        }

        public static ServiceType GetServiceTypeById(int serviceTypeId)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("SELECT Naziv FROM tip_usluge WHERE IdTipaUsluge=@serviceTypeId", connection))
            {
                command.Parameters.AddWithValue("@serviceTypeId", serviceTypeId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ServiceType(serviceTypeId, reader.GetString("Naziv"));
                    }
                }
            }
            return null;
        }

        public static bool AddServiceType(string name)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("dodaj_tip_usluge", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pNaziv", name);
                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }

        public static bool DeleteServiceType(int id)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("obrisi_tip_usluge", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pId", id);
                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }

        public static bool UpdateServiceType(int id, string name)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("izmijeni_naziv_tipa_usluge", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pId", id);
                command.Parameters.AddWithValue("@pNaziv", name);
                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }
    }
}
