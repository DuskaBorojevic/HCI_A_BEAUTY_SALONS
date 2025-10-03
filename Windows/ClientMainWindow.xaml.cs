using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HCI_A.Models;
using HCI_A.Dao;
using Org.BouncyCastle.Cmp;
using System.Windows.Documents;
using System.Windows.Input;
using HCI_A.Controls;

namespace HCI_A.Windows
{
    public partial class ClientMainWindow : Window
    {
        private User _currentUser;

        public ClientMainWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            DataContext = this;

            var welcomeRun = new Run();
            welcomeRun.SetResourceReference(Run.TextProperty, "WelcomeMessage");

            var userRun = new Run($"{_currentUser?.Username}!");

            WelcomeLabel.Inlines.Clear();
            WelcomeLabel.Inlines.Add(welcomeRun);
            WelcomeLabel.Inlines.Add(", ");
            WelcomeLabel.Inlines.Add(userRun);

            LoadSalons();
            LoadCities();
            LoadServiceTypes();
        }

        private void LoadSalons()
        {
            SalonsPanel.Children.Clear();

            try
            {
                var salons = BeautySalonDao.GetBeautySalonsForClient();

                foreach (var salon in salons)
                {
                    AddSalonToPanel(salon);
                }

                if (salons.Count == 0)
                {
                    ShowNoSalonsFound();
                }
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.ShowLoadingError();
            }
        }

        private void AddSalonToPanel(BeautySalon salon)
        {
            Border salonCard = new Border
            {
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(12),
                Margin = new Thickness(0, 0, 0, 15),
                Padding = new Thickness(20, 10, 18, 10), 
                Cursor = System.Windows.Input.Cursors.Hand,
                MinHeight = 120 
            };
            salonCard.SetResourceReference(BackgroundProperty, "ElevatedSurfaceBrush");
            salonCard.SetResourceReference(BorderBrushProperty, "BorderBrush");

            salonCard.MouseEnter += (s, e) => {
                salonCard.SetResourceReference(BackgroundProperty, "HoverBrush");
                salonCard.SetResourceReference(BorderBrushProperty, "PrimaryBrush");
            };
            salonCard.MouseLeave += (s, e) => {
                salonCard.SetResourceReference(BackgroundProperty, "ElevatedSurfaceBrush");
                salonCard.SetResourceReference(BorderBrushProperty, "BorderBrush");
            };

            Grid cardGrid = new Grid();
            cardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            cardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            StackPanel infoPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center
            };

