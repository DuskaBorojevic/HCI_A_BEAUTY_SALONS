using System.Windows;

namespace HCI_A.Windows
{
    public partial class CustomYesNoMessageBox : Window
    {
        public bool Result { get; private set; }

        public CustomYesNoMessageBox(string message)
        {
            InitializeComponent();
            ContentTextBlock.Text = message;
        }

        public CustomYesNoMessageBox(string title, string message)
        {
            InitializeComponent();
            TitleTextBlock.Text=title;
            ContentTextBlock.Text = message;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            DialogResult = true; 
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            DialogResult = false;
        }

        public static bool Show(string message)
        {
            var box = new CustomYesNoMessageBox(message);
            return box.ShowDialog() == true;
        }

        public static bool Show(string title, string message)
        {
            var box = new CustomYesNoMessageBox(title, message);
            return box.ShowDialog() == true;
        }
    }
}
