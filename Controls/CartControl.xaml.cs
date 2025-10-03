using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HCI_A.Dao;
using HCI_A.Helpers;
using HCI_A.Models;
using HCI_A.Windows;

namespace HCI_A.Controls
{
    public partial class CartControl : UserControl
    {
        private readonly int _clientId;
        private List<Cart> _carts = new List<Cart>();

        public CartControl()
        {
            InitializeComponent();
        }

        public CartControl(int clientId)
        {
            InitializeComponent();
            _clientId = clientId;
            LoadCarts();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadCarts();
        }

        private void LoadCarts()
        {
            CartsPanel.Children.Clear();

            try
            {
                _carts = CartDao.GetCartsByClient(_clientId);
                _carts = _carts.Where(c => c.Items != null && c.Items.Count > 0).ToList();

                if (_carts.Any())
                {
                    var cartsHeader = CreateSectionHeader(LocalizationService.GetString("ActiveCarts"), string.Format(LocalizationService.GetString("CartsNum"), _carts.Count));
                    CartsPanel.Children.Add(cartsHeader);

                    foreach (var cart in _carts)
                    {
                        CartsPanel.Children.Add(CreateCartCard(cart));
                    }
                }
                else
                {
                    CartsPanel.Children.Add(CreateEmptyState());
                }

                UpdateCartSummary();
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.ShowLoadingError();
            }
        }

        private UIElement CreateSectionHeader(string title, string subtitle)
        {
            var border = new Border
            {
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(15, 10, 15, 10),
                Margin = new Thickness(0, 0, 0, 10)
            };
            border.SetResourceReference(Border.BackgroundProperty, "PrimaryLightBrush");

            var stack = new StackPanel();

            var titleBlock = new TextBlock
            {
                Text = title,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
            };
            titleBlock.SetResourceReference(TextBlock.ForegroundProperty, "TextOnPrimaryBrush");

            var subtitleBlock = new TextBlock
            {
                Text = subtitle,
                FontSize = 12,
            };
            subtitleBlock.SetResourceReference(ForegroundProperty, "SecondaryLightBrush2");

            stack.Children.Add(titleBlock);
            stack.Children.Add(subtitleBlock);
            border.Child = stack;

            return border;
        }

        private UIElement CreateCartCard(Cart cart)
        {
            var border = new Border
            {
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(12),
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(20)
            };
            border.SetResourceReference(BackgroundProperty, "CardBrush");
            border.SetResourceReference(BorderBrushProperty, "BorderBrush");

            var mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Header
            var headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var headerStack = new StackPanel { Orientation = Orientation.Horizontal };
            TextBlock cartIcon = new TextBlock
            {
                Text = "🛒",
                FontSize = 20,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0)
            };
            cartIcon.SetResourceReference(ForegroundProperty, "PrimaryLightBrush");
            headerStack.Children.Add(cartIcon);

            var titleStack = new StackPanel();
            TextBlock salonName = new TextBlock
            {
                Text = cart.SalonName,
                FontSize = 16,
                FontWeight = FontWeights.Bold
            };
            salonName.SetResourceReference(ForegroundProperty, "PrimaryBrush");
            titleStack.Children.Add(salonName);

            TextBlock cartInfo = new TextBlock
            {
                FontSize = 12,
                FontWeight = FontWeights.SemiBold
            };
            string message = string.Format(LocalizationService.GetString("CartDescription"), cart.Items.Count, GetCartTotal(cart));
            cartInfo.Text = message;
            cartInfo.SetResourceReference(ForegroundProperty, "PrimaryLightBrush");
            titleStack.Children.Add(cartInfo);

            headerStack.Children.Add(titleStack);
            Grid.SetColumn(headerStack, 0);
            headerGrid.Children.Add(headerStack);

            var viewCartBtn = new Button
            {
                Content = "👁",
                Margin = new Thickness(10, 0, 0, 0),
                Width=40
            };
            viewCartBtn.SetResourceReference(StyleProperty, "PrimaryIconButton");
            viewCartBtn.Click += (s, e) => ShowCartDetails(cart);
            Grid.SetColumn(viewCartBtn, 1);
            headerGrid.Children.Add(viewCartBtn);

            Grid.SetRow(headerGrid, 0);
            mainGrid.Children.Add(headerGrid);

            var itemsStack = new StackPanel { Margin = new Thickness(0, 15, 0, 0) };
            var itemsToShow = cart.Items.Take(3);

            foreach (var item in itemsToShow)
            {
                var itemBorder = new Border
                {
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(12, 8, 12, 8),
                    Margin = new Thickness(0, 2, 0, 2)
                };
                itemBorder.SetResourceReference(BackgroundProperty, "SurfaceBrush");

                var itemGrid = new Grid();
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var itemInfo = new StackPanel();

                TextBlock name = new TextBlock
                {
                    Text = item.ProductName,
                    FontWeight = FontWeights.SemiBold
                };
                name.SetResourceReference(ForegroundProperty, "PrimaryDarkBrush");
                itemInfo.Children.Add(name);

                TextBlock quantity = new TextBlock
                {
                    FontSize = 11
                };
                string quant = LocalizationService.GetString("Quantity") + " " + item.Quantity;
                quantity.Text = quant;
                quantity.SetResourceReference(ForegroundProperty, "PrimaryLightBrush");
                itemInfo.Children.Add(quantity);

                TextBlock priceText = new TextBlock
                {
                    Text = $"{item.Total:F2} KM",
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center
                };
                priceText.SetResourceReference(ForegroundProperty, "PrimaryDarkBrush");

                Grid.SetColumn(itemInfo, 0);
                Grid.SetColumn(priceText, 1);
                itemGrid.Children.Add(itemInfo);
                itemGrid.Children.Add(priceText);
                itemBorder.Child = itemGrid;
                itemsStack.Children.Add(itemBorder);
            }

