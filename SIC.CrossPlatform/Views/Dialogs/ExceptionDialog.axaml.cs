using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace SIC.Avalonia.Views.Dialogs
{
    public partial class ExceptionDialog : Window
    {
        private bool _detailsVisible = false;
        
        public bool ShouldContinue { get; private set; } = false;

        public ExceptionDialog()
        {
            InitializeComponent();
        }
        
        public ExceptionDialog(Exception ex) : this()
        {
            var txtShort = this.FindControl<TextBlock>("txtShortMessage");
            var txtDetails = this.FindControl<TextBox>("txtDetails");
            
            if (txtShort != null)
            {
                txtShort.Text = ex.Message;
            }
            
            if (txtDetails != null)
            {
                txtDetails.Text = $"See the end of this message for details on invoking\njust-in-time (JIT) debugging instead of this dialog box.\n\n************** Exception Text **************\n{ex.GetType().FullName}: {ex.Message}\n{ex.StackTrace}";
            }
        }

        private void OnDetailsClick(object sender, RoutedEventArgs e)
        {
            var detailsPanel = this.FindControl<Border>("detailsPanel");
            var btnDetails = this.FindControl<Button>("btnDetails");
            
            if (detailsPanel != null && btnDetails != null)
            {
                _detailsVisible = !_detailsVisible;
                detailsPanel.IsVisible = _detailsVisible;
                btnDetails.Content = _detailsVisible ? "▲ Details" : "▼ Details";
                
                // Resize window
                this.Height = _detailsVisible ? 450 : 320;
            }
        }

        private void OnContinueClick(object sender, RoutedEventArgs e)
        {
            ShouldContinue = true;
            Close();
        }

        private void OnQuitClick(object sender, RoutedEventArgs e)
        {
            ShouldContinue = false;
            Close();
            Environment.Exit(1);
        }
    }
}

