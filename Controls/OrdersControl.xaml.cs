using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using HCI_A.Dao;
using HCI_A.Helpers;
using HCI_A.Models;
using HCI_A.Models.Enums;
using HCI_A.Windows;

namespace HCI_A.Controls
{
    public partial class OrdersControl : UserControl
    {
        private readonly int _userId;
        private readonly int _salonId;
        private readonly bool _isEmployee;
        private List<Order> _orders = new List<Order>();

        public OrdersControl()
        {
            InitializeComponent();
        }

        public OrdersControl(int userId, int salonId, bool isEmployee)
        {
            InitializeComponent();
            _userId = userId;
            _salonId = salonId;
            _isEmployee = isEmployee;
            LoadOrders();
            FillStatusFilterCombo();
            StatusFilterCombo.SelectedIndex = 0;
            LocalizationService.OnLanguageChanged += LocalizationService_OnLanguageChanged;
        }



        private void LoadOrders()
        {
            OrdersPanel.Children.Clear();

            if (_isEmployee && _salonId >= 0)
            {
                LoadEmployeeView();
            }
            else
            {
                LoadClientView();
            }

            UpdateOrderSummary();
        }

        private void LoadClientView()
        {
            try
            {
                _orders = OrderDao.GetOrdersForClient(_userId);

                foreach (var order in _orders)
                {
                    order.Items = OrderDao.GetOrderItems(order.OrderId);
                }

                if (_orders.Any())
                {
                    string count = _orders.Count + " " + LocalizationService.GetString("OrdersNum");

                    var ordersHeader = CreateSectionHeader(LocalizationService.GetString("OrdersHistory"), count);
                    OrdersPanel.Children.Add(ordersHeader);

                    foreach (var order in _orders.OrderByDescending(o => o.Date))
                    {
                        OrdersPanel.Children.Add(CreateOrderCard(order, false));
                    }
                }
                else
                {
                    OrdersPanel.Children.Add(CreateEmptyState(LocalizationService.GetString("NoOrdersYet"), LocalizationService.GetString("EmptyOrderHistory")));
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.ShowLoadingError();
            }
        }

        private void LoadEmployeeView()
        {
            try
            {
                _orders = OrderDao.GetOrdersForSalon(_salonId);

                foreach (var order in _orders)
                {
                    order.Items = OrderDao.GetOrderItems(order.OrderId);
                }

                if (_orders.Any())
                {
                    string count = _orders.Count + " " + LocalizationService.GetString("OrdersNum");
                    var ordersHeader = CreateSectionHeader(LocalizationService.GetString("SalonOrders"), count);
                    OrdersPanel.Children.Add(ordersHeader);

                    foreach (var order in _orders.OrderByDescending(o => o.Date))
                    {
                        OrdersPanel.Children.Add(CreateOrderCard(order, true));
                    }
                }
                else
                {
                    OrdersPanel.Children.Add(CreateEmptyState(LocalizationService.GetString("NoOrders"), LocalizationService.GetString("NoSalonOrders")));
                }
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

        private UIElement CreateOrderCard(Order order, bool employeeView)
        {
            var border = new Border
            {
                BorderBrush = GetStatusBrush(order.Status),
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(12),
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(20)
            };
            border.SetResourceReference(BackgroundProperty, "CardBrush");

            var expander = new Expander
            {
                IsExpanded = false,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0)
            };

            var headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var headerStack = new StackPanel();
            TextBlock orderName = new TextBlock
            {
                FontSize = 16,
                FontWeight = FontWeights.Bold
            };
            orderName.Text=string.Format(LocalizationService.GetString("OrderId"), order.OrderId);
            orderName.SetResourceReference(ForegroundProperty, "PrimaryDarkBrush");
            headerStack.Children.Add(orderName);

            if (!_isEmployee)
            {
                TextBlock salonNameText = new TextBlock
                {
                    Text = BeautySalonDao.GetBeautySalonById(order.BeautySalonId).Name,
                    FontSize = 12,
                    FontStyle = FontStyles.Italic,
                    Margin = new Thickness(0, 2, 0, 2)
                };
                salonNameText.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");
                headerStack.Children.Add(salonNameText);
            }

            var infoText = $"{order.Date:MMM dd, yyyy} • {GetStatusDisplayText(order.Status)} • {order.Total:F2} KM";

            TextBlock infoBlock = new TextBlock
            {
                Text = infoText,
                FontSize = 12
            };
            infoBlock.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");
            headerStack.Children.Add(infoBlock);

            var statusBadge = new Border
            {
                Background = GetStatusBrush(order.Status),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(8, 4, 8, 4),
                Margin = new Thickness(10, 0, 0, 0)
            };
            TextBlock statusBadgeText = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 11,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White
            };
            statusBadgeText.Text = GetStatusDisplayText(order.Status);
            statusBadge.Child = statusBadgeText;

            Grid.SetColumn(headerStack, 0);
            Grid.SetColumn(statusBadge, 1);
            headerGrid.Children.Add(headerStack);
            headerGrid.Children.Add(statusBadge);

            expander.Header = headerGrid;

            var contentStack = new StackPanel { Margin = new Thickness(0, 15, 0, 0) };


            if (order.Items != null && order.Items.Any())
            {
                foreach (var item in order.Items)
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
                    TextBlock itemName = new TextBlock
                    {
                        Text = item.ProductName,
                        FontWeight = FontWeights.SemiBold,
                    };
                    itemName.SetResourceReference(ForegroundProperty, "PrimaryDarkBrush");
                    itemInfo.Children.Add(itemName);

                    TextBlock itemQuantity = new TextBlock
                    {
                        FontSize = 11
                    };
                    string mess = string.Format(LocalizationService.GetString("Quantity") + item.Quantity + " • " + string.Format(LocalizationService.GetString("UnitPrice"), item.Price));
                    itemQuantity.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");
                    itemInfo.Children.Add(itemQuantity);

                    var priceText = new TextBlock
                    {
                        Text = $"{item.Total:F2} KM",
                        FontWeight = FontWeights.Bold,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    priceText.SetResourceReference(ForegroundProperty, "PrimaryBrush");

                    Grid.SetColumn(itemInfo, 0);
                    Grid.SetColumn(priceText, 1);
                    itemGrid.Children.Add(itemInfo);
                    itemGrid.Children.Add(priceText);
                    itemBorder.Child = itemGrid;
                    contentStack.Children.Add(itemBorder);
                }
            }

            var totalBorder = new Border
            {
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(12, 8, 12, 8),
                Margin = new Thickness(0, 10, 0, 0)
            };
            totalBorder.SetResourceReference(BackgroundProperty, "SecondaryLightBrush");

            var totalGrid = new Grid();
            totalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            totalGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var totalLabel = new TextBlock
            {
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center
            };
            totalLabel.SetResourceReference(TextBlock.TextProperty, "TotalAmount");
            totalLabel.SetResourceReference(ForegroundProperty, "PrimaryDarkBrush");

            var totalAmount = new TextBlock
            {
                Text = $"{order.Total:F2} KM",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center
            };
            totalAmount.SetResourceReference(ForegroundProperty, "PrimaryDarkBrush");

            Grid.SetColumn(totalLabel, 0);
            Grid.SetColumn(totalAmount, 1);
            totalGrid.Children.Add(totalLabel);
            totalGrid.Children.Add(totalAmount);
            totalBorder.Child = totalGrid;
            contentStack.Children.Add(totalBorder);

            // Employee Controls
            if (employeeView && order.Status != OrderStatus.DELIVERED && order.Status != OrderStatus.CANCELLED) 
            {
                var controlsBorder = new Border
                {
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(15),
                    Margin = new Thickness(0, 15, 0, 0)
                };
                controlsBorder.SetResourceReference(BackgroundProperty, "PrimaryLightBrush");

                var controlsStack = new StackPanel();

                var headerText = new TextBlock
                {
                    FontSize = 14,
                    FontWeight = FontWeights.SemiBold,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                headerText.SetResourceReference(TextBlock.TextProperty, "UpdateOrderStatus"); 
                headerText.SetResourceReference(ForegroundProperty, "PrimaryDarkBrush");
                controlsStack.Children.Add(headerText);

                var statusGrid = new Grid();
                statusGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                statusGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                statusGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var statusLabel = new TextBlock
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Medium,
                    Margin = new Thickness(0, 0, 10, 0)
                };
                statusLabel.SetResourceReference(Label.ContentProperty, "NewStatus"); 
                statusLabel.SetResourceReference(ForegroundProperty, "TextPrimaryBrush");

                var statusCombo = new ComboBox
                {
                    ItemsSource = Enum.GetValues(typeof(OrderStatus)),
                    SelectedItem = order.Status,
                    MinWidth = 180,
                    Margin = new Thickness(0, 0, 15, 0),
                    Padding = new Thickness(10, 8, 10, 8)
                };

                //var statusItems = Enum.GetValues(typeof(OrderStatus))
                //      .Cast<OrderStatus>()
                //      .Select(s => GetStatusDisplayText(s))
                //      .ToList();

                //statusCombo.ItemsSource = statusItems;
                //statusCombo.FontSize = 10;

                //statusCombo.SelectedItem = GetStatusDisplayText(order.Status);


                var updateBtn = new Button
                {
                    Padding = new Thickness(20, 8, 20, 8),
                    FontWeight = FontWeights.SemiBold
                };
                updateBtn.SetResourceReference(StyleProperty, "SuccessTextButton");
                updateBtn.SetResourceReference(ContentProperty, "BtnUpdate");

                updateBtn.Click += (s, e) =>
                {
                    try
                    {
                        var newStatus = (OrderStatus)statusCombo.SelectedItem;

                        if (newStatus == OrderStatus.DELIVERED || newStatus == OrderStatus.CANCELLED)
                        {
                            string message = string.Format(LocalizationService.GetString("ConfirmMarkOrder"), GetStatusDisplayText(newStatus));
                            bool confirmed = CustomYesNoMessageBox.Show("❓", message);
                            if (!confirmed) return;
                        }

                        OrderDao.UpdateOrderStatus(order.OrderId, newStatus);
                        order.Status = newStatus;
                        LoadOrders();

                        var successMessage = newStatus switch
                        {
                            OrderStatus.IN_PROGRESS => LocalizationService.GetString("MarkedInProgress"),
                            OrderStatus.DELIVERED => LocalizationService.GetString("MarkedDelivered"),
                            OrderStatus.CANCELLED => LocalizationService.GetString("MarkedCancelled"),
                            _ => LocalizationService.GetString("StatusUpdatedSuccessfully"),
                        };

                        CustomOkMessageBox.Show(successMessage, LocalizationService.GetString("Success"), "👏🏼");
                    }
                    catch (Exception ex)
                    {
                        CustomOkMessageBox.Show(LocalizationService.GetString("FailedToUpdateStatus"), LocalizationService.GetString("ValidationError"), "⚠");
                    }
                };

                var currentStatusBorder = new Border
                {
                    Background = GetStatusBrush(order.Status),
                    CornerRadius = new CornerRadius(4),
                    Padding = new Thickness(8, 4, 8, 4),
                    Margin = new Thickness(0, 10, 0, 0)
                };

                var currentStatusText = new TextBlock
                {
                    FontSize = 11,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                string statusL = LocalizationService.GetString("Current");
                string statusText = GetStatusDisplayText(order.Status);
                currentStatusText.Text = statusL + " " + statusText;
                currentStatusBorder.Child = currentStatusText;

                Grid.SetColumn(statusLabel, 0);
                Grid.SetColumn(statusCombo, 1);
                Grid.SetColumn(updateBtn, 2);
                statusGrid.Children.Add(statusLabel);
                statusGrid.Children.Add(statusCombo);
                statusGrid.Children.Add(updateBtn);

                controlsStack.Children.Add(statusGrid);
                controlsStack.Children.Add(currentStatusBorder);
                controlsBorder.Child = controlsStack;
                contentStack.Children.Add(controlsBorder);
            }

            if (!employeeView && order.Status == OrderStatus.PENDING)
            {
                var cancelBtn = new Button
                {
                    Padding = new Thickness(15, 6, 15, 6),
                    Margin = new Thickness(0, 15, 0, 0),
                    FontWeight = FontWeights.SemiBold,
                    HorizontalAlignment = HorizontalAlignment.Right
                };
                cancelBtn.SetResourceReference(ContentProperty, "BtnCancelOrder");
                cancelBtn.SetResourceReference(StyleProperty, "ErrorTextButton");

                cancelBtn.Click += (s, e) =>
                {
                    bool confirmed = CustomYesNoMessageBox.Show("❓",
                        LocalizationService.GetString("ConfirmCancelOrder"));
                    if (!confirmed) return;

                    try
                    {
                        OrderDao.UpdateOrderStatus(order.OrderId, OrderStatus.CANCELLED); 
                        LoadOrders();

                        CustomOkMessageBox.Show(
                            LocalizationService.GetString("OrderCancelled"),
                            LocalizationService.GetString("Success"),
                            "✓"
                        );
                    }
                    catch (Exception ex)
                    {
                        CustomOkMessageBox.Show(
                            LocalizationService.GetString("FailedToCancelOrder"),
                            LocalizationService.GetString("ValidationError"),
                            "⚠"
                        );
                    }
                };

                contentStack.Children.Add(cancelBtn);
            }

            expander.Content = contentStack;
            border.Child = expander;
            return border;
        }

        private UIElement CreateEmptyState(string title, string message)
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
                Text = "📦",
                FontSize = 48,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 15)
            });

