using System;
using System.Windows;
using System.Windows.Controls;
using HCI_A.Models;
using HCI_A.Dao;
using HCI_A.Models.Enums;

namespace HCI_A.Windows
{
    public partial class ServiceInfoWindow : Window, IRefreshable
    {
        private Service _service;
        private User _user;
        private bool _isEditing = false;

        public ServiceInfoWindow(Service service, User user)
        {
            InitializeComponent();

            _service = service;
            _user = user;

            LoadServiceData();

            if (_user.AccountType == AccountType.CLIENT)
            {
                EditSubmitButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Collapsed;
            }
            else if (_user.AccountType == AccountType.BEAUTICIAN)
            {
                // Zaposleni tip "BEAUTICIAN" može editovati ali ne i brisati
                DeleteButton.IsEnabled = false;
            }

            SetEditMode(false);
        }

        private void LoadServiceData()
        {
            ServiceNameText.Text = _service.Name;
            //IdText.Text = _service.ServiceId.ToString();  // više ne prikazujemo ID

            NameTextBox.Text = _service.Name;
            PriceTextBox.Text = _service.Price.ToString("F2");
            DurationTextBox.Text = _service.DurationTime.ToString();
            DescriptionTextBox.Text = _service.Description;
            TypeTextBox.Text = _service.ServiceType.Name;
        }

        private void SetEditMode(bool isEditing)
        {
            _isEditing = isEditing;

            NameTextBox.IsReadOnly = !isEditing;
            PriceTextBox.IsReadOnly = !isEditing;
            DurationTextBox.IsReadOnly = !isEditing;
            DescriptionTextBox.IsReadOnly = !isEditing;
            TypeTextBox.IsReadOnly = true;

            EditSubmitButton.SetResourceReference(Button.ContentProperty, isEditing ? "BtnSave" : "BtnEdit");
            EditSubmitButton.SetResourceReference(Button.StyleProperty, isEditing ? "SuccessTextButton" : "PrimaryTextButton");

            NameTextBox.SetResourceReference(TextBox.BackgroundProperty, isEditing ? "ElevatedSurfaceBrush" : "CardBrush");
            PriceTextBox.SetResourceReference(TextBox.BackgroundProperty, isEditing ? "ElevatedSurfaceBrush" : "CardBrush");
            DurationTextBox.SetResourceReference(TextBox.BackgroundProperty, isEditing ? "ElevatedSurfaceBrush" : "CardBrush");
            DescriptionTextBox.SetResourceReference(TextBox.BackgroundProperty, isEditing ? "ElevatedSurfaceBrush" : "CardBrush");
            TypeTextBox.SetResourceReference(TextBox.BackgroundProperty, "CardBrush");
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
                    string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                    string.IsNullOrWhiteSpace(DurationTextBox.Text) ||
                    string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
                {
                    CustomOkMessageBox.Show(
                        (string)Application.Current.Resources["RequiredFieldsError"],
                        (string)Application.Current.Resources["ValidationError"],
                        "⚠"
                    );
                    return;
                }

                if (!double.TryParse(PriceTextBox.Text, out double price))
                {
                    CustomOkMessageBox.Show(
                        (string)Application.Current.Resources["InvalidPrice"],
                        (string)Application.Current.Resources["ValidationError"],
                        "⚠"
                    );
                    return;
                }

                if (!TimeSpan.TryParse(DurationTextBox.Text, out TimeSpan time))
                {
                    CustomOkMessageBox.Show(
                        (string)Application.Current.Resources["InvalidDuration"],
                        (string)Application.Current.Resources["ValidationError"],
                        "⚠"
                    );
                    return;
                }

                Employee employee = EmployeeDao.GetEmployeeById(_user.UserId);
                int priceListId = BeautySalonDao.GetBeautySalonById(employee.BeautySalonId).PriceList.PriceListId;
                int serviceTypeId = _service.ServiceType.ServiceTypeId;

                bool result = ServiceDao.UpdateService(
                    _service.ServiceId,
                    NameTextBox.Text,
                    price,
                    time,
                    DescriptionTextBox.Text,
                    priceListId,
                    serviceTypeId
                );

                if (result)
                {
                    _service.Name = NameTextBox.Text;
                    _service.Price = price;
                    _service.DurationTime = time;
                    _service.Description = DescriptionTextBox.Text;

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
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool confirmed = CustomYesNoMessageBox.Show((string)Application.Current.Resources["ConfirmDelete"]);

            if (confirmed)
            {
                bool success = ServiceDao.DeleteService(_service.ServiceId);

                if (success)
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
                    manager.RefreshServicesTab();
                }
            }
        }
    }
}
