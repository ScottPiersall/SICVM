using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SIC.Avalonia.Views.Dialogs
{
    public partial class RelocatePromptDialog : Window
    {
        public bool UseRelocatingLoader { get; private set; }
        public bool DialogResult { get; private set; }

        public RelocatePromptDialog()
        {
            InitializeComponent();
        }

        private void OnAbsoluteClick(object sender, RoutedEventArgs e)
        {
            UseRelocatingLoader = false;
            DialogResult = true;
            Close();
        }

        private void OnRelocateClick(object sender, RoutedEventArgs e)
        {
            UseRelocatingLoader = true;
            DialogResult = true;
            Close();
        }
    }
}

