using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HCI_A.Helpers
{
    public static class ButtonFactory
    {
        public static Button CreatePrimaryButton(string content)
        {
            var button = new Button { Content = content };
            button.SetResourceReference(Button.StyleProperty, "PrimaryTextButton");
            //button.Style = (Style)Application.Current.FindResource("PrimaryTextButton");
            return button;
        }

        public static Button CreateSecondaryButton(string content)
        {
            var button = new Button { Content = content };
            button.SetResourceReference(Button.StyleProperty, "SecondaryTextButton");
            //button.Style = (Style)Application.Current.FindResource("SecondaryTextButton");
            return button;
        }

        public static Button CreateSuccessButton(string content)
        {
            var button = new Button { Content = content };
            button.SetResourceReference(Button.StyleProperty, "ErrorTextButton");
            //button.Style = (Style)Application.Current.FindResource("SuccessTextButton");
            return button;
        }

        public static Button CreateErrorButton(string content)
        {
            var button = new Button { Content = content };
            button.SetResourceReference(Button.StyleProperty, "ErrorTextButton");
            //button.Style = (Style)Application.Current.FindResource("ErrorTextButton");
            return button;
        }

        public static Button CreateAddButton()
        {
            var button = new Button();
            button.SetResourceReference(Button.StyleProperty, "AddButton");
            //button.SetResourceReference(Button.ToolTipProperty, "AddNewTooltip");
            return button;
        }

        public static Button CreateEditButton()
        {
            var button = new Button();
            button.SetResourceReference(Button.StyleProperty, "EditButton");
            //button.SetResourceReference(Button.ToolTipProperty, "EditTooltip");
            return button;
        }

        public static Button CreateDeleteButton()
        {
            var button = new Button();
            button.SetResourceReference(Button.StyleProperty, "DeleteButton");
            //button.SetResourceReference(Button.ToolTipProperty, "DeleteTooltip");
            return button;
        }

        public static Button CreateSaveButton()
        {
            var button = new Button();
            button.SetResourceReference(Button.StyleProperty, "SaveButton");
           // button.Style = (Style)Application.Current.FindResource("SaveButton");
            return button;
        }

        public static Button CreateToggleEditButton(bool isEditing = false)
        {
            var button = new Button();
            if (isEditing)
            {
                button.SetResourceReference(Button.StyleProperty, "SaveButton");
                //button.SetResourceReference(Button.ContentProperty, "SubmitBTN");
                //button.Style = (Style)Application.Current.FindResource("SuccessTextButton");
                //button.Content = Application.Current.Resources["SubmitBTN"];
            }
            else
            {
                //button.Style = (Style)Application.Current.FindResource("EditButton");
                button.SetResourceReference(Button.StyleProperty, "EditButton");
            }
            return button;
        }

        public static void SetEditMode(Button button, bool isEditing)
        {
            if (isEditing)
            {
                button.SetResourceReference(Button.StyleProperty, "SaveButton");
                //button.SetResourceReference(Button.ContentProperty, "SubmitBTN");
                //button.Style = (Style)Application.Current.FindResource("SuccessTextButton");
               // button.Content = Application.Current.Resources["SubmitBTN"];
            }
            else
            {
                //button.Style = (Style)Application.Current.FindResource("EditButton");
                button.SetResourceReference(Button.StyleProperty, "EditButton");
            }
        }
    }
}
