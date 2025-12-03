using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SIC.Avalonia.Views.Dialogs
{
    public partial class RelocationWarningDialog : Window
    {
        public bool DialogResult { get; private set; }

        public RelocationWarningDialog()
        {
            InitializeComponent();
        }

        private void OnProceedClick(object sender, RoutedEventArgs e)
        {
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