            var titleBlock = new TextBlock
            {
                Text = title,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };
            titleBlock.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");
            stack.Children.Add(titleBlock);

            var messageBlock = new TextBlock
            {
                Text = message,
                FontSize = 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0)
            };
            messageBlock.SetResourceReference(ForegroundProperty, "TextHintBrush");
            stack.Children.Add(messageBlock);

            border.Child = stack;
            return border;
        }

        private void UpdateOrderSummary()
        {
            try
            {
                if (_isEmployee)
                {
                    string count = string.Format(LocalizationService.GetString("SalonOrdersCount"), _orders.Count);
                    OrderSummaryText.Text = count;
                }
                else
                {
                    string message = string.Format(LocalizationService.GetString("NumPlacedOrders"), _orders.Count);
                    OrderSummaryText.Text = _orders.Count > 0
                        ? message
                        : LocalizationService.GetString("NoOrdersYet");
                }
            }
            catch (Exception ex)
            {
     
                System.Diagnostics.Debug.WriteLine($"UpdateOrderSummary error: {ex.Message}");
            }
        }

        private string GetStatusDisplayText(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.PENDING => LocalizationService.GetString("PendingOrderEnum"),
                OrderStatus.IN_PROGRESS => LocalizationService.GetString("InProgressOrderEnum"),
                OrderStatus.DELIVERED => LocalizationService.GetString("DeliveredOrderEnum"),
                OrderStatus.CANCELLED => LocalizationService.GetString("CancelledOrderEnum"),
                _ => status.ToString()
            };
        }

        private Brush GetStatusBrush(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.PENDING => new SolidColorBrush(Color.FromRgb(255, 193, 7)),
                OrderStatus.IN_PROGRESS => new SolidColorBrush(Color.FromRgb(0, 123, 255)),
                OrderStatus.DELIVERED => new SolidColorBrush(Color.FromRgb(40, 167, 69)),
                OrderStatus.CANCELLED => new SolidColorBrush(Color.FromRgb(220, 53, 69)),
                _ => (Brush)Application.Current.Resources["BorderBrush"]
            };
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
            StatusFilterCombo.SelectedIndex = 0;
        }

        public void RefreshData()
        {
            LoadOrders();
            FillStatusFilterCombo();
            StatusFilterCombo.SelectedIndex = 0;
        }

        private void StatusFilter_Changed(object sender, SelectionChangedEventArgs e)
        {
            ApplyStatusFilter();
        }

        private void ApplyStatusFilter()
        {
            if (_orders == null) return;
            if (StatusFilterCombo.SelectedItem == null) return;

            List<Order> filteredOrders;

            if (StatusFilterCombo.SelectedIndex == 0)
            {
                filteredOrders = _orders;
            }
            else
            {
                string selected = StatusFilterCombo.SelectedItem.ToString();
                filteredOrders = _orders
                    .Where(o => GetStatusDisplayText(o.Status) == selected)
                    .ToList();
            }
           

            OrdersPanel.Children.Clear();

            if (filteredOrders.Any())
            {
                string mess = filteredOrders.Count + " " + LocalizationService.GetString("OrdersNum");
                var ordersHeader = CreateSectionHeader(LocalizationService.GetString("FilteredOrders"), mess);
                OrdersPanel.Children.Add(ordersHeader);

                foreach (var order in filteredOrders.OrderByDescending(o => o.Date))
                {
                    OrdersPanel.Children.Add(CreateOrderCard(order, _isEmployee));
                }
            }
            else
            {

                OrdersPanel.Children.Add(CreateEmptyState(LocalizationService.GetString("NoMatchingOrders"), LocalizationService.GetString("ChangingFilter")));
            }

            OrderSummaryText.Text = $"{filteredOrders.Count}" + " " + LocalizationService.GetString("OrdersNum") + " " + LocalizationService.GetString("Shown");
        }

        private void FillStatusFilterCombo()
        {
            var statusItems = new List<string>
            {
                (string)Application.Current.Resources["AllStatuses"]
            };

            statusItems.AddRange(Enum.GetValues(typeof(OrderStatus))
                                     .Cast<OrderStatus>()
                                     .Select(s => GetStatusDisplayText(s)));

            StatusFilterCombo.ItemsSource = statusItems;
            StatusFilterCombo.FontSize = 10;

        }

        private void LocalizationService_OnLanguageChanged()
        {
            RefreshData();
            
        }
    }
}
