using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HCI_A.Models;
using HCI_A.Dao;
using System.Globalization;
using HCI_A.Models.Enums;
using HCI_A.Components.EmpoyeeSharedComponents;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Automation.Text;
using HCI_A.Controls;
using HCI_A.Helpers;

namespace HCI_A.Windows
{
    public partial class ManagerMainWindow : Window
    {
        private Employee _manager;
        public const int SALON_INFO_TAB = 0;
        public const int EMPLOYEES_TAB = 1;
        public const int SCHEDULE_TAB = 2;
        public const int SERVICES_TAB = 3;
        public const int PRODUCTS_TAB = 4;
        public const int ORDERS_TAB = 5;
        public const int BILLS_TAB = 6;


        public ManagerMainWindow(Employee manager, int defaultTabIndex = 0)
        {
            InitializeComponent();
            _manager = manager;

            var welcomeRun = new Run();
            welcomeRun.SetResourceReference(Run.TextProperty, "WelcomeMessage");

            var userRun = new Run($"{_manager?.Username}!");

            WelcomeLabel.Inlines.Clear();
            WelcomeLabel.Inlines.Add(welcomeRun);
            WelcomeLabel.Inlines.Add(", ");
            WelcomeLabel.Inlines.Add(userRun);

            MainTabControl.SelectedIndex = defaultTabIndex;

            //LocalizationService.OnLanguageChanged += LocalizationService_OnLanguageChanged;

            LoadSalonInfoTab();
            LoadEmployeesTab();
            LoadScheduleTab();
            LoadServicesTab();
            LoadProductsTab();
            LoadBillsTab();
            OrdersTabControl.Content = new OrdersControl(manager.UserId, manager.BeautySalonId, true);
        }

        private void LoadProductsTab()
        {
            ProductsContent.Children.Clear();
            ProductsContent.Children.Add(ProductsInfo.InitProducts(this, _manager));
        }

        private void LoadSalonInfoTab()
        {
            SalonInfoContent.Children.Clear();
            SalonInfoContent.Children.Add(SalonInfo.InitSalonInfo(this, _manager));
        }

        private void LoadEmployeesTab()
        {
            EmployeesPanel.Children.Clear();

            var employees = EmployeeDao.GetEmployeesBySalonId(_manager.BeautySalonId);

            Border employeesCard = new Border();
            employeesCard.SetResourceReference(Border.StyleProperty, "CardStyle");

            StackPanel cardContent = new StackPanel();

            Grid headerContainer = new Grid();
            headerContainer.Margin = new Thickness(0, 0, 0, 15);
            headerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            TextBlock titleBlock = new TextBlock
            {
                FontSize = 18,
                FontWeight = FontWeights.SemiBold,
                VerticalAlignment = VerticalAlignment.Center
            };
            titleBlock.SetResourceReference(TextBlock.TextProperty, "EmployeesLabel");
            titleBlock.SetResourceReference(TextBlock.ForegroundProperty, "PrimaryBrush");
            Grid.SetColumn(titleBlock, 0);
            headerContainer.Children.Add(titleBlock);

            cardContent.Children.Add(headerContainer);

            Grid tableHeader = new Grid
            {
                Margin = new Thickness(0, 0, 0, 10),
                Height = 40
            };
            tableHeader.SetResourceReference(Grid.BackgroundProperty, "SecondaryLightBrush");

            for (int i = 0; i < 3; i++)
                tableHeader.ColumnDefinitions.Add(new ColumnDefinition());

            TextBlock selectLabel = CreateEmployeeHeaderLabel(0);
            TextBlock usernameLabel = CreateEmployeeHeaderLabel(1);
            TextBlock fullNameLabel = CreateEmployeeHeaderLabel(2);

            selectLabel.SetResourceReference(TextBlock.TextProperty, "SelectLabel");
            usernameLabel.SetResourceReference(TextBlock.TextProperty, "UsernameLabel");
            fullNameLabel.SetResourceReference(TextBlock.TextProperty, "FullNameLabel");

            tableHeader.Children.Add(selectLabel);
            tableHeader.Children.Add(usernameLabel);
            tableHeader.Children.Add(fullNameLabel);

            cardContent.Children.Add(tableHeader);

            foreach (var employee in employees)
            {
                if (employee.AccountType == AccountType.MANAGER) continue;

                Grid row = new Grid
                {
                    Margin = new Thickness(0, 5, 0, 0),
                    Height = 40,
                    Cursor = Cursors.Hand
                };
                row.SetResourceReference(Grid.BackgroundProperty, "CardBrush");

                for (int i = 0; i < 3; i++)
                    row.ColumnDefinitions.Add(new ColumnDefinition());

                RadioButton rb = new RadioButton
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Tag = employee.UserId
                };
                rb.Checked += EmployeeRadioButton_Checked;

                TextBlock usernameBlock = CreateEmployeeRowLabel(employee.Username, TextAlignment.Center);
                TextBlock nameBlock = CreateEmployeeRowLabel($"{employee.FirstName} {employee.LastName}", TextAlignment.Center);

                Grid.SetColumn(rb, 0);
                Grid.SetColumn(usernameBlock, 1);
                Grid.SetColumn(nameBlock, 2);

                row.Children.Add(rb);
                row.Children.Add(usernameBlock);
                row.Children.Add(nameBlock);

                cardContent.Children.Add(row);
            }

