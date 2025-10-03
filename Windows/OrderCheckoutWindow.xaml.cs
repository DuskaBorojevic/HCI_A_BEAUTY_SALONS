using HCI_A.Dao;
using HCI_A.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HCI_A.Windows
{
    public partial class OrderCheckoutWindow : Window
    {
        private readonly Cart _cart;

        public OrderCheckoutWindow(Cart cart)
        {
            InitializeComponent();
            _cart = cart;
            LoadOrderSummary();
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

        private void LoadOrderSummary()
        {
            decimal total = 0;
            foreach(var item in _cart.Items)
            {
                total += item.Total;
            }
            SalonNameText.Text = _cart.SalonName;
            TotalAmountText.Text = $"{total:F2} KM";

            OrderItemsPanel.Children.Clear();

            foreach (var item in _cart.Items)
            {
                var itemGrid = new Grid { Margin = new Thickness(0, 0, 2, 0) };
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var nameText = new TextBlock
                {
                    Text = item.ProductName,
                    VerticalAlignment = VerticalAlignment.Center
                };
                nameText.SetResourceReference(ForegroundProperty, "PrimaryDarkBrush");

                var quantityText = new TextBlock
                {
                    Text = $"x{item.Quantity}",
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 0, 0)
                };
                quantityText.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");

                var priceText = new TextBlock
                {
                    Text = $"{item.Total:F2} KM",
                    FontWeight = FontWeights.SemiBold,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(40, 0, 0, 0)
                };
                priceText.SetResourceReference(ForegroundProperty, "PrimaryDarkBrush");

                Grid.SetColumn(nameText, 0);
                Grid.SetColumn(quantityText, 1);
                Grid.SetColumn(priceText, 2);

                itemGrid.Children.Add(nameText);
                itemGrid.Children.Add(quantityText);
                itemGrid.Children.Add(priceText);

                OrderItemsPanel.Children.Add(itemGrid);
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddressBox.Text))
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["EnterAddress"],
                    (string)Application.Current.Resources["Error"], "⚠");
                AddressBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(PhoneBox.Text))
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["EnterPhone"],
                     (string)Application.Current.Resources["Error"], "⚠");
                PhoneBox.Focus();
                return;
            }

            try
            {
                var orderId = OrderDao.CheckoutCart(_cart.CartId, AddressBox.Text.Trim(), PhoneBox.Text.Trim());

                if (orderId > 0)
                {
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    CustomOkMessageBox.Show((string)Application.Current.Resources["OrderFailed"],
                     (string)Application.Current.Resources["Error"], "⚠");
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["OrderError"],
                     (string)Application.Current.Resources["Error"], "⚠");
            }
        }
    }
}
