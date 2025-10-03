using System;
using System.Windows;
using System.Windows.Controls;
using HCI_A.Models;
using HCI_A.Dao;
using HCI_A.Models.Enums;
using HCI_A.Controls;

namespace HCI_A.Windows
{
    public partial class ProfileWindow : Window
    {
        private User _currentUser;
        private Client _clientInfo;

        private bool _isEditMode = false;

        public ProfileWindow(User user, int defaultTabIndex = 0)
        {
            InitializeComponent();
            _currentUser = user;

            if (user.AccountType == AccountType.CLIENT)
            {
                _clientInfo = UserDao.GetUserInfoById(user.UserId);
            }

            LoadUserData();

            if (user.AccountType == AccountType.CLIENT)
            {
                OrdersTabControl.Content = new OrdersControl(_currentUser.UserId, -1, false);
                CartTabControl.Content = new CartControl(_currentUser.UserId);
                InvoicesTabControl.Content = new InvoicesControl(_currentUser.UserId, null, false);
                MainTabControl.SelectedIndex = defaultTabIndex;
            }
            else if (user.AccountType == AccountType.BEAUTICIAN)
            {
                CartTabControl.Visibility = Visibility.Collapsed;
                OrdersTabControl.Visibility = Visibility.Collapsed;
                InvoicesTabControl.Visibility = Visibility.Collapsed;

                MainTabControl.SelectedIndex = 0;
            }
        }

        private void LoadUserData()
        {
            UsernameTextBox.Text = _currentUser.Username;
            PasswordTextBox.Text = new string('*', _currentUser.Password.Length);
            EmailTextBox.Text = _currentUser.EmailAddress;

            if (_currentUser.AccountType == AccountType.CLIENT)
            {
                FirstNameTextBox.Text = _clientInfo.FirstName;
                LastNameTextBox.Text = _clientInfo.LastName;
            }
            else if (_currentUser.AccountType == AccountType.BEAUTICIAN)
            {
                Employee employee = EmployeeDao.GetEmployeeById(_currentUser.UserId);

                FirstNameTextBox.Text = employee.FirstName;
                LastNameTextBox.Text = employee.LastName;

                SalaryLabel.Visibility = Visibility.Visible;
                SalaryTextBox.Visibility = Visibility.Visible;
                SalaryTextBox.Text = $"{employee.Salary:F2} KM";
            }
        }

        private void EditSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isEditMode)
            {
                ToggleEditMode(true);
                EditSaveButton.SetResourceReference(StyleProperty, "PrimaryLargeButton");
                EditSaveButton.SetResourceReference(ContentProperty, "BtnSave");
                _isEditMode = true;
            }
            else
            {
                SaveButton_Click(sender, e);
            }
        }

        private void ToggleEditMode(bool isEditable)
        {
            UsernameTextBox.IsReadOnly = !isEditable;
            PasswordTextBox.IsReadOnly = !isEditable;
            FirstNameTextBox.IsReadOnly = !isEditable;
            LastNameTextBox.IsReadOnly = !isEditable;
            EmailTextBox.IsReadOnly = !isEditable;

            if (isEditable)
            {
                UsernameTextBox.BorderThickness = new Thickness(1);
                PasswordTextBox.Text = _currentUser.Password;
                PasswordTextBox.BorderThickness = new Thickness(1);
                FirstNameTextBox.BorderThickness = new Thickness(1);
                LastNameTextBox.BorderThickness = new Thickness(1);
                EmailTextBox.BorderThickness = new Thickness(1);
            }
            else
            {
                UsernameTextBox.BorderThickness = new Thickness(0);
                PasswordTextBox.BorderThickness = new Thickness(0);
                FirstNameTextBox.BorderThickness = new Thickness(0);
                LastNameTextBox.BorderThickness = new Thickness(0);
                EmailTextBox.BorderThickness = new Thickness(0);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordTextBox.Text) ||
                string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["AllFieldsRequired"], (string)Application.Current.Resources["ValidationError"], "⚠");
                return;
            }

            bool result = UserDao.UpdateAccountInfo(
                _currentUser.UserId,
                UsernameTextBox.Text,
                PasswordTextBox.Text,
                FirstNameTextBox.Text,
                LastNameTextBox.Text,
                EmailTextBox.Text,
                _currentUser.AccountType.ToString());

            if (result)
            {
                CustomOkMessageBox.ShowMsgSuccessUpdate();
                _currentUser.Username = UsernameTextBox.Text;
                _currentUser.Password = PasswordTextBox.Text;
                _currentUser.EmailAddress = EmailTextBox.Text;
                _clientInfo.FirstName = FirstNameTextBox.Text;
                _clientInfo.LastName = LastNameTextBox.Text;

                ToggleEditMode(false);
                EditSaveButton.SetResourceReference(ContentProperty, "BtnEdit");
                PasswordTextBox.Text = new string('*', _currentUser.Password.Length);
                _isEditMode = false;
            }
            else
            {
                CustomOkMessageBox.ShowMsgErrorUpdate();
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
    }
}