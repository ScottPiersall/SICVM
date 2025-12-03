using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace SIC.Avalonia.Views.Dialogs
{
    public partial class SetDeviceContentDialog : Window
    {
        public string HexContent { get; private set; }
        public bool DialogResult { get; private set; }

        public SetDeviceContentDialog()
        {
            InitializeComponent();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            var hexBox = this.FindControl<TextBox>("txtHex");
            HexContent = hexBox.Text;
            DialogResult = true;
            Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

