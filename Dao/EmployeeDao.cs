using HCI_A.Models.Enums;
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
    public class EmployeeDao
    {
        public static List<Employee> GetEmployeesBySalonId(int salonId)
        {
            List<Employee> employees = new List<Employee>();
            MySqlConnection connection = null;
            MySqlCommand command = null;
            try
            {
                connection = DBUtil.GetConnection();
                command = new MySqlCommand("dohvati_zaposlene", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pIdKs", salonId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee employee = new Employee(reader.GetInt32(reader.GetOrdinal("IdKorisnika")),
                            reader.GetString(reader.GetOrdinal("KorisnickoIme")),
                            reader.GetString(reader.GetOrdinal("Lozinka")),
                            reader.GetString(reader.GetOrdinal("EmailAdresa")),
                            Enum.Parse<AccountType>(reader.GetString(reader.GetOrdinal("TipNaloga")).ToUpper()),
                            reader.GetString(reader.GetOrdinal("Ime")),
                            reader.GetString(reader.GetOrdinal("Prezime")),
                            reader.GetString(reader.GetOrdinal("Adresa")),
                            reader.GetDateTime(reader.GetOrdinal("DatumZaposlenja")),
                            reader.GetDouble(reader.GetOrdinal("Plata")),
                            reader.GetInt32(reader.GetOrdinal("KOZMETICKI_SALON_IdKozmetickogSalona")));

                        employees.Add(employee);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
            }
            finally
            {
                DBUtil.Close(command, connection);
            }
            return employees;
        }

        public static Employee GetEmployeeById(int userId)
        {
            Employee employee = null;
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("dohvati_zaposlenog", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pId", userId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32("IdKorisnika");
                        string username = reader.GetString("KorisnickoIme");
                        string password = reader.GetString("Lozinka");
                        string emailAddress = reader.GetString("EmailAdresa");
                        AccountType accountType = Enum.Parse<AccountType>(reader.GetString("TipNaloga").ToUpper());
                        string firstName = reader.GetString("Ime");
                        string lastName = reader.GetString("Prezime");
                        string address = reader.GetString("Adresa");
                        DateTime employmentDate = reader.GetDateTime("DatumZaposlenja");
                        double salary = reader.GetDouble("Plata");
                        int beautySalonId = reader.GetInt32("KOZMETICKI_SALON_IdKozmetickogSalona");

                        employee = new Employee(id, username, password, emailAddress, accountType, firstName, lastName, address, employmentDate, salary, beautySalonId);
                    }
                }
            }
            return employee;
        }

        public static bool DeleteEmployeeById(int userId)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("ukloni_kozmeticara", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pId", userId);

                connection.Open();
                //return command.ExecuteNonQuery() == 1;
                return command.ExecuteNonQuery() >= 0;
            }
        }

        public static bool EditSalary(int employeeId, double newSalary)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("izmijeni_platu", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pId", employeeId);
                command.Parameters.AddWithValue("@pPlata", newSalary);

                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }

        public static int GetEmployeeIdByName(string name)
        {
            int id = -1;
            string[] parts = name.Split(' ');
            if (parts.Length < 2) return id;
            string firstName = parts[0];
            string lastName = parts[1];

            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("dohvati_id_zaposlenog", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pIme", firstName);
                command.Parameters.AddWithValue("@pPrezime", lastName);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        id = reader.GetInt32("KORISNIK_IdKorisnika");
                    }
                }
            }
            return id;
        }

        public static int GetManagerIdBySalonId(int salonId)
        {
            int id = -1;
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("dohvati_menadzera", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pIdSalona", salonId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        id = reader.GetInt32("ZAPOSLENI_KORISNIK_IdKorisnika");
                    }
                }
            }
            return id;
        }

        public static bool RegisterEmployee(
        string username, string password, string email, string type,
        string firstName, string lastName, int salonId)
        {
            bool result = false;

            using (MySqlConnection connection = DBUtil.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand("dodaj_zaposlenog", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@pKorisnickoIme", username);
                        command.Parameters.AddWithValue("@pLozinka", password);
                        command.Parameters.AddWithValue("@pEmailAdresa", email);
                        command.Parameters.AddWithValue("@pTipNaloga", type);
                        command.Parameters.AddWithValue("@pIme", firstName);
                        command.Parameters.AddWithValue("@pPrezime", lastName);
                        command.Parameters.AddWithValue("@pIdSalona", salonId);

                        command.ExecuteNonQuery();
                        result = true;
                    }
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error while adding employee: " + ex.Message);
                }
            }
            return result;
        }

    }
}
