#nullable enable
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace SIC.Avalonia.Views.Dialogs
{
    public partial class StopAtMemoryAddressDialog : Window
    {
        public int Address { get; private set; }
        public bool DialogResult { get; private set; }

        public StopAtMemoryAddressDialog() : this("", 0, 0) { }
        
        public StopAtMemoryAddressDialog(string lastLoadedFile, int lastLoadedStart, int lastLoadedLength)
        {
            InitializeComponent();
            
            var txtFile = this.FindControl<TextBox>("txtLastLoadedFile");
            var txtStart = this.FindControl<TextBox>("txtLastLoadedStart");
            var txtLength = this.FindControl<TextBox>("txtLastLoadedLength");
            var txtHalting = this.FindControl<TextBox>("txtCalculatedHaltingPoint");
            
            if (txtFile != null) txtFile.Text = lastLoadedFile;
            if (txtStart != null) txtStart.Text = lastLoadedStart.ToString("X6");
            if (txtLength != null) txtLength.Text = lastLoadedLength.ToString("X6");
            if (txtHalting != null) txtHalting.Text = (lastLoadedStart + lastLoadedLength).ToString("X6");
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var addressBox = this.FindControl<TextBox>("txtAddress");
                Address = Convert.ToInt32(addressBox?.Text ?? "0", 16);
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

