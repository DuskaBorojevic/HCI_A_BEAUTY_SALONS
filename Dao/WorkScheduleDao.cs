using HCI_A.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace HCI_A.Dao
{
    public class WorkScheduleDao
    {
        public static int CreateNewSchedule(int managerId, string fromDate, string toDate)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand("kreiraj_raspored", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@pIdMenadzera", managerId);
                        command.Parameters.AddWithValue("@pOd", DateTime.Parse(fromDate));
                        command.Parameters.AddWithValue("@pDo", DateTime.Parse(toDate));

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetInt32("NoviId");
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error creating schedule: " + ex.Message);
                }
            }
            return -1;
        }

        public static int GetLastScheduleByManagerId(int managerId)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand("dohvati_id_posljednjeg_rasporeda", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@managerId", managerId);
                    MySqlParameter outputParam = new MySqlParameter("@NoviId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputParam);

                    connection.Open();
                    command.ExecuteNonQuery();

                    return (int)(outputParam.Value ?? -1);
                }
            }
        }

        public static WorkSchedule GetScheduleById(int id)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand("SELECT * FROM raspored WHERE IdRasporeda = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int scheduleId = reader.GetInt32(reader.GetOrdinal("IdRasporeda"));
                            int managerId = reader.GetInt32(reader.GetOrdinal("MENADZER_ZAPOSLENI_KORISNIK_IdKorisnika"));
                            DateTime from = reader.GetDateTime(reader.GetOrdinal("Od"));
                            DateTime to = reader.GetDateTime(reader.GetOrdinal("Do"));

                            return new WorkSchedule(scheduleId, managerId, from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd"));
                        }
                    }
                }
            }
            return null;
        }

        public static List<WorkSchedule> GetWorkSchedulesByBeautySalonId(int beautySalonId)
        {
            List<WorkSchedule> schedules = new List<WorkSchedule>();

            try
            {
                using (MySqlConnection connection = DBUtil.GetConnection())
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("dohvati_rasporede_za_kozmeticki_salon", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@pIdSalona", beautySalonId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                WorkSchedule schedule = new WorkSchedule
                                {
                                    ScheduleId = reader.GetInt32(reader.GetOrdinal("IdRasporeda")),
                                    ManagerId = reader.GetInt32(reader.GetOrdinal("MENADZER_ZAPOSLENI_KORISNIK_IdKorisnika")),
                                    From = reader.GetDateTime(reader.GetOrdinal("Od")).ToString("yyyy-MM-dd"),
                                    To = reader.GetDateTime(reader.GetOrdinal("Do")).ToString("yyyy-MM-dd")
                                };

                                schedules.Add(schedule);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting schedules: {ex.Message}");
            }

            return schedules;
        }

        public static bool DeleteSchedule(int id)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("obrisi_raspored", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pId", id);
                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }
    }
}
