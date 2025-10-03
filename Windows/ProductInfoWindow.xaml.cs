using System;
using System.Windows;
using HCI_A.Models;
using HCI_A.Dao;
using HCI_A.Models.Enums;
using System.Windows.Controls;

namespace HCI_A.Windows
{
    public partial class ProductInfoWindow : Window, IRefreshable
    {
        private Product _product;
        private User _user;
        private bool _isEditing = false;

        public ProductInfoWindow(Product product, User user)
        {
            InitializeComponent();

            _product = product;
            _user = user;

            LoadProductData();

            if (_user.AccountType == AccountType.CLIENT)
            {
                DeleteButton.Visibility = Visibility.Collapsed;
                EditSubmitButton.Visibility = Visibility.Collapsed;
            }

            if (_user.AccountType == AccountType.BEAUTICIAN)
                DeleteButton.IsEnabled = false;

            SetEditMode(false);
        }

        private void LoadProductData()
        {
            ProductNameText.Text = _product.Name;
            //IdText.Text = _product.ProductId.ToString();
            NameTextBox.Text = _product.Name;
            PriceTextBox.Text = _product.Price.ToString("F2");
            DescriptionTextBox.Text = _product.Description;
            AvailabilityComboBox.SelectedIndex = _product.Availability ? 0 : 1;
        }

        private void SetEditMode(bool isEditing)
        {
            _isEditing = isEditing;

            NameTextBox.IsReadOnly = !isEditing;
            PriceTextBox.IsReadOnly = !isEditing;
            DescriptionTextBox.IsReadOnly = !isEditing;
            AvailabilityComboBox.IsEnabled = isEditing;

            //IdText.IsEnabled = false;

            EditSubmitButton.SetResourceReference(Button.ContentProperty, isEditing ? "BtnSave" : "BtnEdit");
            EditSubmitButton.SetResourceReference(Button.StyleProperty, isEditing ? "SuccessTextButton" : "PrimaryTextButton");

            NameTextBox.SetResourceReference(TextBox.BackgroundProperty, isEditing ? "ElevatedSurfaceBrush" : "CardBrush");
            PriceTextBox.SetResourceReference(TextBox.BackgroundProperty, isEditing ? "ElevatedSurfaceBrush" : "CardBrush");
            DescriptionTextBox.SetResourceReference(TextBox.BackgroundProperty, isEditing ? "ElevatedSurfaceBrush" : "CardBrush");
            AvailabilityComboBox.SetResourceReference(ComboBox.BackgroundProperty, isEditing ? "ElevatedSurfaceBrush" : "CardBrush");
        }

        private void EditSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isEditing)
            {
                SetEditMode(true);
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(PriceTextBox.Text))
                {
                    CustomOkMessageBox.Show((string)Application.Current.Resources["ProductRequiredFieldsError"], (string)Application.Current.Resources["ValidationError"], "⚠");
                    return;
                }

                if (!double.TryParse(PriceTextBox.Text, out double price))
                {
                    CustomOkMessageBox.Show((string)Application.Current.Resources["InvalidPrice"], (string)Application.Current.Resources["ValidationError"], "⚠");
                    return;
                }

                Employee employee= null;
                employee = EmployeeDao.GetEmployeeById(_user.UserId);

                bool availability = AvailabilityComboBox.SelectedIndex == 0;
                int priceListId = BeautySalonDao.GetBeautySalonById(employee.BeautySalonId).PriceList.PriceListId;

                bool updated = ProductDao.UpdateProduct(
                    _product.ProductId,
                    NameTextBox.Text,
                    DescriptionTextBox.Text,
                    price,
                    availability,
                    priceListId
                );

                if (updated)
                {
                    _product.Name = NameTextBox.Text;
                    _product.Description = DescriptionTextBox.Text;
                    _product.Price = price;
                    _product.Availability = availability;

                    CustomOkMessageBox.ShowMsgSuccessUpdate();

                    SetEditMode(false);
                    RefreshData();
                }
                else
                {
                    CustomOkMessageBox.ShowMsgErrorUpdate();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error {ex.Message}");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool confirmed = CustomYesNoMessageBox.Show((string)Application.Current.Resources["ConfirmDelete"]);

            if (confirmed)
            {
                if (ProductDao.DeleteProduct(_product.ProductId))
                {
                    CustomOkMessageBox.ShowDeletedSuccess();

                    RefreshData();
                    Close();
                }
                else
                {
                    CustomOkMessageBox.ShowDeleteFailed();
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void RefreshData()
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w is ManagerMainWindow manager)
                {
                    manager.RefreshProductsTab();
                }
            }
        }
    }
}
