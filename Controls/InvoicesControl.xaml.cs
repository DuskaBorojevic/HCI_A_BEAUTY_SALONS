using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HCI_A.Dao;
using HCI_A.Helpers;
using HCI_A.Models;
using HCI_A.Models.Enums;
using HCI_A.Windows;
using Mysqlx.Crud;

namespace HCI_A.Controls
{
    public partial class InvoicesControl : UserControl, IRefreshable
    {
        private int _clientId;
        private int? _salonId;
        private bool _isManagerView;
        private List<Invoice> _allInvoices;


        public InvoicesControl() { }
        public InvoicesControl(int clientId = 0, int? salonId = null, bool isManagerView = false)
        {
            InitializeComponent();
            _clientId = clientId;
            _salonId = salonId;
            _isManagerView = isManagerView;

            LocalizationService.OnLanguageChanged += LocalizationService_OnLanguageChanged;
            InitializeUI();
            LoadInvoices();
        }

        private void InitializeUI()
        {
            if (_isManagerView)
            {
                HeaderTitle.SetResourceReference(TextBlock.TextProperty, "SalonInvoices");
                HeaderSubtitle.SetResourceReference(TextBlock.TextProperty, "InvoiceManagementManager");
            }
            else
            {
                HeaderTitle.SetResourceReference(TextBlock.TextProperty, "ClientInvoices");
                HeaderSubtitle.SetResourceReference(TextBlock.TextProperty, "InvoicesViewClient");
            }

            FillStatusFilterCombo();
            StatusFilterCombo.SelectedIndex = 0;

        }

        private async void LoadInvoices()
        {
            try
            {
                ShowLoading(true);

                await System.Threading.Tasks.Task.Run(() =>
                {
                    if (_isManagerView && _salonId.HasValue)
                    {
                        _allInvoices = InvoiceDao.GetInvoicesForSalon(_salonId.Value);
                    }
                    else if (_clientId > 0)
                    {
                        _allInvoices = InvoiceDao.GetInvoicesForClient(_clientId);
                    }
                    else
                    {
                        _allInvoices = new List<Invoice>();
                    }
                });

                Dispatcher.Invoke(() =>
                {
                    ShowLoading(false);
                    DisplayInvoices(_allInvoices);
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    ShowLoading(false);
                    System.Diagnostics.Debug.WriteLine($"Error loading invoices: {ex.Message}");
                });
            }
        }

        private void DisplayInvoices(List<Invoice> invoices)
        {
            InvoicesPanel.Children.Clear();

            if (invoices == null || !invoices.Any())
            {
                NoInvoicesGrid.Visibility = Visibility.Visible;
                InvoicesScrollViewer.Visibility = Visibility.Collapsed;
                return;
            }

            NoInvoicesGrid.Visibility = Visibility.Collapsed;
            InvoicesScrollViewer.Visibility = Visibility.Visible;

            var groupedInvoices = invoices
                .OrderByDescending(i => i.Date)
                .GroupBy(i => new { i.Date.Year, i.Date.Month })
                .OrderByDescending(g => new DateTime(g.Key.Year, g.Key.Month, 1));

            foreach (var monthGroup in groupedInvoices)
            {
                var monthHeader = new TextBlock
                {
                    Text = new DateTime(monthGroup.Key.Year, monthGroup.Key.Month, 1).ToString("MMMM yyyy"),
                    FontSize = 16,
                    FontWeight = FontWeights.SemiBold,
                    Margin = new Thickness(0, 20, 0, 10)
                };
                monthHeader.SetResourceReference(ForegroundProperty, "TextPrimaryBrush");

                if (InvoicesPanel.Children.Count > 0)
                    InvoicesPanel.Children.Add(monthHeader);

                foreach (var invoice in monthGroup.OrderByDescending(i => i.Date))
                {
                    var invoiceCard = CreateInvoiceCard(invoice);
                    InvoicesPanel.Children.Add(invoiceCard);
                }
            }
        }

        private Border CreateInvoiceCard(Invoice invoice)
        {
            var card = new Border
            {
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(20),
                Margin = new Thickness(0, 0, 0, 15)
            };
            card.SetResourceReference(BorderBrushProperty, "BorderBrush");
            card.SetResourceReference(BackgroundProperty, "SecondaryLightBrush2");

            var mainGrid = new Grid();
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var leftStack = new StackPanel();

            var headerStack = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 10) };

