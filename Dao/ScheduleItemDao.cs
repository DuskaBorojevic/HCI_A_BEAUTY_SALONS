using HCI_A.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Windows.Input;

namespace HCI_A.Dao
{
    public class ScheduleItemDao
    {
        public static bool AddScheduleItem(string day, int employeeId, int beautySalonId, string shiftName, int scheduleId)
        {
            bool result = false;
            using (MySqlConnection connection = DBUtil.GetConnection())
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("dodaj_stavku_rasporeda", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@pDanUSedmici", day);
                        command.Parameters.AddWithValue("@pIdKozmeticara", employeeId);
                        command.Parameters.AddWithValue("@pIdSalona", beautySalonId);
                        command.Parameters.AddWithValue("@pNazivSmjene", shiftName);
                        command.Parameters.AddWithValue("@pIdRasporeda", scheduleId);

                        result = command.ExecuteNonQuery() == 1;
                    }
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error adding schedule item: " + ex.Message);
                    System.Windows.MessageBox.Show("Error adding schedule item: " + ex.Message, "Database Error",
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
            return result;
        }

        public static List<ScheduleItem> GetScheduleItemsByEmployeeAndScheduleId(int employeeId, int scheduleId)
        {
            List<ScheduleItem> scheduleItems = new List<ScheduleItem>();

            using (MySqlConnection connection = DBUtil.GetConnection())
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("dohvati_stavke_rasporeda", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@pIdZaposlenog", employeeId);
                        command.Parameters.AddWithValue("@pIdRasporeda", scheduleId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string day = reader.GetString("DanUSedmici");
                                int beauticianID = reader.GetInt32("KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika");
                                int scheduleID = reader.GetInt32("RASPORED_IdRasporeda");
                                int beautySalonId = EmployeeDao.GetEmployeeById(employeeId).BeautySalonId;
                                Shift shift = ShiftDao.GetShift(beautySalonId, reader.GetString("SMJENA_Naziv"));

                                ScheduleItem item = new ScheduleItem(day, beauticianID, shift, scheduleID);
                                scheduleItems.Add(item);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error retrieving schedule: " + ex.Message);
                }
            }
            return scheduleItems;
        }

        public static ScheduleItem GetScheduleItemByEmployeeAndDay(int employeeId, string dayOfWeek, int scheduleId)
        {
            System.Diagnostics.Debug.WriteLine($"DEBUG: GetScheduleItemByEmployeeAndDay called with EmployeeId={employeeId}, DayOfWeek={dayOfWeek}, ScheduleId={scheduleId}");
            try
            {
                using (MySqlConnection connection = DBUtil.GetConnection())
                {
                    connection.Open();
                    System.Diagnostics.Debug.WriteLine("DEBUG: Database connection opened.");

                    using (MySqlCommand command = new MySqlCommand("dohvati_stavku_rasporeda_dan_zaposleni", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@pIdZaposlenog", employeeId);
                        command.Parameters.AddWithValue("@pDan", dayOfWeek);
                        command.Parameters.AddWithValue("@pIdRasporeda", scheduleId);

                        System.Diagnostics.Debug.WriteLine($"DEBUG: Executing stored procedure 'dohvati_stavku_rasporeda_dan_zaposleni' with params: @pIdZaposlenog={employeeId}, @pDan={dayOfWeek}, @pIdRasporeda={scheduleId}");

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                System.Diagnostics.Debug.WriteLine("DEBUG: Reader found a row.");
                                string readDayOfWeek = reader.GetString(reader.GetOrdinal("DanUSedmici"));
                                int readBeauticianId = reader.GetInt32(reader.GetOrdinal("KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika"));
                                int readScheduleId = reader.GetInt32(reader.GetOrdinal("RASPORED_IdRasporeda"));
                                string shiftName = reader.GetString(reader.GetOrdinal("SMJENA_Naziv")); // This is the shift name from the join
                                int beautySalonId = reader.GetInt32(reader.GetOrdinal("KOZMETICKI_SALON_IdKozmetickogSalona")); // Get salon ID from reader

                                System.Diagnostics.Debug.WriteLine($"DEBUG: Read from DB: Day={readDayOfWeek}, EmployeeId={readBeauticianId}, ScheduleId={readScheduleId}, ShiftName={shiftName}, BeautySalonId={beautySalonId}");

                                // Retrieve the full Shift object using ShiftDao
                                Shift shift = ShiftDao.GetShift(beautySalonId, shiftName);
                                System.Diagnostics.Debug.WriteLine($"DEBUG: Retrieved Shift object: Name={shift?.Name}, FromTime={shift?.FromTime}, ToTime={shift?.ToTime}");

                                ScheduleItem item = new ScheduleItem
                                {
                                    DayOfWeek = readDayOfWeek,
                                    BeauticianId = readBeauticianId,
                                    ScheduleId = readScheduleId,
                                    Shift = shift
                                };
                                System.Diagnostics.Debug.WriteLine($"DEBUG: Returning ScheduleItem for {item.DayOfWeek} with Shift: {item.Shift?.Name ?? "NULL"}");
                                return item;
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("DEBUG: Reader found no rows for the given parameters.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in GetScheduleItemByEmployeeAndDay: {ex.Message}\nStack Trace: {ex.StackTrace}");
                System.Windows.MessageBox.Show($"Error getting schedule item: {ex.Message}", "Database Error",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

            System.Diagnostics.Debug.WriteLine("DEBUG: Returning NULL ScheduleItem.");
            return null;
        }

        public static bool RemoveScheduleItem(int employeeId, string dayOfWeek, int scheduleId)
        {
            try
            {
                using (MySqlConnection connection = DBUtil.GetConnection())
                {
                    connection.Open();

                    string query = "DELETE FROM raspored_stavka " +
                                  "WHERE KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika = @KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika " +
                                  "AND DanUSedmici = @DanUSedmici " +
                                  "AND RASPORED_IdRasporeda = @RASPORED_IdRasporeda";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika", employeeId);
                        command.Parameters.AddWithValue("@DanUSedmici", dayOfWeek);
                        command.Parameters.AddWithValue("@RASPORED_IdRasporeda", scheduleId);

                        return command.ExecuteNonQuery() >= 0; 
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing schedule item: {ex.Message}");
                return false;
            }
        }

        public static bool AddOrUpdateScheduleItem(string dayOfWeek, int employeeId, int beautySalonId, string shiftName, int scheduleId)
        {
            try
            {
                ScheduleItem existingItem = GetScheduleItemByEmployeeAndDay(employeeId, dayOfWeek, scheduleId);

                using (MySqlConnection connection = DBUtil.GetConnection())
                {
                    connection.Open();

                    string query;

                    if (existingItem != null)
                    {
                        query = "UPDATE raspored_stavka SET SMJENA_Naziv = @SMJENA_Naziv " +
                               "WHERE KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika = @KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika " +
                               "AND DanUSedmici = @DanUSedmici " +
                               "AND RASPORED_IdRasporeda = @RASPORED_IdRasporeda";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@SMJENA_Naziv", shiftName);
                            command.Parameters.AddWithValue("@KOZMETICAR_ZAPOSLENI_KORISNIK_IdKorisnika", employeeId);
                            command.Parameters.AddWithValue("@DanUSedmici", dayOfWeek);
                            command.Parameters.AddWithValue("@RASPORED_IdRasporeda", scheduleId);

                            return command.ExecuteNonQuery() > 0;
                        }
                    }
                    else
                    {
                        return AddScheduleItem(dayOfWeek, employeeId, beautySalonId, shiftName, scheduleId);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding/updating schedule item: {ex.Message}");
                System.Windows.MessageBox.Show($"Error adding/updating schedule item: {ex.Message}", "Database Error",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
        }
    }
}

