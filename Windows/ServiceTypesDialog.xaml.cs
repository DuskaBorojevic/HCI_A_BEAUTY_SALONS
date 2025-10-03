using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HCI_A.Models;
using HCI_A.Dao;
using HCI_A.Helpers;
using Org.BouncyCastle.Security;
using HCI_A.Models.Enums;

namespace HCI_A.Windows
{
    public partial class ServiceTypesDialog : Window, IRefreshable
    {
        private readonly Window _parentWindow;
        private AddServiceTypeDialog _addDialog;

        public ServiceTypesDialog(Window parentWindow)
        {
            InitializeComponent();
            _parentWindow = parentWindow;


            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = parentWindow;

            LoadServiceTypes();
        }

        private void LoadServiceTypes()
        {
            ServiceTypesPanel.Children.Clear();

            Grid header = CreateHeader();
            ServiceTypesPanel.Children.Add(header);

            foreach (var type in ServiceTypeDao.GetServiceTypes())
            {
                ServiceTypesPanel.Children.Add(CreateServiceTypeRow(type));
            }
        }

        private Grid CreateHeader()
        {
            Grid header = new Grid();
            for (int i = 0; i < 3; i++) 
                header.ColumnDefinitions.Add(new ColumnDefinition());

            TextBlock idLabel = CreateHeaderLabel(0);
            TextBlock nameLabel = CreateHeaderLabel(1);
            TextBlock actionsLabel = CreateHeaderLabel(2);

            idLabel.SetResourceReference(TextBlock.TextProperty, "IdLabel");
            idLabel.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");
            nameLabel.SetResourceReference(TextBlock.TextProperty, "NameLabel");
            nameLabel.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");
            actionsLabel.SetResourceReference(TextBlock.TextProperty, "ActionsLabel");
            actionsLabel.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");

            header.Children.Add(idLabel);
            header.Children.Add(nameLabel);
            header.Children.Add(actionsLabel); 

            return header;
        }

        private Grid CreateServiceTypeRow(ServiceType type)
        {
            Grid row = new Grid();
            for (int i = 0; i < 3; i++)
                row.ColumnDefinitions.Add(new ColumnDefinition());

            TextBlock idBlock = new TextBlock
            {
                Text = type.ServiceTypeId.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            idBlock.SetResourceReference(ForegroundProperty, "TextBrush");
            idBlock.SetResourceReference(BackgroundProperty, "CardBrush");


            TextBox nameTextBox = new TextBox
            {
                Text = type.Name,
                IsReadOnly = true,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5),
                Padding = new Thickness(5)
            };
            nameTextBox.SetResourceReference(BackgroundProperty, "CardBrush");
            nameTextBox.SetResourceReference(ForegroundProperty, "TextBrush");

            Button editSubmitButton = ButtonFactory.CreateToggleEditButton(false);
            bool isEditing = false;
            editSubmitButton.Margin = new Thickness(0, 0, 8, 0); 
            editSubmitButton.Click += (s, e) => {
                if (!isEditing)
                {
                    isEditing = true;
                    ButtonFactory.SetEditMode(editSubmitButton, true);
                    nameTextBox.IsReadOnly = false;
                    nameTextBox.SetResourceReference(Control.BackgroundProperty, "InputBackgroundBrush");
                    nameTextBox.Focus();
                }
                else
                {
                    ServiceTypeDao.UpdateServiceType(Int32.Parse(idBlock.Text), nameTextBox.Text);

                    isEditing = false;
                    ButtonFactory.SetEditMode(editSubmitButton, false);
                    nameTextBox.IsReadOnly = true;
                    nameTextBox.SetResourceReference(Control.BackgroundProperty, "ElevatedSurfaceBrush");
                    RefreshData();
                }
            };

            Button deleteButton = ButtonFactory.CreateDeleteButton();
            deleteButton.Tag = type;
            deleteButton.Click += DeleteButton_Click;

            StackPanel actionsPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            actionsPanel.Children.Add(editSubmitButton);
            actionsPanel.Children.Add(deleteButton);

            Grid.SetColumn(idBlock, 0);
            Grid.SetColumn(nameTextBox, 1);
            Grid.SetColumn(actionsPanel, 2);

            row.Children.Add(idBlock);
            row.Children.Add(nameTextBox);
            row.Children.Add(actionsPanel);

            row.SetResourceReference(BackgroundProperty, "ElevatedSurfaceBrush");
            row.Margin = new Thickness(0, 5, 0, 0);
            row.Height = 40;

            return row;
        }

