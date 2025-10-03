using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HCI_A.Dao;
using HCI_A.Helpers;
using HCI_A.Models;

namespace HCI_A.Windows
{
    public partial class CartDetailsWindow : Window
    {
        private readonly Cart _cart;
        private readonly int _clientId;

        public event EventHandler CartUpdated;

        public CartDetailsWindow(Cart cart, int clientId)
        {
            InitializeComponent();
            _cart = cart;
            _clientId = clientId;
            LoadCartDetails();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadCartDetails()
        {
            SalonNameText.SetResourceReference(TextBlock.TextProperty, "ShoppingCart");

            if (!string.IsNullOrEmpty(_cart?.SalonName))
            {
                SalonNameText.Text = $"{_cart.SalonName} - {SalonNameText.Text}";
            }

            UpdateSummary();
            LoadItems();
        }

        private void LoadItems()
        {
            ItemsPanel.Children.Clear();

            if (!_cart.Items.Any())
            {
                var emptyState = new Border
                {
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(40),
                    Margin = new Thickness(0, 0, 20, 0)
                };
                emptyState.SetResourceReference(BackgroundProperty, "SurfaceBrush");
                var stack = new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                stack.Children.Add(new TextBlock
                {
                    Text = "🛒",
                    FontSize = 48,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 15)
                });

                TextBlock emptyCart= new TextBlock
                {
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                emptyCart.SetResourceReference(TextBlock.TextProperty, "EmptyCart");
                emptyCart.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");
                
                stack.Children.Add(emptyCart);

                emptyState.Child = stack;
                ItemsPanel.Children.Add(emptyState);
                return;
            }

            foreach (var item in _cart.Items)
            {
                //Product product = ProductDao.GetProductById(item.ProductId);
                //if(product.Availability)
                ItemsPanel.Children.Add(CreateItemCard(item));
            }
        }

        private UIElement CreateItemCard(OrderItem item)
        {
            var border = new Border
            {
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(15)
            };
            border.SetResourceReference(BackgroundProperty, "CardBrush");
            border.SetResourceReference(BorderBrushProperty, "BorderBrush");

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // --- Product Info ---
            var infoStack = new StackPanel();

            var infoTB = new TextBlock
            {
                Text = item.ProductName,
                FontSize = 16,
                FontWeight = FontWeights.Bold
            };
            infoTB.SetResourceReference(ForegroundProperty, "PrimaryDarkBrush");
            infoStack.Children.Add(infoTB);

            if (!string.IsNullOrEmpty(item.ProductDescription))
            {
                var descTB = new TextBlock
                {
                    Text = item.ProductDescription,
                    FontSize = 12,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 2, 0, 2)
                };
                descTB.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");
                infoStack.Children.Add(descTB);
            }

            var priceTB = new TextBlock
            {
                //Text = $"Unit Price: {item.Price:F2} KM",
                FontSize = 12,
                Margin = new Thickness(0, 5, 0, 5)
            };
            string unitPriceText = string.Format(
            (string)Application.Current.Resources["UnitPrice"],
            item.Price);
            priceTB.Text=unitPriceText;
            priceTB.SetResourceReference(ForegroundProperty, "PrimaryBrush");
            infoStack.Children.Add(priceTB);

            Grid.SetColumn(infoStack, 0);
            grid.Children.Add(infoStack);

            var rightGrid = new Grid
            {
                Margin = new Thickness(20, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Price
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Quantity + Remove

            var totalPrice = new TextBlock
            {
                Text = $"{item.Total:F2} KM",
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            totalPrice.SetResourceReference(ForegroundProperty, "PrimaryBrush");
            Grid.SetRow(totalPrice, 0);
            rightGrid.Children.Add(totalPrice);

            var bottomControls = new Grid { Margin = new Thickness(0, 8, 0, 0) };
            bottomControls.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            bottomControls.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var quantityStack = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };

            var decreaseBtn = new Button();
            decreaseBtn.SetResourceReference(StyleProperty, "DecreaseButton");
            decreaseBtn.Click += (s, e) => UpdateQuantity(item, item.Quantity - 1);

            var quantityText = new TextBlock
            {
                Text = item.Quantity.ToString(),
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                MinWidth = 40,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5, 0, 5, 0)
            };
            quantityText.SetResourceReference(ForegroundProperty, "PrimaryDarkBrush");

            var increaseBtn = new Button();
            increaseBtn.SetResourceReference(StyleProperty, "IncreaseButton");
            increaseBtn.Click += (s, e) => UpdateQuantity(item, item.Quantity + 1);

            quantityStack.Children.Add(decreaseBtn);
            quantityStack.Children.Add(quantityText);
            quantityStack.Children.Add(increaseBtn);

            Grid.SetColumn(quantityStack, 0);
            bottomControls.Children.Add(quantityStack);


            var removeBtn = new Button();

            removeBtn.SetResourceReference(StyleProperty, "DeleteButton");
            removeBtn.Click += (s, e) => RemoveItem(item);

            Grid.SetColumn(removeBtn, 1);
            bottomControls.Children.Add(removeBtn);

            Grid.SetRow(bottomControls, 1);
            rightGrid.Children.Add(bottomControls);

            Grid.SetColumn(rightGrid, 2);
            grid.Children.Add(rightGrid);

            border.Child = grid;
            return border;
        }


        private void UpdateQuantity(OrderItem item, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                RemoveItem(item);
                return;
            }

            if (CartDao.UpdateCartItemQuantity(item.ItemId, newQuantity))
            {
                item.Quantity = newQuantity;
                LoadCartDetails();
                CartUpdated?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["FailedToUpdateQuantity"], (string)Application.Current.Resources["ValidationError"], "⚠");
            }
        }

