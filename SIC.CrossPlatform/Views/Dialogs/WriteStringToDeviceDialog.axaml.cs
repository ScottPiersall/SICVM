using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SIC.Avalonia.Views.Dialogs
{
    public partial class WriteStringToDeviceDialog : Window
    {
        public string StringContent { get; private set; }
        public bool DialogResult { get; private set; }

        public WriteStringToDeviceDialog()
        {
            InitializeComponent();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            var stringBox = this.FindControl<TextBox>("txtString");
            StringContent = stringBox.Text;
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