            if (cart.Items.Count > 3)
            {
                var info = new TextBlock
                {
                    FontStyle = FontStyles.Italic,
                    Margin = new Thickness(12, 5, 0, 0)
                };
                string itemsNum = string.Format(LocalizationService.GetString("MoreItemsLabel"), (cart.Items.Count - 3));
                info.Text = itemsNum;
                info.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");
                itemsStack.Children.Add(info);
            }

            Grid.SetRow(itemsStack, 1);
            mainGrid.Children.Add(itemsStack);

            var buttonGrid = new Grid { Margin = new Thickness(0, 15, 0, 0) };
            buttonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            buttonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var totalText = new TextBlock
            {
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center
            };
            string total = string.Format(LocalizationService.GetString("TotalQuantity"), GetCartTotal(cart));
            totalText.Text = total;
            totalText.SetResourceReference(ForegroundProperty, "PrimaryBrush");

            var checkoutBtn = new Button
            {
                Padding = new Thickness(20, 8, 20, 8)
            };
            checkoutBtn.SetResourceReference(ContentProperty, "Checkout");
            checkoutBtn.SetResourceReference(StyleProperty, "PrimaryTextButton");
            checkoutBtn.Click += (s, e) => ShowCheckoutDialog(cart);

            Grid.SetColumn(totalText, 0);
            Grid.SetColumn(checkoutBtn, 1);
            buttonGrid.Children.Add(totalText);
            buttonGrid.Children.Add(checkoutBtn);

            Grid.SetRow(buttonGrid, 2);
            mainGrid.Children.Add(buttonGrid);

            border.Child = mainGrid;
            return border;
        }

        private UIElement CreateEmptyState()
        {
            var border = new Border
            {
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(40),
                Margin = new Thickness(0, 20, 0, 20)
            };
            border.SetResourceReference(BackgroundProperty, "SurfaceBrush");

            var stack = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            stack.Children.Add(new TextBlock
            {
                Text = "🛒",
                FontSize = 48,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 15)
            });

            var messageBlock = new TextBlock
            {
                FontSize = 16,
                FontStyle = FontStyles.Italic,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };
            messageBlock.SetResourceReference(TextBlock.TextProperty, "NoActiveCartsTitle"); //?
            messageBlock.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");
            stack.Children.Add(messageBlock);

            var hintBlock = new TextBlock
            {
                FontSize = 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0)
            };
            hintBlock.SetResourceReference(TextBlock.TextProperty, "NoActiveCartsMessage");
            hintBlock.SetResourceReference(ForegroundProperty, "TextHintBrush");
            stack.Children.Add(hintBlock);

            border.Child = stack;
            return border;
        }

        private void ShowCartDetails(Cart cart)
        {
            try
            {
                var cartWindow = new CartDetailsWindow(cart, _clientId);
                cartWindow.CartUpdated += (s, e) => LoadCarts();
                cartWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show(LocalizationService.GetString("ErrorOpeningCartDetails"), LocalizationService.GetString("Error"), "✖️");
            }
        }

        private void ShowCheckoutDialog(Cart cart)
        {
            try
            {
                var dialog = new OrderCheckoutWindow(cart);
                if (dialog.ShowDialog() == true)
                {
                    LoadCarts();
                    CustomOkMessageBox.Show(LocalizationService.GetString("OrderPlacedMessage"), LocalizationService.GetString("Success"), "👏🏼") ;
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show(LocalizationService.GetString("ErrorDuringCheckout"), LocalizationService.GetString("ValidationError"), "⚠");
            }
        }

        private void UpdateCartSummary()
        {
            try
            {
                var totalCarts = _carts.Count;
                var totalItems = _carts.Sum(c => c.Items?.Count ?? 0);
                var totalValue = _carts.Sum(c => GetCartTotal(c));

                //CartSummaryText.Text = totalCarts > 0
                //    ? $"{totalCarts} cart(s) • {totalItems} item(s) • {totalValue:F2} KM"
                //    : LocalizationService.GetString("NoActiveCarts");
                CartSummaryText.Text = totalCarts > 0
                ? string.Format(LocalizationService.GetString("CartsNum"), totalCarts) + " • " +
                string.Format(LocalizationService.GetString("CartDescription"), totalItems, totalValue) :
                    LocalizationService.GetString("NoActiveCarts");
            }
            catch (Exception ex)
            {
                CartSummaryText.Text = LocalizationService.GetString("ErrorLoadingSummary");
                System.Diagnostics.Debug.WriteLine($"UpdateCartSummary error: {ex.Message}");
            }
        }

        private decimal GetCartTotal(Cart cart)
        {
            decimal total = 0;
            foreach (var item in cart.Items)
            {
                total += item.Total;
            }
            return total;
        }

    }
}
