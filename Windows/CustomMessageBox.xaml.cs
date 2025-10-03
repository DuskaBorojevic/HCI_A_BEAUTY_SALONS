using HCI_A.Helpers;
using HCI_A.Models.Enums;
using System.Windows;

namespace HCI_A.Windows
{
    public partial class CustomMessageBox : Window
    {

        public CustomMessageBox(string message, string header = "Info", Window owner = null)
        {
            InitializeComponent();
            ContentTextBlock.Text = message;
            HeaderTextBlock.Text = header;

            if (owner != null)
                Owner = owner;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Static helper metoda
        public static void Show(string message, string header = "Info", Window owner = null)
        {
            var box = new CustomMessageBox(message, header, owner);
            box.ShowDialog();
        }
    }
}



