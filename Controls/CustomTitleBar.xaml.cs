using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HCI_A.Controls
{
    public partial class CustomTitleBar : UserControl
    {
        private Window parentWindow;


        public CustomTitleBar()
        {
            InitializeComponent();
            Loaded += CustomTitleBar_Loaded;
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(string),
                typeof(CustomTitleBar),
                new PropertyMetadata(string.Empty));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private void CustomTitleBar_Loaded(object sender, RoutedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) // double click za maximize/restore
            {
                ToggleMaximizeRestore();
            }
            else
            {
                parentWindow?.DragMove();
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            if (parentWindow != null)
                parentWindow.WindowState = WindowState.Minimized;
        }

        private void MaximizeRestore_Click(object sender, RoutedEventArgs e)
        {
            ToggleMaximizeRestore();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            parentWindow?.Close();
        }

        private void ToggleMaximizeRestore()
        {
            if (parentWindow != null)
            {
                if (parentWindow.WindowState == WindowState.Normal)
                {
                    parentWindow.WindowState = WindowState.Maximized;
                }
                else
                {
                    parentWindow.WindowState = WindowState.Normal;
                }
            }
        }
    }
}
