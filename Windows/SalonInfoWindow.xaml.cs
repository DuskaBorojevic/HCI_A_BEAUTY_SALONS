using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using HCI_A.Models;
using HCI_A.Dao;
using HCI_A.Models.Enums;
using System.Linq;
using HCI_A.Helpers;

namespace HCI_A.Windows
{
    public partial class SalonInfoWindow : Window
    {
        private BeautySalon _beautySalon;
        private AccountType _accountType;
        private bool _isManager;

        public SalonInfoWindow(int beautySalonId, int userId)
        {
            InitializeComponent();

            _accountType = UserDao.GetUserAccountTypeById(userId);
            _beautySalon = BeautySalonDao.GetBeautySalonById(beautySalonId);

            _isManager = _accountType == AccountType.MANAGER;
            string infoText = (string)Application.Current.FindResource("Information");
            this.Title = _beautySalon.Name + " - " + infoText;


            LoadSalonData();

        }

        private void LoadSalonData()
        {
            Boolean isClient= _accountType == AccountType.CLIENT;
            if (isClient)
            {
                SalonNameText.Text = _beautySalon.Name + " 🌸";
            }
            else
            {
                SalonNameText.Text = _beautySalon.Name;
            }

            AddressValueText.Text = _beautySalon.Address;
            WorkTimeText.Text = _beautySalon.WorkTime;
            PlaceValueText.Text = _beautySalon.Location.Name;
            PhoneText.Text = _beautySalon.PhoneNumber;

            LoadEmployees();
        }

        private void LoadEmployees()
        {
            EmployeesPanel.Children.Clear();

            try
            {
                var employees = _beautySalon.Employees ?? EmployeeDao.GetEmployeesBySalonId(_beautySalon.BeautySalonId);

                if (employees == null || !employees.Any())
                {
                    TextBlock noEmployeeText = new TextBlock
                    {
                        FontStyle = FontStyles.Italic,
                        Margin = new Thickness(0, 10, 0, 10)
                    };
                    noEmployeeText.SetResourceReference(TextBlock.TextProperty, "LoadingError");
                    noEmployeeText.SetResourceReference(TextBlock.TextProperty, "TextSecondaryBrush");
                    EmployeesPanel.Children.Add(noEmployeeText);
                    return;
                }

                foreach (var employee in employees)
                {
                    Border employeeBorder = new Border
                    {
                        CornerRadius = new CornerRadius(6),
                        Margin = new Thickness(0, 0, 0, 8),
                        Padding = new Thickness(15, 10, 15, 10),
                        BorderThickness = new Thickness(1)
                    };
                    employeeBorder.SetResourceReference(BackgroundProperty, "ElevatedSurfaceBrush");
                    employeeBorder.SetResourceReference(BorderBrushProperty, "BorderBrush");
                    employeeBorder.SetResourceReference(BackgroundProperty, "CardBrush");
                    Grid employeeGrid = new Grid();
                    employeeGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    employeeGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                    if (_isManager)
                    {
                        employeeGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    }

                    StackPanel employeeInfo = new StackPanel
                    {
                        Orientation = Orientation.Vertical
                    };
                    employeeInfo.SetResourceReference(BackgroundProperty, "CardBrush");

                    TextBlock nameText = new TextBlock
                    {
                        Text = $"{employee.FirstName} {employee.LastName}",
                        FontWeight = FontWeights.SemiBold,
                        FontSize = 14
                    };
                    nameText.SetResourceReference(ForegroundProperty, "TextBrush");
                    nameText.SetResourceReference(BackgroundProperty, "CardBrush");

                    TextBlock roleText = new TextBlock
                    {
                        Margin = new Thickness(0, 3, 0, 0),
                        FontSize = 12
                    };
                    roleText.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");
                    roleText.SetResourceReference(BackgroundProperty, "CardBrush");
                    string role = GetAccountTypeDisplayText(employee.AccountType);
                    roleText.Text = role;

                    TextBlock emailText = new TextBlock
                    {
                        Text = employee.EmailAddress,
                        Margin = new Thickness(0, 2, 0, 0),
                        FontSize = 11
                    };
                    emailText.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");
                    emailText.SetResourceReference(BackgroundProperty, "CardBrush");

                    employeeInfo.Children.Add(nameText);
                    employeeInfo.Children.Add(roleText);
                    employeeInfo.Children.Add(emailText);

                    TextBlock userIcon = new TextBlock
                    {
                        Text = "👤",
                        FontSize = 20,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(10, 0, 0, 0)
                    };
                    userIcon.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");

                    Grid.SetColumn(employeeInfo, 0);
                    Grid.SetColumn(userIcon, 1);

                    employeeGrid.Children.Add(employeeInfo);
                    employeeGrid.Children.Add(userIcon);

                    employeeBorder.Child = employeeGrid;
                    EmployeesPanel.Children.Add(employeeBorder);
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.ShowLoadingError();

                TextBlock errorText = new TextBlock
                {
                    Margin = new Thickness(0, 10, 0, 10)
                };
                errorText.SetResourceReference(TextBlock.TextProperty, "NoEmployeesInformation");
                errorText.SetResourceReference(TextBlock.ForegroundProperty, "ErrorBrush");
                EmployeesPanel.Children.Add(errorText);
            }
        }

        private string GetAccountTypeDisplayText(AccountType type)
        {
            return type switch
            {
                AccountType.CLIENT => LocalizationService.GetString("ClientEnum"),
                AccountType.BEAUTICIAN => LocalizationService.GetString("BeauticianEnum"),
                AccountType.MANAGER => LocalizationService.GetString("ManagerEnum"),
                _ => type.ToString()
            };
        }


        private void PriceListButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PriceListWindow priceListWindow = new PriceListWindow(
                    _beautySalon.Name,
                    _beautySalon.PriceList);

                priceListWindow.Owner = this;
                priceListWindow.ShowDialog(); //TODO
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.ShowLoadingError();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