        private void RemoveItem(OrderItem item)
        {
            string message = string.Format(LocalizationService.GetString("ConfirmRemoveFromCart"), item.ProductName);
            bool confirmed = CustomYesNoMessageBox.Show(message);

            if (confirmed)
            {
                if (CartDao.RemoveFromCart(item.ItemId))
                {
                    _cart.Items.Remove(item);
                    LoadCartDetails();
                    CartUpdated?.Invoke(this, EventArgs.Empty);

                    if (!_cart.Items.Any())
                    {
                        CustomOkMessageBox.Show((string)Application.Current.Resources["EmptyCart"],
                    (string)Application.Current.Resources["Info"]);
                    }
                }
                else
                {
                    CustomOkMessageBox.Show((string)Application.Current.Resources["FailedToRemoveItem"], (string)Application.Current.Resources["Error"], "✖️");
                }
            }
        }

        private void ClearCart_Click(object sender, RoutedEventArgs e)
        {

            bool confirmed = CustomYesNoMessageBox.Show("❓", (string)Application.Current.Resources["ConfirmDelete"]);


            if (confirmed)
            {
                var itemsToRemove = _cart.Items.ToList();
                bool allRemoved = true;

                foreach (var item in itemsToRemove)
                {
                    if (!CartDao.RemoveFromCart(item.ItemId))
                    {
                        allRemoved = false;
                    }
                }

                if (allRemoved)
                {
                    _cart.Items.Clear();
                    LoadCartDetails();
                    CartUpdated?.Invoke(this, EventArgs.Empty);
                        CustomOkMessageBox.Show((string)Application.Current.Resources["CartCleared"],
                     (string)Application.Current.Resources["Success"], "⚠");
                }
                else
                {
                    CustomOkMessageBox.Show((string)Application.Current.Resources["PartialRemove"],
                    (string)Application.Current.Resources["ValidationError"], "⚠");
                }
            }
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            if (!_cart.Items.Any())
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["EmptyCart"],
                    (string)Application.Current.Resources["ValidationError"], "⚠");
                return;
            }

            var checkoutWindow = new OrderCheckoutWindow(_cart);
            if (checkoutWindow.ShowDialog() == true)
            {
                CartUpdated?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
        }

        private void UpdateSummary()
        {
            var totalItems = _cart.Items.Sum(i => i.Quantity);
            var totalAmount = _cart.Items.Sum(i => i.Total);

            string cartSummaryText = string.Format(
            (string)Application.Current.Resources["CartSummary"],
            totalItems, totalAmount);
            CartSummaryText.Text = cartSummaryText;

            string totalItemsText = string.Format(
            (string)Application.Current.Resources["TotalItems"],
            totalItems);
            TotalItemsText.Text = totalItemsText;

            TotalAmountText.Text = $"{totalAmount:F2} KM";
            
            //CartSummaryText.Text = $"{totalItems} item(s) • {totalAmount:F2} KM";
            //TotalItemsText.Text = $"{totalItems} item(s)";
            //TotalAmountText.Text = $"{totalAmount:F2} KM";
        }
    }
}
