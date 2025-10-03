using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HCI_A.Helpers;

namespace HCI_A.Controls
{
    public partial class ThemeLanguageSelector : UserControl
    {
        public ThemeLanguageSelector()
        {
            InitializeComponent();

            ThemeManager.ThemeChanged += OnThemeChanged;
            LocalizationService.OnLanguageChanged += OnLanguageChanged;

            UpdateIcons();
        }

        private void OnThemeChanged(object sender, string newTheme)
        {
            UpdateIcons();
            ThemePopup.IsOpen = !ThemePopup.IsOpen;
            //ThemeContextMenu.SetResourceReference(BackgroundProperty, "CardBrush");
            //ThemeContextMenu.SetResourceReference(BorderBrushProperty, "BorderBrush"); 

            //LanguageContextMenu.SetResourceReference(BackgroundProperty, "CardBrush");
            //LanguageContextMenu.SetResourceReference(BorderBrushProperty, "BorderBrush");
        }

        private void OnLanguageChanged()
        {
            UpdateIcons();
            LanguagePopup.IsOpen = !LanguagePopup.IsOpen;
        }

        private void UpdateIcons()
        {
            ThemeIcon.Text = ThemeManager.GetThemeIcon();
            LanguageIcon.Text = LocalizationService.GetLanguageIcon();
        }

        //private void ThemeButton_Click(object sender, RoutedEventArgs e)
        //{
        //    ThemeContextMenu.IsOpen = true;
        //}

        //private void LanguageButton_Click(object sender, RoutedEventArgs e)
        //{
        //    LanguageContextMenu.IsOpen = true;
        //}

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            ThemePopup.IsOpen = !ThemePopup.IsOpen;
        }

        private void LanguageButton_Click(object sender, RoutedEventArgs e)
        {
            LanguagePopup.IsOpen = !LanguagePopup.IsOpen;
        }

        private void LightTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.SetTheme("Light");
        }

        private void DarkTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.SetTheme("Dark");
        }

        private void NeutralTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.SetTheme("Neutral");
        }

        private void English_Click(object sender, RoutedEventArgs e)
        {
            LocalizationService.SetLanguage("en");
        }

        private void Serbian_Click(object sender, RoutedEventArgs e)
        {
            LocalizationService.SetLanguage("sr");
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;
            LocalizationService.OnLanguageChanged -= OnLanguageChanged;
        }
    }
}
