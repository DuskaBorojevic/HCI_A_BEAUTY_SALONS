using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using HCI_A.Helpers;
using HCI_A.Models;

namespace HCI_A.Windows
{
    public partial class ProductQuantityDialog : Window
    {
        private readonly Product _product;
        private int _quantity = 1;

        public int SelectedQuantity => _quantity;

        public ProductQuantityDialog(Product product)
        {
            InitializeComponent();
            _product = product;
            LoadProductInfo();
            UpdateTotal();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void LoadProductInfo()
        {
            ProductNameText.Text = _product.Name;
            //ProductPriceText.Text = $"{_product.Price:F2} KM per unit";
            ProductPriceText.Text = $"{_product.Price:F2} KM";
        }

        private void DecreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_quantity > 1)
            {
                _quantity--;
                QuantityTextBox.Text = _quantity.ToString();
                UpdateTotal();
            }
        }

        private void IncreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_quantity < 99) 
            {
                _quantity++;
                QuantityTextBox.Text = _quantity.ToString();
                UpdateTotal();
            }
        }

        private void QuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void QuantityTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (int.TryParse(QuantityTextBox.Text, out int newQuantity) && newQuantity > 0 && newQuantity <= 99)
            {
                _quantity = newQuantity;
                UpdateTotal();
            }
            else if (string.IsNullOrEmpty(QuantityTextBox.Text))
            {
                _quantity = 1;
                UpdateTotal();
            }
        }

        private void UpdateTotal()
        {
            var total = _product.Price * _quantity;
            string message = string.Format(LocalizationService.GetString("TotalQuantity"), total);
            TotalPriceText.Text = message;
            //TotalPriceText.Text = $"Total: {total:F2} KM";
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (_quantity > 0)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["MsgSelectQuantity"], (string)Application.Current.Resources["Error"], "⚠");
            }
        }
    }
}
