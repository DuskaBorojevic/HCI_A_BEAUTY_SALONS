using HCI_A.Dao;
using HCI_A.Models.Enums;
using HCI_A.Models;
using System;
using System.Windows;
using MySql.Data.MySqlClient;
using HCI_A.Helpers;

namespace HCI_A.Windows
{
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string username = txtUsername.Text;
                string password = txtPassword.Password;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    CustomOkMessageBox.Show((string)Application.Current.Resources["NonEmptyLoginMessage"], (string)Application.Current.Resources["ValidationError"], "⚠");
                    return;
                }

                User user = UserDao.Login(username, password);

                if (user == null)
                {
                    CustomOkMessageBox.Show((string)Application.Current.Resources["FailedLoginMessage"], (string)Application.Current.Resources["ValidationError"], "✖️");
                }
                else
                {
                    AppSession.CurrentUser = user;
                    string selectedTheme = string.IsNullOrEmpty(user.Theme) ? "Light" : user.Theme;
                    string selectedLanguage = string.IsNullOrEmpty(user.Language) ? "en" : user.Language;

                    ThemeManager.SetTheme(selectedTheme);
                    LocalizationService.SetLanguage(selectedLanguage);


                    switch (user.AccountType)
                    {
                        case AccountType.CLIENT:
                            this.Hide();
                            new ClientMainWindow(user).Show();
                            break;

                        case AccountType.BEAUTICIAN:
                            this.Hide();
                            Employee beautician = EmployeeDao.GetEmployeeById(user.UserId);
                            new BeauticianMainWindow(beautician).Show();
                            break;

                        case AccountType.MANAGER:
                            this.Hide();
                            Employee manager = EmployeeDao.GetEmployeeById(user.UserId);
                            new ManagerMainWindow(manager).Show();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
            }
        }

        private void GoToRegistration_Click(object sender, RoutedEventArgs e)
        {
            RegistrationPage registration = new RegistrationPage(false, -1);
            registration.Show();
            this.Close();
        }
    }
}