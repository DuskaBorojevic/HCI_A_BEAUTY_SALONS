using HCI_A.Dao;
using HCI_A.Helpers;
using HCI_A.Models;
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

namespace HCI_A.Windows
{
    public partial class PriceListWindow : Window
    {
        private string _beautySalonName;
        private PriceList _priceList;
        //private int _currentClientId;
        private List<Service> _services;
        private List<Product> _products;

        public PriceListWindow(string beautySalonName, PriceList priceList)
        {
            InitializeComponent();

            _beautySalonName = beautySalonName;
            _priceList = priceList;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            LoadServiceData();
            LoadProductData();

            var labelRun = new Run();
            labelRun.SetResourceReference(Run.TextProperty, "PublicationDate"); 


            var dateRun = new Run($" {_priceList.PublicationDate:dd. MM. yyyy.}");

            PublicationDateTextBlock.Inlines.Clear();
            PublicationDateTextBlock.Inlines.Add(labelRun);
            PublicationDateTextBlock.Inlines.Add(": ");
            PublicationDateTextBlock.Inlines.Add(dateRun);

            SalonNameTextBlock.Text = _beautySalonName;
            //PublicationDateTextBlock.Text = "Publication date: " + _priceList.PublicationDate; //TODO

            DataContext = this;
        }

        private void LoadServiceData()
        {
            _services = ServiceDao.GetServicesByPriceListId(_priceList.PriceListId);
            ServicesDataGrid.ItemsSource = _services;
        }

        private void LoadProductData()
        {
            _products = ProductDao.GetProductsByPriceListId(_priceList.PriceListId);
            ProductsDataGrid.ItemsSource = _products;
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as Product;

            if (product == null)
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["ProductNotIdentified"],
                   (string)Application.Current.Resources["Error"], "⚠");
                return;
            }

            if (!product.Availability)
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["ProductUnavailable"],
                    (string)Application.Current.Resources["Error"], "⚠");
                return;
            }

            var quantityDialog = new ProductQuantityDialog(product);
            if (quantityDialog.ShowDialog() == true)
            {
                var quantity = quantityDialog.SelectedQuantity;

                if (CartDao.AddToCart(AppSession.CurrentUser.UserId, product.ProductId, quantity))
                {
                    string message = string.Format(LocalizationService.GetString("ProductAdded"), quantity, product.Name);
                    CustomOkMessageBox.Show(message,(string)Application.Current.Resources["Success"], "👏🏼");
                }
                else
                {
                    CustomOkMessageBox.Show((string)Application.Current.Resources["ProductAddFailed"],
                   (string)Application.Current.Resources["Error"], "⚠");

                }
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