        private TextBlock CreateHeaderLabel(int column)
        {
            TextBlock label = new TextBlock
            {
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetColumn(label, column);
            return label;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ServiceType type)
            {
                bool confirmed = CustomYesNoMessageBox.Show((string)Application.Current.Resources["ConfirmDelete"]);
                if (confirmed)
                {
                    bool success = ServiceTypeDao.DeleteServiceType(type.ServiceTypeId);
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
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _addDialog = new AddServiceTypeDialog(this);
            _addDialog.ShowDialog();
        }

        public void RefreshData() => LoadServiceTypes();
    }

    public class AddServiceTypeDialog : Window
    {
        private ServiceTypesDialog _parent;
        private TextBox _nameTextBox;

        public AddServiceTypeDialog(ServiceTypesDialog parent)
        {
            _parent = parent;

            SetResourceReference(TitleProperty, "AddServiceTypeTitle");
            Width = 400;
            Height = 200;
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = parent;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Grid grid = new Grid
            {
                Margin = new Thickness(20)
            };

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            TextBlock headerText = new TextBlock
            {
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 15)
            };
            headerText.SetResourceReference(TextBlock.TextProperty, "ServiceTypeNameHeader");
            Grid.SetRow(headerText, 0);

            Grid textBoxGrid = new Grid();
            TextBlock placeholder = new TextBlock
            {
                Foreground = Brushes.Gray,
                Margin = new Thickness(10, 5, 0, 0)
            };
            placeholder.SetResourceReference(TextBlock.TextProperty, "NamePlaceholder");
            _nameTextBox = new TextBox
            {
                Padding = new Thickness(8, 5, 8, 5),
                Margin = new Thickness(0, 0, 0, 15),
                BorderBrush = Brushes.LightGray
            };

            _nameTextBox.GotFocus += (s, e) => placeholder.Visibility = Visibility.Collapsed;
            _nameTextBox.LostFocus += (s, e) => {
                if (string.IsNullOrWhiteSpace(_nameTextBox.Text))
                    placeholder.Visibility = Visibility.Visible;
            };

            textBoxGrid.Children.Add(_nameTextBox);
            textBoxGrid.Children.Add(placeholder);
            Grid.SetRow(textBoxGrid, 1);

            Button submitButton = new Button();
            submitButton.SetResourceReference(Button.ContentProperty, "BtnSubmit");
            submitButton.SetResourceReference(Button.StyleProperty, "SuccessTextButton");

            submitButton.HorizontalAlignment = HorizontalAlignment.Center;
            submitButton.Click += SubmitButton_Click;
            Grid.SetRow(submitButton, 2);

            grid.Children.Add(headerText);
            grid.Children.Add(textBoxGrid);
            grid.Children.Add(submitButton);

            Content = grid;

            Loaded += (s, e) => _nameTextBox.Focus();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_nameTextBox.Text))
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["NewServiceTypeError"], (string)Application.Current.Resources["ValidationError"], "⚠");
                _nameTextBox.Focus();
                return;
            }

            bool result = ServiceTypeDao.AddServiceType(_nameTextBox.Text);

            if (result)
            {
                CustomOkMessageBox.ShowMsgSuccessfulAddition();
                Close();
                _parent.RefreshData();
            }
            else
            {
                CustomOkMessageBox.Show((string)Application.Current.Resources["TypeExistsError"], (string)Application.Current.Resources["Error"], "⚠");
                _nameTextBox.SelectAll();
                _nameTextBox.Focus();
            }
        }
    }
}