using HCI_A.Components.EmpoyeeSharedComponents;
using HCI_A.Helpers;
using HCI_A.Models;
using HCI_A.Windows;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Dao
{
    public class ShiftDao
    {
        public static List<Shift> GetShiftsByBeautySalonId(int beautySalonId)
        {
            List<Shift> shifts = new List<Shift>();
            string query = "SELECT * FROM smjena WHERE KOZMETICKI_SALON_IdKozmetickogSalona = @beautySalonId";

            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@beautySalonId", beautySalonId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader["Naziv"].ToString();
                        int salonId = Convert.ToInt32(reader["KOZMETICKI_SALON_IdKozmetickogSalona"]);
                        TimeSpan from = ((TimeSpan)reader["Od"]);
                        TimeSpan to = ((TimeSpan)reader["Do"]);

                        shifts.Add(new Shift(name, salonId, from, to));
                    }
                }
            }
            return shifts;
        }

        public static Shift GetShift(int beautySalonId, string name)
        {
            System.Diagnostics.Debug.WriteLine($"DEBUG: ShiftDao.GetShift called for Name='{name}', BeautySalonId={beautySalonId}");
            Shift shift = null;
            string query = "kozmeticki_salon.dohvati_smjenu";
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure; 
                command.Parameters.AddWithValue("@pNaziv", name); 
                command.Parameters.AddWithValue("@pIdKozmetickogSalona", beautySalonId); 

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nameOfShift = reader["Naziv"].ToString();
                            int salonId = Convert.ToInt32(reader["KOZMETICKI_SALON_IdKozmetickogSalona"]);
                            TimeSpan from = ((TimeSpan)reader["Od"]);
                            TimeSpan to = ((TimeSpan)reader["Do"]);
                            shift = new Shift(nameOfShift, salonId, from, to);
                            System.Diagnostics.Debug.WriteLine($"DEBUG: ShiftDao.GetShift found shift: {shift.Name} (From: {shift.FromTime}, To: {shift.ToTime})");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"DEBUG: ShiftDao.GetShift found no shift for Name='{name}', BeautySalonId={beautySalonId}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ERROR in ShiftDao.GetShift: {ex.Message}\nStack Trace: {ex.StackTrace}");
                    System.Windows.MessageBox.Show($"Error in ShiftDao.GetShift: {ex.Message}", "Database Error",
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
            return shift;
        }

        public static bool CreateShift(string name, int beautySalonId, string from, string to)
        {
            if (!TimeSpan.TryParse(from, out TimeSpan startTime) ||
                !TimeSpan.TryParse(to, out TimeSpan endTime))
            {
                CustomOkMessageBox.Show(
                    LocalizationService.GetString("TimeValidationError"),
                     LocalizationService.GetString("ValidationError"),
                    "⚠"
                );
                return false;
            }

            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("kreiraj_smjenu", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pNaziv", name);
                command.Parameters.AddWithValue("@pIdKozmetickogSalona", beautySalonId);
                command.Parameters.AddWithValue("@pOd", startTime);
                command.Parameters.AddWithValue("@pDo", endTime);

                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }

        public static bool UpdateShift(string name, int beautySalonId, string from, string to)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("azuriraj_smjenu", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pNaziv", name);
                command.Parameters.AddWithValue("@pIdKozmetickogSalona", beautySalonId);
                command.Parameters.AddWithValue("@pOd", TimeSpan.Parse(from));
                command.Parameters.AddWithValue("@pDo", TimeSpan.Parse(to));

                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }

        public static bool DeleteShift(string name, int beautySalonId)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("obrisi_smjenu", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pNaziv", name);
                command.Parameters.AddWithValue("@pIdKozmetickogSalona", beautySalonId);

                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }
    }
}
