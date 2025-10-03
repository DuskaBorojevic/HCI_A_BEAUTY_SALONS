using HCI_A.Dao;
using HCI_A.Models.Enums;
using HCI_A.Models;
using System.Windows.Controls;
using System.Windows;
using HCI_A.Windows;
using HCI_A.Helpers;

namespace HCI_A.Components.EmpoyeeSharedComponents
{
    public static class ServicesInfo
    {
        public static UIElement InitServices(Window parentWindow, Employee employee)
        {
            BeautySalon salon = BeautySalonDao.GetBeautySalonById(employee.BeautySalonId);
            var services = ServiceDao.GetServicesByPriceListId(salon.PriceList.PriceListId);

            StackPanel mainPanel = new StackPanel();

            Border servicesCard = new Border();
            servicesCard.SetResourceReference(Border.StyleProperty, "CardStyle");

            StackPanel cardContent = new StackPanel();

            Grid headerContainer = new Grid();
            headerContainer.Margin = new Thickness(0, 0, 0, 15);
            headerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            TextBlock titleBlock = new TextBlock
            {
                FontSize = 18,
                FontWeight = FontWeights.SemiBold,
                VerticalAlignment = VerticalAlignment.Center
            };
            titleBlock.SetResourceReference(TextBlock.TextProperty, "ServicesLabel");
            titleBlock.SetResourceReference(TextBlock.ForegroundProperty, "PrimaryBrush");
            Grid.SetColumn(titleBlock, 0);
            headerContainer.Children.Add(titleBlock);

            if (employee.AccountType == AccountType.MANAGER)
            {
                Button addServiceButton = ButtonFactory.CreateAddButton();
                addServiceButton.Click += (s, e) =>
                {
                    AddServiceDialog dialog = new AddServiceDialog(parentWindow, salon.PriceList.PriceListId);
                    dialog.ShowDialog();
                };
                Grid.SetColumn(addServiceButton, 1);
                headerContainer.Children.Add(addServiceButton);
            }

            cardContent.Children.Add(headerContainer);

            Grid tableHeader = new Grid
            {
                Margin = new Thickness(0, 0, 0, 10),
                Height = 40
            };
            tableHeader.SetResourceReference(Grid.BackgroundProperty, "SecondaryLightBrush");

            for (int i = 0; i < 6; i++)
                tableHeader.ColumnDefinitions.Add(new ColumnDefinition());

            TextBlock selectLabel = CreateHeaderLabel(0);
            TextBlock nameLabel = CreateHeaderLabel(1);
            TextBlock priceLabel = CreateHeaderLabel(2);
            TextBlock durationLabel = CreateHeaderLabel(3);
            TextBlock descriptionLabel = CreateHeaderLabel(4);
            TextBlock serviceTypeLabel = CreateHeaderLabel(5);

            selectLabel.SetResourceReference(TextBlock.TextProperty, "SelectLabel");
            nameLabel.SetResourceReference(TextBlock.TextProperty, "NameLabel");
            priceLabel.SetResourceReference(TextBlock.TextProperty, "PriceLabel");
            durationLabel.SetResourceReference(TextBlock.TextProperty, "DurationLabel");
            descriptionLabel.SetResourceReference(TextBlock.TextProperty, "DescriptionLabel");
            serviceTypeLabel.SetResourceReference(TextBlock.TextProperty, "TypeLabel");

            tableHeader.Children.Add(selectLabel);
            tableHeader.Children.Add(nameLabel);
            tableHeader.Children.Add(priceLabel);
            tableHeader.Children.Add(durationLabel);
            tableHeader.Children.Add(descriptionLabel);
            tableHeader.Children.Add(serviceTypeLabel);

            cardContent.Children.Add(tableHeader);

            foreach (var service in services)
            {
                Grid row = new Grid
                {
                    Margin = new Thickness(0, 5, 0, 0),
                    Height = 40,
                    Cursor = System.Windows.Input.Cursors.Hand
                };
                row.SetResourceReference(Grid.BackgroundProperty, "CardBrush");

                for (int i = 0; i < 6; i++)
                    row.ColumnDefinitions.Add(new ColumnDefinition());

                RadioButton radioButton = new RadioButton
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Tag = service
                };

                TextBlock nameBlock = CreateRowLabel(service.Name, TextAlignment.Left);
                TextBlock priceBlock = CreateRowLabel(service.Price.ToString("F2"), TextAlignment.Center);
                TextBlock durationBlock = CreateRowLabel(service.DurationTime.ToString(), TextAlignment.Center);
                TextBlock descriptionBlock = CreateRowLabel(service.Description, TextAlignment.Left);
                descriptionBlock.TextWrapping = TextWrapping.Wrap;
                TextBlock typeBlock = CreateRowLabel(service.ServiceType.Name, TextAlignment.Center);

                radioButton.Checked += (s, e) =>
                {
                    ServiceInfoWindow serviceInfoWindow = new ServiceInfoWindow(service, employee);
                    serviceInfoWindow.Closed += (sender, args) =>
                    {
                        radioButton.IsChecked = false;
                    };
                    serviceInfoWindow.ShowDialog();
                };

                Grid.SetColumn(radioButton, 0);
                Grid.SetColumn(nameBlock, 1);
                Grid.SetColumn(priceBlock, 2);
                Grid.SetColumn(durationBlock, 3);
                Grid.SetColumn(descriptionBlock, 4);
                Grid.SetColumn(typeBlock, 5);

                row.Children.Add(radioButton);
                row.Children.Add(nameBlock);
                row.Children.Add(priceBlock);
                row.Children.Add(durationBlock);
                row.Children.Add(descriptionBlock);
                row.Children.Add(typeBlock);

                cardContent.Children.Add(row);
            }

            servicesCard.Child = cardContent;
            mainPanel.Children.Add(servicesCard);

            return mainPanel;
        }

        private static TextBlock CreateHeaderLabel(int column)
        {
            TextBlock label = new TextBlock
            {
                FontWeight = FontWeights.SemiBold,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(5)
            };
            label.SetResourceReference(TextBlock.ForegroundProperty, "PrimaryDarkBrush");
            Grid.SetColumn(label, column);
            return label;
        }

        private static TextBlock CreateRowLabel(string text, TextAlignment alignment)
        {
            TextBlock tb = new TextBlock
            {
                Text = text,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = alignment == TextAlignment.Center ? HorizontalAlignment.Center : HorizontalAlignment.Left,
                Padding = new Thickness(5)
            };
            tb.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");
            return tb;
        }
    }
}
