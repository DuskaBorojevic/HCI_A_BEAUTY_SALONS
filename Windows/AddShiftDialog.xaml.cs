using HCI_A.Dao;
using HCI_A.Helpers;
using HCI_A.Models;
using System.Windows;
using System.Windows.Input;

namespace HCI_A.Windows
{
    public partial class AddShiftDialog : Window
    {
        private readonly ScheduleWindow _parentControl;
        private readonly Employee _employee;

        public AddShiftDialog(ScheduleWindow parentControl, Employee employee)
        {
            InitializeComponent();
            _parentControl = parentControl;
            _employee = employee;

            Window parentWindow = Window.GetWindow(parentControl);
            if (parentWindow != null)
            {
                Owner = parentWindow;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(StartTimeTextBox.Text) ||
                string.IsNullOrWhiteSpace(EndTimeTextBox.Text))
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["RequiredFieldsError"], (string)Application.Current.Resources["ValidationError"], "⚠");
                return;
            }

            bool result = ShiftDao.CreateShift(
                NameTextBox.Text,
                _employee.BeautySalonId,
                StartTimeTextBox.Text,
                EndTimeTextBox.Text
            );

            if (result)
            {
                CustomOkMessageBox.ShowMsgSuccessfulAddition();
                Close();
                _parentControl.LoadShifts(); //TODO PROVJERITI OO, PREBACILA METODU NA PUBLIC
            }
            else
            {
                CustomOkMessageBox.ShowMsgAdditionError();
            }
        }
    }
}
