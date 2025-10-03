using HCI_A.Dao;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace HCI_A.Helpers
{
    public static class LocalizationService
    {
        private static string _currentLanguage = "en";
        private static readonly Dictionary<string, string> _languageFiles = new Dictionary<string, string>
        {
            ["en"] = "Resources/Languages/EnglishLanguage.xaml",
            ["sr"] = "Resources/Languages/SerbianLanguage.xaml"
        };

        static LocalizationService()
        {
            SetLanguage("en");
        }

        public static void SetLanguage(string languageCode)
        {
            if (!_languageFiles.ContainsKey(languageCode) || _currentLanguage == languageCode)
                return;

            try
            {
                // Remove existing language dictionary
                RemoveCurrentLanguageDictionary();

                // Load new language dictionary
                var languageDict = new ResourceDictionary();
                languageDict.Source = new Uri($"pack://application:,,,/{_languageFiles[languageCode]}");

                // Add to application resources at the beginning so it takes precedence
                Application.Current.Resources.MergedDictionaries.Insert(0, languageDict);

                _currentLanguage = languageCode;
                if (AppSession.CurrentUser != null)
                    UserDao.UpdateLanguage(AppSession.CurrentUser.UserId, _currentLanguage);

                // Set culture for formatting
                var culture = languageCode == "sr" ? new CultureInfo("sr-Latn-RS") : new CultureInfo("en-US");
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;

                // Force refresh of all windows
                RefreshAllWindows();

                // Notify subscribers
                OnLanguageChanged?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading language resources: {ex.Message}", "Localization Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void RemoveCurrentLanguageDictionary()
        {
            // Remove any existing language dictionaries
            var toRemove = new List<ResourceDictionary>();

            foreach (var dict in Application.Current.Resources.MergedDictionaries)
            {
                if (dict.Source != null &&
                    (dict.Source.OriginalString.Contains("EnglishLanguage.xaml") ||
                      dict.Source.OriginalString.Contains("SerbianLanguage.xaml")))
                {
                    toRemove.Add(dict);
                }
            }

            foreach (var dict in toRemove)
            {
                Application.Current.Resources.MergedDictionaries.Remove(dict);
            }
        }

        private static void RefreshAllWindows()
        {
            // Force refresh of all open windows
            foreach (Window window in Application.Current.Windows)
            {
                try
                {
                    window.InvalidateVisual();
                    window.UpdateLayout();

                    RefreshFrameworkElement(window);
                }
                catch
                {
     
                }
            }
        }

        private static void RefreshFrameworkElement(System.Windows.DependencyObject element)
        {
            if (element is System.Windows.FrameworkElement fe)
            {
                fe.InvalidateVisual();
                fe.UpdateLayout();
            }

            // Refresh all children
            int childCount = System.Windows.Media.VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(element, i);
                RefreshFrameworkElement(child);
            }
        }

        public static string GetString(string key)
        {
            try
            {
                if (Application.Current.Resources.Contains(key))
                {
                    return Application.Current.Resources[key]?.ToString() ?? key;
                }
            }
            catch
            {
           
            }

            return key;
        }

        public static string CurrentLanguage => _currentLanguage;

        public static event Action OnLanguageChanged;

        public static List<string> GetAvailableLanguages()
        {
            return new List<string> { "en", "sr" };
        }

        public static string GetLanguageDisplayName(string languageCode)
        {
            return languageCode switch
            {
                "en" => "English",
                "sr" => "Srpski",
                _ => languageCode
            };
        }

        public static string GetLanguageFlag(string languageCode)
        {
            return languageCode switch
            {
                "en" => "🇺🇸",
                "sr" => "sr",
                _ => "🌐"
            };
        }

        public static string GetLanguageIcon()
        {
            return "🌐"; 
        }
    }
}