            TextBlock icon = new TextBlock
            {
                Text = "🧾",
                FontSize = 20,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0)
            };
            icon.SetResourceReference(ForegroundProperty, "PrimaryBrush");
            headerStack.Children.Add(icon);
           

            TextBlock header = new TextBlock
            {

                FontSize = 16,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center
            };
            string text = LocalizationService.GetString("Invoice") + " #" + invoice.OrderId;
            header.Text = text;

            header.SetResourceReference(ForegroundProperty, "PrimaryBrush");
            headerStack.Children.Add(header);

            leftStack.Children.Add(headerStack);

            var detailsGrid = new Grid { Margin = new Thickness(0, 0, 0, 15) };
            detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            detailsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            detailsGrid.RowDefinitions.Add(new RowDefinition());
            detailsGrid.RowDefinitions.Add(new RowDefinition());
            if (_isManagerView)
                detailsGrid.RowDefinitions.Add(new RowDefinition());

            var invoiceDateStack = new StackPanel { Margin = new Thickness(0, 0, 10, 5) };
            invoiceDateStack.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");
            TextBlock invoiceDate = new TextBlock
            {
                FontSize = 12,
                FontWeight = FontWeights.SemiBold
            };
            invoiceDate.SetResourceReference(TextBlock.TextProperty, "InvoiceDate");
            invoiceDateStack.Children.Add(invoiceDate);

            invoiceDateStack.Children.Add(new TextBlock
            {
                Text = invoice.Date.ToString("dd.MM.yyyy"),
                FontSize = 14
            });
            Grid.SetColumn(invoiceDateStack, 0);
            Grid.SetRow(invoiceDateStack, 0);
            detailsGrid.Children.Add(invoiceDateStack);

            var orderDateStack = new StackPanel { Margin = new Thickness(10, 0, 0, 5) };
            orderDateStack.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");

            TextBlock orderDate = new TextBlock
            {
                FontSize = 12,
                FontWeight = FontWeights.SemiBold
            };
            orderDate.SetResourceReference(TextBlock.TextProperty, "OrderDate");
            orderDateStack.Children.Add(orderDate);
            orderDateStack.Children.Add(new TextBlock
            {
                Text = invoice.OrderDate.ToString("dd.MM.yyyy"),
                FontSize = 14,
            });
            Grid.SetColumn(orderDateStack, 1);
            Grid.SetRow(orderDateStack, 0);
            detailsGrid.Children.Add(orderDateStack);


            var amountStack = new StackPanel { Margin = new Thickness(0, 0, 10, 5) };
            amountStack.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");
            TextBlock amount = new TextBlock
            {
                FontSize = 12,
                FontWeight = FontWeights.SemiBold
            };
            amount.SetResourceReference(TextBlock.TextProperty, "Price");
          
            amountStack.Children.Add(amount);   
            amountStack.Children.Add(new TextBlock
            {
                Text = $"{invoice.Amount:F2} KM",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
            });
            Grid.SetColumn(amountStack, 0);
            Grid.SetRow(amountStack, 1);
            detailsGrid.Children.Add(amountStack);

            if (_isManagerView && !string.IsNullOrEmpty(invoice.ClientFirstName))
            {
                var clientStack = new StackPanel { Margin = new Thickness(10, 0, 0, 5) };
                clientStack.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");
                TextBlock client = new TextBlock
                {
                    FontSize = 12,
                    FontWeight = FontWeights.SemiBold
                };
                client.SetResourceReference(TextBlock.TextProperty, "Client");

                clientStack.Children.Add(client);
                
                clientStack.Children.Add(new TextBlock
                {
                    Text = $"{invoice.ClientFirstName} {invoice.ClientLastName}",
                    FontSize = 14
                });
                Grid.SetColumn(clientStack, 1);
                Grid.SetRow(clientStack, 1);
                detailsGrid.Children.Add(clientStack);
            }

            leftStack.Children.Add(detailsGrid);

            Grid.SetColumn(leftStack, 0);
            mainGrid.Children.Add(leftStack);

            var rightStack = new StackPanel { HorizontalAlignment = HorizontalAlignment.Right };

            var statusBorder = new Border
            {
                CornerRadius = new CornerRadius(20),
                Padding = new Thickness(15, 8, 15, 8),
                Margin = new Thickness(0, 0, 0, 10)
            };

            var statusText = new TextBlock
            {
                Text = GetStatusDisplayBadge(invoice.Status),
                FontSize = 12,
                FontWeight = FontWeights.SemiBold,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            switch (invoice.Status)
            {
                case InvoiceStatus.PAID:
                    statusBorder.Background = new SolidColorBrush(Color.FromRgb(220, 252, 231));
                    statusText.Foreground = new SolidColorBrush(Color.FromRgb(22, 163, 74));
                    break;
                case InvoiceStatus.PENDING:
                    statusBorder.Background = new SolidColorBrush(Color.FromRgb(254, 249, 195));
                    statusText.Foreground = new SolidColorBrush(Color.FromRgb(161, 98, 7));
                    break;
                case InvoiceStatus.CANCELLED:
                    statusBorder.Background = new SolidColorBrush(Color.FromRgb(254, 226, 226));
                    statusText.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                    break;
                default:
                    statusBorder.Background = (Brush)Application.Current.Resources["CardBrush"];
                    statusText.Foreground = (Brush)Application.Current.Resources["TextSecondaryBrush"];
                    break;
            }

            statusBorder.Child = statusText;
            rightStack.Children.Add(statusBorder);

            if (_isManagerView)
            {
                var actionsStack = new StackPanel { Orientation = Orientation.Horizontal };

                if (invoice.Status == InvoiceStatus.PENDING)
                {
                    var markPaidBtn = new Button
                    {
                        Margin = new Thickness(0, 0, 5, 0),
                        Padding = new Thickness(12, 6, 12, 6)
                    };
                    markPaidBtn.SetResourceReference(ContentProperty, "BtnMarkPaid");
                    markPaidBtn.SetResourceReference(StyleProperty, "SuccessTextButton");
                    markPaidBtn.SetResourceReference(ForegroundProperty, "TextOnPrimaryBrush");
                    markPaidBtn.Click += (s, e) => MarkInvoiceAsPaid(invoice);
                    actionsStack.Children.Add(markPaidBtn);
                }

                rightStack.Children.Add(actionsStack);
            }

            Grid.SetColumn(rightStack, 1);
            mainGrid.Children.Add(rightStack);

            card.Child = mainGrid;
            return card;
        }

        private string GetStatusDisplayText(InvoiceStatus status)
        {
            return status switch
            {
                InvoiceStatus.PAID => LocalizationService.GetString("PaidEnum"),
                InvoiceStatus.PENDING => LocalizationService.GetString("PendingEnum"),
                InvoiceStatus.CANCELLED => LocalizationService.GetString("CancelledEnum"),
                _ => status.ToString()
            }; 
        }

        private string GetStatusDisplayBadge(InvoiceStatus status)
        {
            return status switch
            {
                InvoiceStatus.PAID => LocalizationService.GetString("PaidEnumBadge"),
                InvoiceStatus.PENDING => LocalizationService.GetString("PendingEnumBadge"),
                InvoiceStatus.CANCELLED => LocalizationService.GetString("CancelledEnumBadge"),
                _ => status.ToString()
            };
        }

        private void MarkInvoiceAsPaid(Invoice invoice)
        { 
            try
            {
                string message = string.Format(LocalizationService.GetString("MarkInvoiceAsPaid"), invoice.OrderId);
                bool confirmed = CustomYesNoMessageBox.Show(message);

                if (confirmed)
                {
                    InvoiceDao.UpdateInvoiceStatus(invoice.OrderId, InvoiceStatus.PAID);
                    invoice.Status = InvoiceStatus.PAID;
                    LoadInvoices();
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["FailedToUpdateStatus"], (string)Application.Current.Resources["Error"], "⚠");
            }
        }

        private void ShowLoading(bool show)
        {
            LoadingGrid.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            InvoicesScrollViewer.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
            NoInvoicesGrid.Visibility = Visibility.Collapsed;
        }

        private void StatusFilter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (_allInvoices == null) return;
            if (StatusFilterCombo.SelectedItem == null) return;

            List<Invoice> filteredInvoices;

            if (StatusFilterCombo.SelectedIndex == 0)
            {
                filteredInvoices = _allInvoices;
            }
            else
            {
                var selected = StatusFilterCombo.SelectedItem.ToString();
                filteredInvoices = _allInvoices
                    .Where(i => GetStatusDisplayText(i.Status) == selected)
                    .ToList();
            }

            DisplayInvoices(filteredInvoices);
        }

        private void FillStatusFilterCombo()
        {
            var statusItems = new List<string>
            {
                (string)Application.Current.Resources["AllStatuses"]
            };

            statusItems.AddRange(Enum.GetValues(typeof(InvoiceStatus))
                                     .Cast<InvoiceStatus>()
                                     .Select(s => GetStatusDisplayText(s)));

            StatusFilterCombo.ItemsSource = statusItems;
            StatusFilterCombo.FontSize = 10;

        }

        private void RefreshInvoices_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void LocalizationService_OnLanguageChanged()
        {
            RefreshData();
        }

        public void RefreshData()
        {
            LoadInvoices();
            FillStatusFilterCombo();
            StatusFilterCombo.SelectedIndex = 0;
        }
    }
}
