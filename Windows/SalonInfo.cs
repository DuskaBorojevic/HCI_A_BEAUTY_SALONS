using HCI_A.Dao;
using HCI_A.Helpers;
using HCI_A.Models;
using HCI_A.Models.Enums;
using HCI_A.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HCI_A.Components.EmpoyeeSharedComponents
{
    public static class SalonInfo
    {
        static bool editMode = false;

        public static UIElement InitSalonInfo(Window parentWindow, Employee employee)
        {
            BeautySalon salon = BeautySalonDao.GetBeautySalonById(employee.BeautySalonId);

            StackPanel mainPanel = new StackPanel();
            mainPanel.Margin = new Thickness(20);

            TextBox nameTextBox = CreateReadOnlyTextBox(salon.Name);
            TextBox addressTextBox = CreateReadOnlyTextBox(salon.Address);
            TextBox placeTextBox = CreateReadOnlyTextBox(salon.Location.Name);
            TextBox workTimeTextBox = CreateReadOnlyTextBox(salon.WorkTime, true);
            TextBox phoneTextBox = CreateReadOnlyTextBox(salon.PhoneNumber ?? "");

            mainPanel.Children.Add(CreateRow("NameLabel", nameTextBox));
            mainPanel.Children.Add(CreateRow("AddressLabel", addressTextBox));
            mainPanel.Children.Add(CreateRow("CityLabel", placeTextBox));
            mainPanel.Children.Add(CreateRow("WorkingHoursManagerLabel", workTimeTextBox));
            mainPanel.Children.Add(CreateRow("PhoneLabel", phoneTextBox));

            if (employee.AccountType == AccountType.MANAGER)
            {
                Border separator = new Border
                {
                    BorderThickness = new Thickness(0, 1, 0, 0),
                    Margin = new Thickness(0, 10, 0, 20)
                };
                separator.SetResourceReference(Control.BorderBrushProperty, "PrimaryBrush");
                mainPanel.Children.Add(separator);

                Button editSubmitButton = ButtonFactory.CreatePrimaryButton(null);
                editSubmitButton.SetResourceReference(Button.ContentProperty, "BtnEdit");
                editSubmitButton.Width = 200;
                editSubmitButton.Height = 40;
                editSubmitButton.Margin = new Thickness(0, 0, 0, 20);
                editSubmitButton.HorizontalAlignment = HorizontalAlignment.Center;

                editSubmitButton.Click += (s, e) =>
                {
                    if (!editMode)
                    {
                        editMode = true;
                        editSubmitButton.SetResourceReference(Button.ContentProperty, "BtnSave");
                        editSubmitButton.SetResourceReference(Button.StyleProperty, "SuccessTextButton");

                        nameTextBox.IsReadOnly = false;
                        addressTextBox.IsReadOnly = false;
                        placeTextBox.IsReadOnly = false;
                        workTimeTextBox.IsReadOnly = false;
                        phoneTextBox.IsReadOnly = false;

                        nameTextBox.SetResourceReference(Control.BackgroundProperty, "InputBackgroundBrush");
                        addressTextBox.SetResourceReference(Control.BackgroundProperty, "InputBackgroundBrush");
                        placeTextBox.SetResourceReference(Control.BackgroundProperty, "InputBackgroundBrush");
                        workTimeTextBox.SetResourceReference(Control.BackgroundProperty, "InputBackgroundBrush");
                        phoneTextBox.SetResourceReference(Control.BackgroundProperty, "InputBackgroundBrush");

                        nameTextBox.Focus();
                    }
                    else
                    {

                        int postNum = LocationDao.GetLocationIdByName(placeTextBox.Text);
                        bool result = BeautySalonDao.UpdateBeautySalonInfo(
                            employee.BeautySalonId,
                            nameTextBox.Text,
                            addressTextBox.Text,
                            workTimeTextBox.Text,
                            postNum,
                            phoneTextBox.Text
                        );

                        if (result)
                        {
                            editMode = false;
                            editSubmitButton.SetResourceReference(Button.ContentProperty, "EditSalonInformation");
                            editSubmitButton.SetResourceReference(Button.StyleProperty, "PrimaryTextButton");

                            nameTextBox.IsReadOnly = true;
                            addressTextBox.IsReadOnly = true;
                            placeTextBox.IsReadOnly = true;
                            workTimeTextBox.IsReadOnly = true;
                            phoneTextBox.IsReadOnly = true;

                            nameTextBox.SetResourceReference(Control.BackgroundProperty, "CardBrush");
                            addressTextBox.SetResourceReference(Control.BackgroundProperty, "CardBrush");
                            placeTextBox.SetResourceReference(Control.BackgroundProperty, "CardBrush");
                            workTimeTextBox.SetResourceReference(Control.BackgroundProperty, "CardBrush");
                            phoneTextBox.SetResourceReference(Control.BackgroundProperty, "CardBrush");

                            CustomOkMessageBox.ShowMsgSuccessUpdate();

                            ManagerMainWindow managerWindow = new ManagerMainWindow(employee, ManagerMainWindow.SALON_INFO_TAB);
                            managerWindow.Show();
                            parentWindow.Close();
                        }
                        else
                        {
                            CustomOkMessageBox.ShowMsgErrorUpdate();
                        }
                    }
                };

                mainPanel.Children.Add(editSubmitButton);
            }

            return mainPanel;
        }

        private static TextBox CreateReadOnlyTextBox(string text, bool multiline = false)
        {
            TextBox tb = new TextBox
            {
                Text = text,
                IsReadOnly = true,
                Padding = new Thickness(8, 5, 8, 5),
                FontSize = 16
            };
            tb.SetResourceReference(Control.BackgroundProperty, "CardBrush");
            tb.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");

            if (multiline)
            {
                tb.TextWrapping = TextWrapping.Wrap;
                tb.AcceptsReturn = true;
                tb.Height = 100;
            }

            return tb;
        }

        private static Border CreateRow(string labelKey, TextBox textBox)
        {
            Grid rowGrid = new Grid();
            rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBlock label = new TextBlock
            {
                FontWeight = FontWeights.SemiBold,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16
            };
            label.SetResourceReference(TextBlock.TextProperty, labelKey);
            label.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");

            Grid.SetColumn(label, 0);
            Grid.SetColumn(textBox, 1);

            rowGrid.Children.Add(label);
            rowGrid.Children.Add(textBox);

            Border border = new Border
            {
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 10)
            };
            border.SetResourceReference(Border.BackgroundProperty, "CardBrush");
            border.Child = rowGrid;

            return border;
        }
    }
}
