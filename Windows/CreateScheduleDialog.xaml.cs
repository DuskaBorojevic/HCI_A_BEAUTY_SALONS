using HCI_A.Dao;
using HCI_A.Models;
using System;
using System.Windows;
using System.Windows.Threading;

namespace HCI_A.Windows
{
    public partial class CreateScheduleDialog : Window
    {
        private readonly ScheduleWindow _parentControl;
        private readonly Employee _employee;

        public DateTime Today => DateTime.Today;
        public DateTime NextWeek => DateTime.Today.AddDays(7);

        public CreateScheduleDialog(ScheduleWindow parentControl, Employee employee)
        {
            InitializeComponent();
            _parentControl = parentControl;
            _employee = employee;

            Window parentWindow = Window.GetWindow(parentControl);
            if (parentWindow != null)
            {
                Owner = parentWindow;
            }

            DataContext = this;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (FromDatePicker.SelectedDate == null || ToDatePicker.SelectedDate == null)
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["RequiredFieldsError"], (string)Application.Current.Resources["ValidationError"], "⚠");
                return;
            }

            if (FromDatePicker.SelectedDate > ToDatePicker.SelectedDate)
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["ScheduleDateError"], (string)Application.Current.Resources["ValidationError"], "⚠");
                return;
            }

            string fromDate = FromDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            string toDate = ToDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");

            try
            {
                int scheduleId = WorkScheduleDao.CreateNewSchedule(_employee.UserId, fromDate, toDate);

                if (scheduleId > 0)
                {
                    CustomOkMessageBox.Show((string)Application.Current.Resources["ScheduleCreatedSuccessfully"], (string)Application.Current.Resources["Success"], "👏🏼");

                    Close();

                    // Force refresh and select the new schedule - use Dispatcher to ensure UI updates
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                        _parentControl.ShowNewSchedule(scheduleId);
                    }));
                }
                else
                {
                    CustomOkMessageBox.Show((string)Application.Current.Resources["ScheduleCreateError"], (string)Application.Current.Resources["Error"], "✖️");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating schedule: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

