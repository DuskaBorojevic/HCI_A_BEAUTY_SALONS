using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using HCI_A.Dao;
using HCI_A.Models;

namespace HCI_A.Windows
{
    public partial class EmployeeInfoWindow : Window, IRefreshable
    {
        private readonly Employee _employee;
        private readonly Employee _manager;

        public EmployeeInfoWindow(Employee employee, Employee manager)
        {
            InitializeComponent();

            _employee = employee;
            _manager = manager;

            EmployeeNameText.Text = $"{_employee.FirstName} {_employee.LastName}";

            LoadData();
        }

        private void LoadData()
        {
            UsernameText.Text = _employee.Username;
            EmailText.Text = _employee.EmailAddress;
            FullNameText.Text = _employee.FirstName + " " + _employee.LastName;
            AddressText.Text = _employee.Address;
            EmploymentDateText.Text = _employee.EmploymentDate.ToString("dd. MM. yyyy.", CultureInfo.InvariantCulture);

            SalaryText.Text = $"{_employee.Salary} KM";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

        private void EditSalaryButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EditSalaryDialog(this, _employee, _manager);
            dialog.ShowDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool confirmed = CustomYesNoMessageBox.Show((string)Application.Current.Resources["ConfirmDelete"]);

            if (confirmed)
            {
                bool success = EmployeeDao.DeleteEmployeeById(_employee.UserId);
                if (success)
                {
                    CustomOkMessageBox.ShowDeletedSuccess();
                    Close();
                    RefreshData();
                }
                else
                {
                    CustomOkMessageBox.ShowDeleteFailed();
                }
            }
        }

        public void RefreshData()
        {
            var managerWindow = new ManagerMainWindow(_manager, ManagerMainWindow.EMPLOYEES_TAB);
            managerWindow.Show();

            foreach (Window w in Application.Current.Windows)
            {
                if (w is ManagerMainWindow && w != managerWindow)
                    w.Close();
            }
        }
    }

    public class EditSalaryDialog : Window
    {
        private readonly Window _parentWindow;
        private readonly Employee _employee;
        private readonly Employee _manager;
        private TextBox _salaryTextBox;

        public EditSalaryDialog(Window parentWindow, Employee employee, Employee manager)
        {
            _parentWindow = parentWindow;
            _employee = employee;
            _manager = manager;

            SetResourceReference(Window.TitleProperty, "EditSalaryTitle");
            Width = 420;
            Height = 220;
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = parentWindow;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            var grid = new Grid { Margin = new Thickness(20) };
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var header = new TextBlock
            {
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 15)
            };
            header.SetResourceReference(TextBlock.TextProperty, "EditSalaryDescription");
            Grid.SetRow(header, 0);

            _salaryTextBox = new TextBox
            {
                Text = _employee.Salary.ToString(CultureInfo.InvariantCulture),
                Padding = new Thickness(8, 5, 8, 5),
                Margin = new Thickness(0, 0, 0, 15)
            };
            _salaryTextBox.SetResourceReference(TextBox.BackgroundProperty, "ElevatedSurfaceBrush");
            Grid.SetRow(_salaryTextBox, 1);

            var saveButton = new Button
            {
                Padding = new Thickness(15, 8, 15, 8),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            saveButton.SetResourceReference(Button.ContentProperty, "BtnSubmit");
            saveButton.SetResourceReference(Button.StyleProperty, "SuccessTextButton");

            saveButton.Foreground = System.Windows.Media.Brushes.White;
            saveButton.Click += SaveButton_Click;
            Grid.SetRow(saveButton, 2);

            grid.Children.Add(header);
            grid.Children.Add(_salaryTextBox);
            grid.Children.Add(saveButton);

            Content = grid;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(_salaryTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var salary))
            {
                CustomOkMessageBox.Show(
                    (string)Application.Current.Resources["SalaryValidationError"],
                    (string)Application.Current.Resources["ValidationError"],
                    "⚠"
                );
                return;
            }

            bool result = EmployeeDao.EditSalary(_employee.UserId, salary);

            if (result)
            {
                CustomOkMessageBox.ShowMsgSuccessUpdate();
                Close();

                if (_parentWindow is EmployeeInfoWindow w)
                    w.RefreshData();
            }
            else
            {
                CustomOkMessageBox.ShowMsgErrorUpdate();
            }
        }
    }
}
