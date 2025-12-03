using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace SIC.Avalonia.Views.Dialogs
{
    public partial class SetMemoryWordDialog : Window
    {
        public int Address { get; private set; }
        public int Value { get; private set; }
        public bool DialogResult { get; private set; }

        public SetMemoryWordDialog()
        {
            InitializeComponent();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var addressBox = this.FindControl<TextBox>("txtAddress");
                var valueBox = this.FindControl<TextBox>("txtValue");
                
                Address = Convert.ToInt32(addressBox.Text, 16);
                Value = Convert.ToInt32(valueBox.Text, 16);
                DialogResult = true;
                Close();
            }
            catch
            {
                // Invalid input
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

