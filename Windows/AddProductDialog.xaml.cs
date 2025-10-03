using System;
using System.Windows;
using HCI_A.Models;
using HCI_A.Dao;

namespace HCI_A.Windows
{
    public partial class AddProductDialog : Window
    {
        private Window _parentWindow;
        private int _priceListId;

        public AddProductDialog(Window parentWindow, int priceListId)
        {
            InitializeComponent();

            _parentWindow = parentWindow;
            _priceListId = priceListId;

            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = parentWindow;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                    string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
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


                bool isAvailable = AvailabilityComboBox.SelectedIndex == 0;
                bool result = ProductDao.AddProduct(
                    NameTextBox.Text,
                    DescriptionTextBox.Text,
                    price,
                    isAvailable,
                    _priceListId
                );

                if (result)
                {
                    CustomOkMessageBox.ShowMsgSuccessfulAddition();
                    Close();


                    if (_parentWindow is ManagerMainWindow managerWindow)
                    {
                        managerWindow.RefreshProductsTab();
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