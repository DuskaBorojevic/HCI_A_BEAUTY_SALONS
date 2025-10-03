using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HCI_A.Models;
using HCI_A.Dao;
using HCI_A.Components.EmpoyeeSharedComponents;
using HCI_A.Controls;

namespace HCI_A.Windows
{
    public partial class BeauticianMainWindow : Window
    { 
        private User _currentUser;
        Employee beautician;

        public const int SALON_INFO_TAB = 0;
        public const int SCHEDULE_TAB = 1;
        public const int SERVICES_TAB = 3;
        public const int PRODUCTS_TAB = 4;
        public const int BILLS_TAB = 5;
        public BeauticianMainWindow(User user, int defaultTabIndex = 0)
        {
            InitializeComponent();
            _currentUser = user;

            beautician = EmployeeDao.GetEmployeeById(_currentUser.UserId);
            DataContext = this;

            var welcomeRun = new Run();
            welcomeRun.SetResourceReference(Run.TextProperty, "WelcomeMessage");

            var userRun = new Run($"{_currentUser?.Username}!");

            WelcomeLabel.Inlines.Clear();
            WelcomeLabel.Inlines.Add(welcomeRun);
            WelcomeLabel.Inlines.Add(", ");
            WelcomeLabel.Inlines.Add(userRun);

            LoadSalonInfoTab();
            LoadScheduleTab();
            LoadServicesTab();
            LoadProductsTab();
            OrdersTabControl.Content = new OrdersControl(beautician.UserId, beautician.BeautySalonId, true);
        }

        private void LoadProductsTab()
        {
            ProductsContent.Children.Clear();
            ProductsContent.Children.Add(ProductsInfo.InitProducts(this, beautician));
        }

        private void LoadSalonInfoTab()
        {
            SalonInfoContent.Children.Clear();
            SalonInfoContent.Children.Add(SalonInfo.InitSalonInfo(this, beautician));
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfileWindow profileWindow = new ProfileWindow(_currentUser);
            profileWindow.Owner = this;
            profileWindow.ShowDialog();
        }

        private void LoadScheduleTab()
        {
            try
            {
                ScheduleContent.Children.Clear();
                ScheduleWindow scheduleWindow = new ScheduleWindow(beautician);

                scheduleWindow.HorizontalAlignment = HorizontalAlignment.Stretch;
                scheduleWindow.VerticalAlignment = VerticalAlignment.Stretch;

                ScheduleContent.Children.Add(scheduleWindow);

                System.Diagnostics.Debug.WriteLine("Schedule tab loaded successfully");
            }
            catch (Exception ex)
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["ScheduleLoadingError"], (string)Application.Current.Resources["Error"], "⚠");
            }
        }

        private void LoadServicesTab()
        {
            ServicesContent.Children.Clear();
            ServicesContent.Children.Add(ServicesInfo.InitServices(this, beautician));
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

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
