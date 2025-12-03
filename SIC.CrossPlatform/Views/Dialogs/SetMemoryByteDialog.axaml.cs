using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace SIC.Avalonia.Views.Dialogs
{
    public partial class SetMemoryByteDialog : Window
    {
        public int Address { get; private set; }
        public byte Value { get; private set; }
        public bool DialogResult { get; private set; }

        public SetMemoryByteDialog()
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
                Value = Convert.ToByte(valueBox.Text, 16);
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

