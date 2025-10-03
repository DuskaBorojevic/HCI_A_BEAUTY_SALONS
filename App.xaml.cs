using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace HCI_A
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void ChangeLanguage(string langCode)
        {
            // Postavljanje kulture aplikacije
            CultureInfo culture = new CultureInfo(langCode);
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            // Određivanje odgovarajućeg fajla sa jezikom
            string dictPath = $"Languages/{(langCode == "sr" ? "SerbianLanguage.xaml" : "EnglishLanguage.xaml")}";
            ResourceDictionary newLangDict = new ResourceDictionary { Source = new Uri(dictPath, UriKind.Relative) };

            // Pronalazimo i uklanjamo prethodni jezički ResourceDictionary
            ResourceDictionary oldLangDict = Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Languages/"));

            if (oldLangDict != null)
            {
                Current.Resources.MergedDictionaries.Remove(oldLangDict);
            }

            // Dodajemo novi jezički ResourceDictionary
            Current.Resources.MergedDictionaries.Add(newLangDict);

            // Ažuriranje jezika za ceo UI
            FrameworkElement mainWindow = Current.MainWindow;
            if (mainWindow != null)
            {
                mainWindow.Language = XmlLanguage.GetLanguage(culture.IetfLanguageTag);
            }
        }

        public void SwitchTheme(bool isDarkTheme)
        {
            var oldTheme = Application.Current.Resources.MergedDictionaries[0];
            var newTheme = new ResourceDictionary
            {
                Source = new Uri(isDarkTheme ?
                    "Themes/DarkTheme.xaml" :
                    "Themes/LightTheme.xaml",
                    UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries[0] = newTheme;
        }
    }
}
