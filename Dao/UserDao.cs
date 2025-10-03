using HCI_A.Models;
using HCI_A.Models.Enums;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace HCI_A.Dao
{
    public class UserDao
    {
        private static int _currentUserId = -1;

        public static void SetCurrentUserId(int userId)
        {
            _currentUserId = userId;
        }

        public static int GetCurrentUserId()
        {
            return _currentUserId;
        }

        public static void ClearCurrentUser()
        {
            _currentUserId = -1;
        }
        public static User Login(string username, string password)
        {
            User user = null;
            MySqlConnection connection = null;
            MySqlCommand command = null;

            try
            {
                connection = DBUtil.GetConnection();
                command = new MySqlCommand("dohvati_korisnika", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@pKorisnickoIme", username);
                command.Parameters.AddWithValue("@pLozinka", password);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        System.Diagnostics.Debug.WriteLine("User found in database!");

                        int userId = reader.GetInt32("IdKorisnika");
                        string email = reader.GetString("EmailAdresa");
                        string accountTypeName = reader.GetString("TipNaloga");

                        string theme = reader.IsDBNull(reader.GetOrdinal("Tema")) ? null : reader.GetString("Tema");
                        string language = reader.IsDBNull(reader.GetOrdinal("Jezik")) ? null : reader.GetString("Jezik");

                        System.Diagnostics.Debug.WriteLine($"Retrieved userId={userId}, email={email}, accountTypeName={accountTypeName}");

                        AccountType accountType;
                        try
                        {
                            accountType = (AccountType)Enum.Parse(typeof(AccountType), accountTypeName.ToUpper());
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine($"Enum parse error: {e.Message}");
                            throw;
                        }

                        user = new User(userId, username, password, email, accountType, theme, language);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No user found with given credentials.");
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
            return user;
        }

       

        public static Employee GetCurrentUserAsEmployee()
        {
            if (_currentUserId == -1)
                return null;

            return EmployeeDao.GetEmployeeById(_currentUserId);
        }

        public static AccountType GetUserAccountTypeById(int id)
        {
            AccountType result = AccountType.CLIENT; 
            MySqlConnection connection = null;
            MySqlCommand command = null;

            try
            {
                connection = DBUtil.GetConnection();
                command = new MySqlCommand("SELECT TipNaloga FROM korisnik WHERE Id=@id", connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string tipNaloga = reader.GetString("TipNaloga");

                        if (Enum.TryParse(tipNaloga, true, out AccountType parsedType))
                        {
                            result = parsedType;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                DBUtil.Close(command, connection);
            }

            return result;
        }

        public static bool UpdateAccountInfo(int userId, string username, string password,
            string firstName, string lastName, string emailAddress, string accountType)
        {
            bool result = false;
            MySqlConnection connection = null;
            MySqlCommand command = null;

            try
            {
                connection = DBUtil.GetConnection();
                command = new MySqlCommand("azuriraj_korisnika", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@pId", userId);
                command.Parameters.AddWithValue("@pKorisnickoIme", username);
                command.Parameters.AddWithValue("@pLozinka", password);
                command.Parameters.AddWithValue("@pIme", firstName);
                command.Parameters.AddWithValue("@pPrezime", lastName);
                command.Parameters.AddWithValue("@pEmailAdresa", emailAddress);
                command.Parameters.AddWithValue("@pTipNaloga", accountType);

                connection.Open();
                result = command.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating user: {ex.Message}");
            }
            finally
            {
                DBUtil.Close(command, connection);
            }
            return result;
        }

        public static Client GetUserInfoById(int id)
        {
            Client client = null;

            using (MySqlConnection connection = DBUtil.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand("dohvati_korisnicke_info", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pId", id);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            client = new Client(
                                reader.GetInt32("Id"),
                                reader.GetString("KorisnickoIme"),
                                reader.GetString("Ime"),
                                reader.GetString("Prezime"),
                                reader.GetString("Email")
                            );
                        }
                    }
                }
            }

            return client;
        }

        public static void UpdateTheme(int userId, string theme)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            {
                connection.Open();
                string query = "UPDATE korisnik SET Tema=@theme WHERE IdKorisnika=@id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@theme", theme);
                    command.Parameters.AddWithValue("@id", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateLanguage(int userId, string lang)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            {
                connection.Open();
                string query = "UPDATE korisnik SET Jezik=@lang WHERE IdKorisnika=@id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@lang", lang);
                    command.Parameters.AddWithValue("@id", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

    }


}