            TextBlock nameText = new TextBlock
            {
                Text = salon.Name,
                FontSize = 18,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 5)
            };
            nameText.SetResourceReference(ForegroundProperty, "TextBrush");

            StackPanel locationPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 8)
            };

            TextBlock locationIcon = new TextBlock
            {
                Text = "📍",
                FontSize = 14,
                Margin = new Thickness(0, 0, 8, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            locationIcon.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");
            TextBlock locationText = new TextBlock
            {
                Text = $"{salon.Location.Name}",
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center
            };
            locationText.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");

            locationPanel.Children.Add(locationIcon);
            locationPanel.Children.Add(locationText);

            TextBlock addressText = new TextBlock
            {
                Text = salon.Address?.Length > 50 ? salon.Address.Substring(0, 47) + "..." : salon.Address,
                FontSize = 12,
                Margin = new Thickness(0, 0, 0, 0)
            };
            addressText.SetResourceReference(ForegroundProperty, "TextSecondaryBrush");

            infoPanel.Children.Add(nameText);
            infoPanel.Children.Add(locationPanel);
            infoPanel.Children.Add(addressText);

            Button viewDetailsButton = new Button
            {
                Width = 150, 
                Height = 40,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Tag = salon.BeautySalonId

            };
            viewDetailsButton.SetResourceReference(Button.StyleProperty, "PrimaryTextButton");
            viewDetailsButton.SetResourceReference(Button.ContentProperty, "ViewDetails");
            viewDetailsButton.Click += ViewDetailsButton_Click;

            Grid.SetColumn(infoPanel, 0);
            Grid.SetColumn(viewDetailsButton, 1);

            cardGrid.Children.Add(infoPanel);
            cardGrid.Children.Add(viewDetailsButton);

            salonCard.Child = cardGrid;

            salonCard.MouseLeftButtonUp += (s, e) => {
                OpenSalonDetails(salon.BeautySalonId);
            };

            SalonsPanel.Children.Add(salonCard);
        }

        private void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag != null)
            {
                int salonId = (int)button.Tag;
                OpenSalonDetails(salonId);
            }
        }

        private void OpenSalonDetails(int salonId)
        {
            try
            {
                SalonInfoWindow salonInfoWindow = new SalonInfoWindow(salonId, _currentUser.UserId);
                salonInfoWindow.Owner = this;
                salonInfoWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.ShowLoadingError();
            }
        }

        private void LoadCities()
        {
            CityComboBox.Items.Clear();

            TextBlock allCitiesText = new TextBlock();
            allCitiesText.SetResourceReference(TextBlock.TextProperty, "AllCities");

            ComboBoxItem allCitiesItem = new ComboBoxItem();
            allCitiesItem.Content = allCitiesText;
            allCitiesItem.Tag = "AllCities"; 

            CityComboBox.Items.Add(allCitiesItem);

            try
            {
                var cities = LocationDao.GetCities();

                foreach (var city in cities)
                {
                    CityComboBox.Items.Add(city.Name);
                }

                CityComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.ShowLoadingError();
            }
        }

        private void LoadServiceTypes()
        {
            ServiceComboBox.Items.Clear();
            TextBlock allServicesText = new TextBlock();
            allServicesText.SetResourceReference(TextBlock.TextProperty, "AllServices");

            ComboBoxItem allServicesItem = new ComboBoxItem();
            allServicesItem.Content = allServicesText;
            allServicesItem.Tag = "AllServices"; 

            ServiceComboBox.Items.Add(allServicesItem);

            try
            {
                var serviceTypes = ServiceTypeDao.GetServiceTypes();

                foreach (var serviceType in serviceTypes)
                {
                    ServiceComboBox.Items.Add(serviceType.Name);
                }

                ServiceComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.ShowLoadingError();
            }
        }

        

        private void ApplyFilters()
        {
            try
            {
                string selectedCity = null;
                string selectedType = null;

                //System.Diagnostics.Debug.WriteLine($"City Filter: {CityComboBox.SelectedItem.ToString()}");
                //System.Diagnostics.Debug.WriteLine($"Service Type filter: {ServiceComboBox.SelectedItem.ToString()}");


                if (CityComboBox.SelectedItem != null)
                {
                    if (CityComboBox.SelectedItem is ComboBoxItem cityItem && cityItem.Tag?.ToString() != "AllCities")
                        selectedCity = cityItem.Content.ToString();
                    else if (!(CityComboBox.SelectedItem is ComboBoxItem))
                        selectedCity = CityComboBox.SelectedItem.ToString();
                }


                if (ServiceComboBox.SelectedItem != null)
                {
                    if (ServiceComboBox.SelectedItem is ComboBoxItem serviceItem && serviceItem.Tag?.ToString() != "AllServices")
                        selectedType = serviceItem.Content.ToString();
                    else if (!(ServiceComboBox.SelectedItem is ComboBoxItem))
                        selectedType = ServiceComboBox.SelectedItem.ToString();
                }

                var filteredSalons = BeautySalonDao.GetBeautySalonsWithFilter(selectedCity, selectedType);

                //filteredSalons = SearchSalons(filteredSalons);

                SalonsPanel.Children.Clear();
                foreach (var salon in filteredSalons)
                    AddSalonToPanel(salon);

                if (filteredSalons.Count == 0)
                {
                    ShowNoSalonsFound();
                }
            }
            catch (Exception)
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["MsgFilterError"],
                    (string)Application.Current.Resources["Error"], "⚠");
            }
        }

        private void SearchSalons()
        {
            string searchText = SearchTextBox.Text?.Trim().ToLower();
            List<BeautySalon> salons = BeautySalonDao.GetBeautySalonsForClient();

            if (!string.IsNullOrEmpty(searchText))
            {
                salons = salons
                    .Where(s =>
                        (s.Name?.ToLower().Contains(searchText) ?? false) ||
                        (s.Location.Name?.ToLower().Contains(searchText) ?? false) ||
                        (s.Address?.ToLower().Contains(searchText) ?? false)
                    ).ToList();
            }
            SalonsPanel.Children.Clear();
            foreach (var salon in salons)
                AddSalonToPanel(salon);

            if (salons.Count == 0)
            {
                ShowNoSalonsFound();
            }
        }

        private void SearchTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SearchSalons();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchSalons();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfilePopup.IsOpen = !ProfilePopup.IsOpen;
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            string text = (string)Application.Current.Resources["SearchBoxTooltip"];
            if (SearchTextBox.Text == text)
            {
                SearchTextBox.Text = "";
                SearchTextBox.Foreground = Brushes.Black;
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.SetResourceReference(TextBlock.TextProperty, "SearchBoxTooltip");
                SearchTextBox.Foreground = Brushes.Gray;
            }
        }

        private void ViewProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfilePopup.IsOpen = false;

            ProfileWindow profileWindow = new ProfileWindow(_currentUser);
            profileWindow.Owner = this;
            profileWindow.ShowDialog();
        }

        private void ViewOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            ProfilePopup.IsOpen = false;

            ProfileWindow profileWindow = new ProfileWindow(_currentUser, 2);
            profileWindow.Owner = this;
            profileWindow.ShowDialog();

        }

        private void ViewInvoicesButton_Click(object sender, RoutedEventArgs e)
        {
            ProfilePopup.IsOpen = false;

            ProfileWindow profileWindow = new ProfileWindow(_currentUser, 3); 
            profileWindow.Owner = this;
            profileWindow.ShowDialog();
        }

        private void ViewCartsButton_Click(object sender, RoutedEventArgs e)
        {
            ProfilePopup.IsOpen = false;

            ProfileWindow profileWindow = new ProfileWindow(_currentUser, 1);
            profileWindow.Owner = this;
            profileWindow.ShowDialog();
        }

        private void ShowNoSalonsFound()
        {
            SalonsPanel.Children.Clear();

            Border noSalonsCard = new Border
            {
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(20),
                Margin = new Thickness(0, 0, 0, 15)
            };
            noSalonsCard.SetResourceReference(BackgroundProperty, "ElevatedSurfaceBrush");
            noSalonsCard.SetResourceReference(BorderBrushProperty, "BorderBrush");

            StackPanel stack = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            TextBlock icon = new TextBlock
            {
                Text = "😿", 
                FontSize = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            TextBlock message = new TextBlock
            {
                FontSize = 16,
                FontStyle = FontStyles.Italic,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 15)
            };
            message.SetResourceReference(TextBlock.ForegroundProperty, "TextSecondaryBrush");
            message.SetResourceReference(TextBlock.TextProperty, "NoMatches");

            Button resetButton = new Button
            {
                Width = 150,
                Height = 40,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            resetButton.SetResourceReference(Button.ContentProperty, "BtnResetFilters");
            resetButton.SetResourceReference(Button.StyleProperty, "PrimaryLargeButton");

            resetButton.Click += ResetButton_Click;

            stack.Children.Add(icon);
            stack.Children.Add(message);
            stack.Children.Add(resetButton);

            noSalonsCard.Child = stack;

            SalonsPanel.Children.Add(noSalonsCard);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (CityComboBox.Items.Count > 0) CityComboBox.SelectedIndex = 0;
            if (ServiceComboBox.Items.Count > 0) ServiceComboBox.SelectedIndex = 0;

            SearchTextBox.SetResourceReference(TextBox.TextProperty, "SearchBoxTooltip");

            LoadSalons();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            bool confirmed = CustomYesNoMessageBox.Show("❓", (string)Application.Current.Resources["ConfirmLogoutQuestion"]);

            if (confirmed)
            {
                new LoginPage().Show();
                this.Close();
            }
        }
    }
}