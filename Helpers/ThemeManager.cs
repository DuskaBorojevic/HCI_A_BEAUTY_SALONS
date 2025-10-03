using HCI_A.Dao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace HCI_A.Helpers
{
    public static class ThemeManager
    {
        private static string _currentTheme = "Light";
        private static readonly Dictionary<string, string> _themeFiles = new Dictionary<string, string>
        {
            ["Light"] = "Resources/Themes/LightTheme.xaml",
            ["Dark"] = "Resources/Themes/DarkTheme.xaml",
            ["Neutral"] = "Resources/Themes/NeutralTheme.xaml"
        };
        private static readonly string SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HCI_A", "settings.txt");

        public static event EventHandler<string> ThemeChanged;

        static ThemeManager()
        {
            LoadThemeFromFile();
        }

        public static void SetTheme(string themeName)
        {
            if (!_themeFiles.ContainsKey(themeName) || _currentTheme == themeName)
                return;

            try
            {
                // Remove existing theme dictionary
                RemoveCurrentThemeDictionary();

                // Load new theme dictionary
                var themeDict = new ResourceDictionary();
                themeDict.Source = new Uri($"pack://application:,,,/{_themeFiles[themeName]}");

                // Add to application resources - insert after language resources
                var insertIndex = 1; // After language dictionary
                if (Application.Current.Resources.MergedDictionaries.Count > insertIndex)
                {
                    Application.Current.Resources.MergedDictionaries.Insert(insertIndex, themeDict);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(themeDict);
                }

                _currentTheme = themeName;
                if (AppSession.CurrentUser != null)
                    UserDao.UpdateTheme(AppSession.CurrentUser.UserId, _currentTheme);
                SaveThemeToFile();

                // Force refresh of all windows
                RefreshAllWindows();

                // Notify subscribers
                ThemeChanged?.Invoke(null, themeName);
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error loading theme: {ex.Message}", "Theme Error",
                //    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void RemoveCurrentThemeDictionary()
        {
            // Remove any existing theme dictionaries
            var toRemove = new List<ResourceDictionary>();

            foreach (var dict in Application.Current.Resources.MergedDictionaries)
            {
                if (dict.Source != null &&
                    (dict.Source.OriginalString.Contains("LightTheme.xaml") ||
                     dict.Source.OriginalString.Contains("DarkTheme.xaml") ||
                     dict.Source.OriginalString.Contains("NeutralTheme.xaml")))
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
                    // Force the window to re-evaluate its resources
                    window.InvalidateVisual();
                    window.UpdateLayout();

                    // Recursively refresh all child elements
                    RefreshFrameworkElement(window);
                }
                catch
                {
                    // Ignore errors during refresh
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

        private static void SaveThemeToFile()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));
                File.WriteAllText(SettingsPath, _currentTheme);
            }
            catch
            {
                // Ignore file save errors
            }
        }

        private static void LoadThemeFromFile()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var savedTheme = File.ReadAllText(SettingsPath).Trim();
                    if (_themeFiles.ContainsKey(savedTheme))
                    {
                        _currentTheme = savedTheme;
                        ApplyTheme(_currentTheme);
                    }
                }
            }
            catch
            {
                // Use default theme if loading fails
            }
        }

        private static void ApplyTheme(string themeName)
        {
            var app = Application.Current;
            if (app?.Resources == null) return;

            // Clear existing theme resources
            RemoveCurrentThemeDictionary();

            // Add new theme
            try
            {
                var themeDict = new ResourceDictionary();
                themeDict.Source = new Uri($"pack://application:,,,/{_themeFiles[themeName]}");
                app.Resources.MergedDictionaries.Add(themeDict);
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error loading theme: {ex.Message}", "Theme Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public static string CurrentTheme => _currentTheme;

        public static string GetThemeIcon()
        {
            return "🎨"; 
        }

        public static List<string> GetAvailableThemes()
        {
            return new List<string> { "Light", "Dark", "Neutral" };
        }
    }
}
