using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace SIC.Avalonia.Views.Dialogs
{
    public partial class SetRegisterWordDialog : Window
    {
        public string Register { get; private set; }
        public int Value { get; private set; }
        public bool DialogResult { get; private set; }

        public SetRegisterWordDialog()
        {
            InitializeComponent();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var registerBox = this.FindControl<ComboBox>("cmbRegister");
                var valueBox = this.FindControl<TextBox>("txtValue");
                
                Register = (registerBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "A";
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

