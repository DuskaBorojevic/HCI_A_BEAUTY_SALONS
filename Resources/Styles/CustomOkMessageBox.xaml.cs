using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HCI_A.Windows
{
    public partial class CustomOkMessageBox : Window
    {
        public CustomOkMessageBox(string message, string header = "info", string icon = "ℹ️", Window owner = null)
        {
            InitializeComponent();
            ContentTextBlock.Text = message;
            HeaderTextBlock.Text = header;
            IconTextBlock.Text = icon;

            if (owner != null)
                Owner = owner;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public static void Show(string message, string header = "info", string icon = "ℹ️", Window owner = null)
        {
            var box = new CustomOkMessageBox(message, header, icon,  owner);
            box.ShowDialog();
        }

        public static void ShowDeletedSuccess()
        {
            Show((string)Application.Current.Resources["DeletedSuccess"], (string)Application.Current.Resources["Success"], "✓");
        }

        public static void ShowDeleteFailed()
        {
            Show((string)Application.Current.Resources["DeleteFailed"],
                        (string)Application.Current.Resources["Error"], "✖️");
        }
        public static void ShowMsgSuccessUpdate()
        {
            Show((string)Application.Current.Resources["MsgSuccessUpdate"],
                    (string)Application.Current.Resources["Success"], "👏🏼");
        } 
        
        public static void ShowMsgErrorUpdate()
        {
            Show((string)Application.Current.Resources["MsgErrorUpdate"],
                    (string)Application.Current.Resources["Error"], "✖️");
        }
        public static void ShowLoadingError()
        {
            Show((string)Application.Current.Resources["LoadingError"],
                    (string)Application.Current.Resources["Error"], "⚠");
        }

        public static void ShowMsgSuccessfulAddition()
        {
            Show((string)Application.Current.Resources["SuccessfulAddition"], (string)Application.Current.Resources["Success"], "👏🏼");
        }

        public static void ShowMsgAdditionError()
        {
            Show((string)Application.Current.Resources["AdditionError"],
                    (string)Application.Current.Resources["Error"], "✖️");
        }
    }
}
