using HCI_A.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HCI_A.Windows
{
    public partial class RegistrationPage : Window
    {
        private bool _isEmployee = false;
        private int _beautySalonId;
        public RegistrationPage(bool isEmployee, int beautySalonId)
        {
            InitializeComponent();
            _isEmployee = isEmployee;
            _beautySalonId = beautySalonId;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email))
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["RequiredFieldsError"], (string)Application.Current.Resources["ValidationError"], "⚠");
                return;
            }
            bool result = false;
            if (!_isEmployee)
            {
                result = ClientDao.RegisterClient(username, password, firstName, lastName, email, "CLIENT");
            }
            else
            {
                result = EmployeeDao.RegisterEmployee(username, password, email, "BEAUTICIAN", firstName, lastName, _beautySalonId);
            }

            if (result)
            {
                string message = (string)FindResource("RegistrationSuccessful") + " " + username + "!";
                CustomOkMessageBox.Show(message,
                                (string)FindResource("Success"),
                                "👏🏼");
                LoginPage login = new LoginPage();
                login.Show();
                this.Close();
            }
            else
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["RegistrationFailed"], (string)Application.Current.Resources["Error"], "⚠");
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginPage login = new LoginPage();
            login.Show();
            this.Close();
        }
    }
}
