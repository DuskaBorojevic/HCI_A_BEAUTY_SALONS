using System;
using System.Windows;
using HCI_A.Models;
using HCI_A.Dao;
using Microsoft.VisualBasic;

namespace HCI_A.Windows
{
    public partial class AddServiceDialog : Window
    {
        private Window _parentWindow;
        private int _priceListId;

        public AddServiceDialog(Window parentWindow, int priceListId)
        {
            InitializeComponent();

            _parentWindow = parentWindow;
            _priceListId = priceListId;

            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = parentWindow;
        }

        private void TypesButton_Click(object sender, RoutedEventArgs e)
        {
            ServiceTypesDialog dialog = new ServiceTypesDialog(this);
            dialog.ShowDialog();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                    string.IsNullOrWhiteSpace(DurationTextBox.Text) ||
                    string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
                    string.IsNullOrWhiteSpace(TypeTextBox.Text))
                {
                    CustomOkMessageBox.Show((string)Application.Current.Resources["RequiredFieldsError"], (string)Application.Current.Resources["ValidationError"], "⚠");

                    return;
                }

                double price;
                if (!double.TryParse(PriceTextBox.Text, out price))
                {
                    CustomOkMessageBox.Show((string)Application.Current.Resources["InvalidPrice"], (string)Application.Current.Resources["ValidationError"], "⚠");
                    return;
                }

                TimeSpan time;
                if (!TimeSpan.TryParse(DurationTextBox.Text, out time))
                {
                    CustomOkMessageBox.Show((string)Application.Current.Resources["InvalidDuration"], (string)Application.Current.Resources["ValidationError"], "⚠");
                    return;
                }

                int serviceTypeId;
                if (!int.TryParse(TypeTextBox.Text, out serviceTypeId))
                {
                    CustomOkMessageBox.Show((string)Application.Current.Resources["InvalidType"], (string)Application.Current.Resources["ValidationError"], "⚠");
                    return;
                }

                bool result = ServiceDao.AddService(
                    NameTextBox.Text,
                    price,
                    time,
                    DescriptionTextBox.Text,
                    _priceListId,
                    serviceTypeId
                );

                if (result)
                {
                    CustomOkMessageBox.ShowMsgSuccessfulAddition();
                    Close();

                    if (_parentWindow is ManagerMainWindow managerWindow)
                    {
                        managerWindow.RefreshServicesTab();
                    }
                }
                else
                {
                    CustomOkMessageBox.ShowMsgAdditionError();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}