            StackPanel buttonContainer = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 20, 0, 0)
            };

            Button registerButton = new Button
            {
                Height = 40,
                Width = 200,
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Cursor = Cursors.Hand
            };
            registerButton.SetResourceReference(Button.ContentProperty, "RegisterNewEmployee");
            registerButton.SetResourceReference(Button.StyleProperty, "PrimaryButtonStyle");

            registerButton.Click += RegisterEmployeeButton_Click;

            buttonContainer.Children.Add(registerButton);
            cardContent.Children.Add(buttonContainer);

            employeesCard.Child = cardContent;
            EmployeesPanel.Children.Add(employeesCard);
        }

 
        private TextBlock CreateEmployeeHeaderLabel(int column)
        {
            TextBlock label = new TextBlock
            {
                FontWeight = FontWeights.SemiBold,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(5),
                FontSize=16
            };
            label.SetResourceReference(TextBlock.ForegroundProperty, "PrimaryDarkBrush");
            Grid.SetColumn(label, column);
            return label;
        }

        private TextBlock CreateEmployeeRowLabel(string text, TextAlignment alignment)
        {
            TextBlock tb = new TextBlock
            {
                Text = text,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = alignment == TextAlignment.Center ? HorizontalAlignment.Center : HorizontalAlignment.Left,
                Padding = new Thickness(5),
                FontSize = 16
            };
            tb.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");
            return tb;
        }

        private void RegisterEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationPage registrationPage = new RegistrationPage(true, _manager.BeautySalonId);
            registrationPage.Show();

            this.Close();
        }


        private void EmployeeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                int employeeId = (int)radioButton.Tag;
                Employee employee = EmployeeDao.GetEmployeeById(employeeId);
                EmployeeInfoWindow employeeInfoWindow = new EmployeeInfoWindow(employee, _manager);
                employeeInfoWindow.Closed += (sender, args) =>
                {
                    radioButton.IsChecked = false;
                };
                employeeInfoWindow.ShowDialog();
            }
        }

        private void LoadScheduleTab()
        {
            try
            {
                ScheduleContent.Children.Clear();
                ScheduleWindow scheduleWindow = new ScheduleWindow(_manager);

                scheduleWindow.HorizontalAlignment = HorizontalAlignment.Stretch;
                scheduleWindow.VerticalAlignment = VerticalAlignment.Stretch;

                ScheduleContent.Children.Add(scheduleWindow);
            }
            catch (Exception ex)
            {
                MessageBox.Show((string)Application.Current.Resources["ScheduleLoadingError"], (string)Application.Current.Resources["Error"], MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadServicesTab()
        {
            ServicesContent.Children.Clear();
            ServicesContent.Children.Add(ServicesInfo.InitServices(this, _manager));
        }

        private void LoadBillsTab()
        {
            BillsPanel.Children.Clear();

            var invoicesControl = new InvoicesControl(0, _manager.BeautySalonId, true);
            BillsPanel.Children.Add(invoicesControl);
        }

        private TextBlock CreateHeaderLabel(string text, int column)
        {
            TextBlock label = new TextBlock();
            label.Text = text;
            label.FontWeight = FontWeights.Bold;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.SetResourceReference(TextBlock.ForegroundProperty, "TextOnPrimaryBrush");
            Grid.SetColumn(label, column);
            return label;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            bool confirmed = CustomYesNoMessageBox.Show("❓", (string)Application.Current.Resources["ConfirmLogoutQuestion"]);

            if (confirmed)
            {
                new LoginPage().Show();
                this.Close();
            }
        }

        public void RefreshProductsTab()
        {
            ManagerMainWindow managerWindow = new ManagerMainWindow(_manager, ManagerMainWindow.PRODUCTS_TAB);
            managerWindow.Show();

            foreach (Window window in Application.Current.Windows)
            {
                if (window is ManagerMainWindow && window != managerWindow)
                {
                    window.Close();
                }
            }
        }

        public void RefreshServicesTab()
        {
            ManagerMainWindow managerWindow = new ManagerMainWindow(_manager, ManagerMainWindow.SERVICES_TAB);
            managerWindow.Show();

            foreach (Window window in Application.Current.Windows)
            {
                if (window is ManagerMainWindow && window != managerWindow)
                {
                    window.Close();
                }
            }
        }

        public void RefreshScheduleTab()
        {
            ManagerMainWindow managerWindow = new ManagerMainWindow(_manager, ManagerMainWindow.SCHEDULE_TAB);
            managerWindow.Show();

            foreach (Window window in Application.Current.Windows)
            {
                if (window is ManagerMainWindow && window != managerWindow)
                {
                    window.Close();
                }
            }
        }

        public void RefreshOrdersTab()
        {
            ManagerMainWindow managerWindow = new ManagerMainWindow(_manager, ManagerMainWindow.ORDERS_TAB);
            managerWindow.Show();

            foreach (Window window in Application.Current.Windows)
            {
                if (window is ManagerMainWindow && window != managerWindow)
                {
                    window.Close();
                }
            }
        }

        public void RefreshBillsTab()
        { 
            ManagerMainWindow managerWindow = new ManagerMainWindow(_manager, 6);
            managerWindow.Show();

            foreach (Window window in Application.Current.Windows)
            {
                if (window is ManagerMainWindow && window != managerWindow)
                {
                    window.Close();
                }
            }
        }

        //private void LocalizationService_OnLanguageChanged()
        //{
        //    RefreshBillsTab();
        //}
    }
}
