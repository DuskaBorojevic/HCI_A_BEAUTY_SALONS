using HCI_A.Dao;
using HCI_A.Models;
using HCI_A.Models.Enums;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using HCI_A.Helpers;

namespace HCI_A.Windows
{
    public partial class ScheduleWindow : UserControl, IRefreshable
    {
        private readonly Employee _employee;
        private readonly bool _isManager;
        private int _selectedScheduleId;
        private List<WorkSchedule> _schedules;

        public ScheduleWindow(Employee employee)
        {
            InitializeComponent();
            _employee = employee;
            _isManager = employee.AccountType == AccountType.MANAGER;

            DataContext = this;

            if (!_isManager)
            {
                CreateScheduleButton.Visibility = Visibility.Collapsed;
                SaveScheduleButton.Visibility = Visibility.Collapsed;
                DeleteScheduleButton.Visibility = Visibility.Collapsed;
            }

            LocalizationService.OnLanguageChanged += LocalizationService_OnLanguageChanged;

            LoadSchedules();
            LoadShifts();
            LoadMostRecentSchedule();
        }

        private void LoadMostRecentSchedule()
        {
            try
            {
                if (_schedules != null && _schedules.Count > 0)
                {
                    var mostRecentSchedule = _schedules.OrderByDescending(s => s.ScheduleId).FirstOrDefault();

                    if (mostRecentSchedule != null)
                    {
                        for (int i = 0; i < ScheduleComboBox.Items.Count; i++)
                        {
                            ComboBoxItem item = ScheduleComboBox.Items[i] as ComboBoxItem;
                            if (item != null && item.Tag is int scheduleId && scheduleId == mostRecentSchedule.ScheduleId)
                            {
                                ScheduleComboBox.SelectedIndex = i;
                                LoadScheduleData(mostRecentSchedule.ScheduleId);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    ClearScheduleDisplay();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading most recent schedule: {ex.Message}");
            }
        }

        private void ClearScheduleDisplay()
        {
            FromDateBlock.Text = "";
            ToDateBlock.Text = "";
            ScheduleTable.Children.Clear();
            ScheduleTable.RowDefinitions.Clear();
            ScheduleTable.ColumnDefinitions.Clear();
        }

        public void ShowNewSchedule(int scheduleId)
        {
            try
            {
                WorkSchedule newSchedule = WorkScheduleDao.GetScheduleById(scheduleId);
                if (newSchedule == null)
                {
                    CustomOkMessageBox.Show((string)Application.Current.Resources["ScheduleLoadingError"], (string)Application.Current.Resources["Error"], "⚠");
                    return;
                }
                _schedules = WorkScheduleDao.GetWorkSchedulesByBeautySalonId(_employee.BeautySalonId);

                ScheduleComboBox.Items.Clear();
                int selectedIndex = -1;

                for (int i = 0; i < _schedules.Count; i++)
                {
                    var schedule = _schedules[i];
                    string displayText = $"{schedule.From} to {schedule.To}";
                    ComboBoxItem item = new ComboBoxItem
                    {
                        Content = displayText,
                        Tag = schedule.ScheduleId
                    };

                    ScheduleComboBox.Items.Add(item);

                    if (schedule.ScheduleId == scheduleId)
                        selectedIndex = i;
                }

                if (selectedIndex == -1)
                {
                    string displayText = $"{newSchedule.From} to {newSchedule.To}";
                    ComboBoxItem item = new ComboBoxItem
                    {
                        Content = displayText,
                        Tag = newSchedule.ScheduleId
                    };

                    ScheduleComboBox.Items.Add(item);
                    selectedIndex = ScheduleComboBox.Items.Count - 1;
                    _schedules.Add(newSchedule);
                }

                MainTabControl.SelectedIndex = 0;

                if (selectedIndex >= 0)
                    ScheduleComboBox.SelectedIndex = selectedIndex;

                LoadScheduleDataDirect(newSchedule);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error showing schedule: {ex.Message}");
            }
        }

        private void LoadSchedules()
        {
            try
            {
                _schedules = WorkScheduleDao.GetWorkSchedulesByBeautySalonId(_employee.BeautySalonId);
                ScheduleComboBox.Items.Clear();

                if (_schedules == null || _schedules.Count == 0)
                {
                    ComboBoxItem placeholderItem = new ComboBoxItem
                    {
                        Tag = -1,
                        IsEnabled = false
                    };
                    placeholderItem.SetResourceReference(ComboBoxItem.ContentProperty, "NoSchedulesPlaceholder");
                    ScheduleComboBox.Items.Add(placeholderItem);
                    ScheduleComboBox.SelectedIndex = 0;
                    return;
                }

                _schedules = _schedules.OrderByDescending(s => s.ScheduleId).ToList();

                foreach (var schedule in _schedules)
                {
                    string displayText = $"{schedule.From} - {schedule.To}";
                    ComboBoxItem item = new ComboBoxItem
                    {
                        Content = displayText,
                        Tag = schedule.ScheduleId
                    };

                    ScheduleComboBox.Items.Add(item);
                }
            }
            catch
            {
                ComboBoxItem errorItem = new ComboBoxItem
                {
                    Tag = -1,
                    IsEnabled = false
                };
                errorItem.SetResourceReference(ComboBoxItem.ContentProperty, "LoadingSchedulesErrorPlaceholder");
                ScheduleComboBox.Items.Add(errorItem);
                ScheduleComboBox.SelectedIndex = 0;
            }
        }

        public void LoadShifts()
        {
            if (ShiftsPanel.Children.Count > 0)
                ShiftsPanel.Children.Clear();

            Grid headerGrid = new Grid { Margin = new Thickness(0, 0, 0, 15) };
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            TextBlock headerText = new TextBlock
            {
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 0, 0)
            };
            headerText.SetResourceReference(TextBlock.TextProperty, "ShiftsLabel");
            headerText.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");
            headerGrid.Children.Add(headerText);

            if (_isManager)
            {
                Button addShiftButton = ButtonFactory.CreateAddButton();
                addShiftButton.Margin = new Thickness(0, 0, 10, 0);
                addShiftButton.Click += AddShiftButton_Click;
                Grid.SetColumn(addShiftButton, 1);
                headerGrid.Children.Add(addShiftButton);
            }

            ShiftsPanel.Children.Add(headerGrid);

            Grid headerRow = new Grid
            {
                Margin = new Thickness(0, 0, 0, 10),
                HorizontalAlignment = HorizontalAlignment.Left,
                MaxWidth = 800,
            };

            headerRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            headerRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            headerRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            if (_isManager)
            {
                headerRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
                headerRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
            }

            TextBlock nameHeader = new TextBlock { FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Center };
            nameHeader.SetResourceReference(TextBlock.TextProperty, "NameLabel");
            nameHeader.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");
            nameHeader.FontSize = 16;
            Grid.SetColumn(nameHeader, 0);
            headerRow.Children.Add(nameHeader);

            TextBlock startTimeHeader = new TextBlock { FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Center };
            startTimeHeader.SetResourceReference(TextBlock.TextProperty, "StartTimeLabel");
            startTimeHeader.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");
            startTimeHeader.FontSize = 16;
            Grid.SetColumn(startTimeHeader, 1);
            headerRow.Children.Add(startTimeHeader);

            TextBlock endTimeHeader = new TextBlock { FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Center };
            endTimeHeader.SetResourceReference(TextBlock.TextProperty, "EndTimeLabel");
            endTimeHeader.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");
            endTimeHeader.FontSize = 16;
            Grid.SetColumn(endTimeHeader, 2);
            headerRow.Children.Add(endTimeHeader);

            ShiftsPanel.Children.Add(headerRow);

            var shifts = ShiftDao.GetShiftsByBeautySalonId(_employee.BeautySalonId);

            foreach (var shift in shifts)
            {
                Grid shiftGrid = new Grid
                {
                    Margin = new Thickness(0, 0, 0, 10),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    MaxWidth = 800
                };

                shiftGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
                shiftGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
                shiftGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
                if (_isManager)
                {
                    shiftGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
                    shiftGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
                }

                TextBox nameTextBox = new TextBox
                {
                    Text = shift.Name,
                    IsReadOnly = true,
                    Margin = new Thickness(5),
                    Padding = new Thickness(5),
                    FontSize = 16
                };
                nameTextBox.SetResourceReference(BackgroundProperty, "CardBrush");
                nameTextBox.SetResourceReference(ForegroundProperty, "TextBrush");

                TextBox startTimeTextBox = new TextBox
                {
                    Text = shift.FromTime.ToString(),
                    IsReadOnly = true,
                    Margin = new Thickness(5),
                    Padding = new Thickness(5),
                    FontSize = 16
                };
                startTimeTextBox.SetResourceReference(BackgroundProperty, "CardBrush");
                startTimeTextBox.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");

                TextBox endTimeTextBox = new TextBox
                {
                    Text = shift.ToTime.ToString(),
                    IsReadOnly = true,
                    Margin = new Thickness(5),
                    Padding = new Thickness(5),
                    FontSize = 16
                };
                endTimeTextBox.SetResourceReference(BackgroundProperty, "CardBrush");
                endTimeTextBox.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");

                shiftGrid.Children.Add(nameTextBox);
                Grid.SetColumn(nameTextBox, 0);
                shiftGrid.Children.Add(startTimeTextBox);
                Grid.SetColumn(startTimeTextBox, 1);
                shiftGrid.Children.Add(endTimeTextBox);
                Grid.SetColumn(endTimeTextBox, 2);

                if (_isManager)
                {
                    Button editSaveButton = ButtonFactory.CreateEditButton();
                    Grid.SetColumn(editSaveButton, 3);
                    shiftGrid.Children.Add(editSaveButton);

                    bool isEditing = false;

                    editSaveButton.Click += (s, e) =>
                    {
                        if (!isEditing)
                        {
                            nameTextBox.IsReadOnly = false;
                            startTimeTextBox.IsReadOnly = false;
                            endTimeTextBox.IsReadOnly = false;
                            editSaveButton.SetResourceReference(Button.StyleProperty, "SaveButton");
                            isEditing = true;
                        }
                        else
                        {

                            bool result = ShiftDao.UpdateShift(
                                nameTextBox.Text,
                                _employee.BeautySalonId,
                                startTimeTextBox.Text,
                                endTimeTextBox.Text
                            );

                            if (result)
                            {
                                CustomOkMessageBox.ShowMsgSuccessUpdate();
                                editSaveButton.SetResourceReference(Button.StyleProperty, "EditButton");
                                RefreshData();
                            }
                            else
                            {
                                CustomOkMessageBox.ShowMsgErrorUpdate();
                                editSaveButton.SetResourceReference(Button.StyleProperty, "EditButton");
                            }
                        }
                    };

                    Button deleteButton = ButtonFactory.CreateDeleteButton();
                    deleteButton.Click += (s, e) =>
                    {
                        bool confirmed = CustomYesNoMessageBox.Show((string)Application.Current.Resources["ConfirmDelete"]);
                        if (confirmed)
                        {
                            bool result = ShiftDao.DeleteShift(shift.Name, _employee.BeautySalonId);

                            if (result)
                            {
                                CustomOkMessageBox.ShowDeletedSuccess();
                                RefreshData();
                            }
                            else
                            {
                                CustomOkMessageBox.ShowDeleteFailed();
                            }
                        }
                    };

                    Grid.SetColumn(deleteButton, 4);
                    shiftGrid.Children.Add(deleteButton);
                }

                ShiftsPanel.Children.Add(shiftGrid);
            }
        }


        private void LoadScheduleDataDirect(WorkSchedule selectedSchedule)
        {
            try
            {
                _selectedScheduleId = selectedSchedule.ScheduleId;

                DateTime fromDate = DateTime.Parse(selectedSchedule.From);
                FromDateBlock.Text = fromDate.ToString("dd. MM. yyyy.");

                DateTime toDate = DateTime.Parse(selectedSchedule.To);
                ToDateBlock.Text = toDate.ToString("dd. MM. yyyy.");

                ScheduleTable.Children.Clear();
                ScheduleTable.RowDefinitions.Clear();
                ScheduleTable.ColumnDefinitions.Clear();

                string[] daysOfWeek =
                {
                    (string)FindResource("DayMonday"),
                    (string)FindResource("DayTuesday"),
                    (string)FindResource("DayWednesday"),
                    (string)FindResource("DayThursday"),
                    (string)FindResource("DayFriday"),
                    (string)FindResource("DaySaturday"),
                    (string)FindResource("DaySunday")
                };

                ScheduleTable.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
                for (int i = 0; i < daysOfWeek.Length; i++)
                    ScheduleTable.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                ScheduleTable.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                //Border employeeHeaderBorder = CreateHeaderCell("EMPLOYEES");   //PROVJERITIII!!!!!!!
                Border employeeHeaderBorder = CreateHeaderCell(null);
                employeeHeaderBorder.SetResourceReference(TextBlock.TextProperty, "EmployeesLabel");
                ScheduleTable.Children.Add(employeeHeaderBorder);

                for (int i = 0; i < daysOfWeek.Length; i++)
                {
                    Border dayHeaderBorder = CreateHeaderCell(daysOfWeek[i]); ///TODO
                    Grid.SetRow(dayHeaderBorder, 0);
                    Grid.SetColumn(dayHeaderBorder, i + 1);
                    ScheduleTable.Children.Add(dayHeaderBorder);
                }

                var employees = EmployeeDao.GetEmployeesBySalonId(_employee.BeautySalonId);
                var beauticians = employees.Where(e => e.AccountType != AccountType.MANAGER).ToList();
                var shifts = ShiftDao.GetShiftsByBeautySalonId(_employee.BeautySalonId);

                for (int i = 0; i < beauticians.Count; i++)
                {
                    ScheduleTable.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    Border employeeBorder = CreateDataCell($"{beauticians[i].FirstName} {beauticians[i].LastName}");
                    Grid.SetRow(employeeBorder, i + 1);
                    ScheduleTable.Children.Add(employeeBorder);

                    for (int j = 0; j < daysOfWeek.Length; j++)
                    {
                        Border cellBorder = new Border
                        {
                            BorderThickness = new Thickness(1),
                            Margin = new Thickness(2),
                            Padding = new Thickness(5)
                        };
                        cellBorder.SetResourceReference(BorderBrushProperty, "BorderBrush");
                        cellBorder.SetResourceReference(BackgroundProperty, "ElevatedSurfaceBrush");

                        ScheduleItem item = ScheduleItemDao.GetScheduleItemByEmployeeAndDay(
                            beauticians[i].UserId,
                            DayMapper(daysOfWeek[j]),
                            selectedSchedule.ScheduleId
                        );
                        System.Diagnostics.Debug.WriteLine(DayMapper(daysOfWeek[j]));   
                        System.Diagnostics.Debug.WriteLine(item);   

                        if (_isManager)
                        {
                            ComboBox shiftComboBox = new ComboBox
                            {
                                Margin = new Thickness(2),
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Center,
                                Tag = new ScheduleCellInfo
                                {
                                    EmployeeId = beauticians[i].UserId,
                                    DayOfWeek = daysOfWeek[j],
                                    ScheduleId = selectedSchedule.ScheduleId
                                }
                            };

                            shiftComboBox.Items.Add(new ComboBoxItem { Content = " " });

                            foreach (var shift in shifts)
                            {
                                ComboBoxItem shiftItem = new ComboBoxItem
                                {
                                    Content = shift.Name,
                                    Tag = shift
                                };

                                shiftComboBox.Items.Add(shiftItem);

                                if (item != null && item.Shift.Name == shift.Name)
                                    shiftComboBox.SelectedItem = shiftItem;
                            }

                            if (item == null)
                                shiftComboBox.SelectedIndex = 0;

                            cellBorder.Child = shiftComboBox;
                        }
                        else
                        {
                            TextBlock shiftText = new TextBlock
                            {
                                Text = item != null ? item.Shift.Name : "",
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center
                            };

                            cellBorder.Child = shiftText;
                        }

                        Grid.SetRow(cellBorder, i + 1);
                        Grid.SetColumn(cellBorder, j + 1);
                        ScheduleTable.Children.Add(cellBorder);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading schedule data: {ex.Message}");
            }
        }

        private string DayMapper(String day)
        {
            if (day == LocalizationService.GetString("DayMonday"))
                return "Monday";
            else if (day == LocalizationService.GetString("DayTuesday"))
                return "Tuesday";
            else if (day == LocalizationService.GetString("DayWednesday"))
                return "Wednesday";
            else if (day == LocalizationService.GetString("DayThursday"))
                return "Thursday";
            else if (day == LocalizationService.GetString("DayFriday"))
                return "Friday";
            else if (day == LocalizationService.GetString("DaySaturday"))
                return "Saturday";
            else 
                return "Sunday";

        }

        private void LoadScheduleData(int scheduleId)
        {
            try
            {
                if (scheduleId == -1)
                {
                    ClearScheduleDisplay();
                    return;
                }

                WorkSchedule selectedSchedule = _schedules.FirstOrDefault(s => s.ScheduleId == scheduleId);
                if (selectedSchedule == null)
                {
                    CustomOkMessageBox.ShowLoadingError();
                    return;
                }

                LoadScheduleDataDirect(selectedSchedule);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading schedule data: {ex.Message}");
            }
        }

        private void LocalizationService_OnLanguageChanged()
        {
            if (_selectedScheduleId != -1)
            {
                var schedule = WorkScheduleDao.GetScheduleById(_selectedScheduleId);
                if (schedule != null)
                    LoadScheduleDataDirect(schedule);
            }
        }

        private Border CreateHeaderCell(string text)
        {
            Border border = new Border
            {
                BorderThickness = new Thickness(1),
                Margin = new Thickness(2),
                Padding = new Thickness(10, 8, 10, 8)
            };
            border.SetResourceReference(BorderBrushProperty, "BorderBrush");
            border.SetResourceReference(BackgroundProperty, "PrimaryBrush");

            TextBlock textBlock = new TextBlock
            {
                Text = text,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            border.Child = textBlock;
            return border;
        }

        private Border CreateDataCell(string text)
        {
            Border border = new Border
            {
                BorderThickness = new Thickness(1),
                Margin = new Thickness(2),
                Padding = new Thickness(10, 8, 10, 8)
            };
            border.SetResourceReference(BorderBrushProperty, "BorderBrush");
            border.SetResourceReference(BackgroundProperty, "ElevatedSurfaceBrush");

            TextBlock textBlock = new TextBlock
            {
                Text = text,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center, 
                FontSize=16,
            };
            textBlock.SetResourceReference(ForegroundProperty, "TextBrush");

            border.Child = textBlock;
            return border;
        }

        private void ScheduleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is int scheduleId)
            {
                LoadScheduleData(scheduleId);
            }
        }

        private void CreateScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            CreateScheduleDialog dialog = new CreateScheduleDialog(this, _employee);
            dialog.ShowDialog();
        }

        private void DeleteScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            bool confirmed = CustomYesNoMessageBox.Show((string)Application.Current.Resources["ConfirmDelete"]);


            if (confirmed)
            {
                bool success = WorkScheduleDao.DeleteSchedule(_selectedScheduleId);

                if (success)
                {
                    CustomOkMessageBox.ShowDeletedSuccess();
                    RefreshData();
                }
                else
                {
                    CustomOkMessageBox.ShowDeleteFailed();
                }
            }
        }

        private void AddShiftButton_Click(object sender, RoutedEventArgs e)
        {
            AddShiftDialog dialog = new AddShiftDialog(this, _employee);
            dialog.ShowDialog();
        }

        private void SaveScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isManager) return;

            bool success = true;

            foreach (var child in ScheduleTable.Children)
            {
                if (child is Border border && border.Child is ComboBox comboBox && comboBox.Tag is ScheduleCellInfo cellInfo)
                {
                    ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
                    if (selectedItem != null)
                    {
                        if (comboBox.SelectedIndex == 0)
                        {
                            // "No shift"
                            ScheduleItemDao.RemoveScheduleItem(cellInfo.EmployeeId, cellInfo.DayOfWeek, cellInfo.ScheduleId);
                        }
                        else if (selectedItem.Tag is Shift shift)
                        {
                            bool result = ScheduleItemDao.AddOrUpdateScheduleItem(
                                cellInfo.DayOfWeek,
                                cellInfo.EmployeeId,
                                _employee.BeautySalonId,
                                shift.Name,
                                cellInfo.ScheduleId
                            );
                            if (!result) success = false;
                        }
                    }
                }
            }

            if (success)
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["MsgSuccessSave"],
                    (string)Application.Current.Resources["Success"], "✓");

                LoadScheduleData(_selectedScheduleId);
            }
            else
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["MsgErrorSave"],
                    (string)Application.Current.Resources["Error"], "⚠");
            }
        }

        public void RefreshData()
        {
            LoadShifts();
            LoadSchedules();
            LoadMostRecentSchedule();
        }

    }

    public class ScheduleCellInfo
    {
        public int EmployeeId { get; set; }
        public string DayOfWeek { get; set; }
        public int ScheduleId { get; set; }
    }
}




















