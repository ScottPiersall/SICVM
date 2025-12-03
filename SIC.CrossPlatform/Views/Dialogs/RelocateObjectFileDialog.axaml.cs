#nullable enable
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace SIC.Avalonia.Views.Dialogs
{
    public partial class RelocateObjectFileDialog : Window
    {
        public int RelocatedToAddress { get; private set; }
        public bool DialogResult { get; private set; }
        
        private int _maxAddress;
        private int _programLengthInBytes;
        private TextBox? _txtRelocationAddress;

        public RelocateObjectFileDialog() : this(Array.Empty<string>(), Array.Empty<string>()) { }
        
        public RelocateObjectFileDialog(string[] lines, string[] mods)
        {
            InitializeComponent();
            
            int startAddress = 0;
            int programLength = 0;
            string programName = "";
            int modRecordCount = mods.Length;
            
            // Parse header record to get program info
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                if (line[0] == 'H')
                {
                    startAddress = int.Parse(line.Substring(7, 6), System.Globalization.NumberStyles.HexNumber);
                    programLength = int.Parse(line.Substring(13, 6), System.Globalization.NumberStyles.HexNumber);
                    programName = line.Substring(1, 6).TrimEnd();
                    break;
                }
            }
            
            _programLengthInBytes = programLength;
            _maxAddress = 32767 - programLength;
            
            var lblName = this.FindControl<TextBlock>("lblProgramName");
            var lblLength = this.FindControl<TextBlock>("lblProgramLength");
            var lblRecords = this.FindControl<TextBlock>("lblRelocationRecords");
            var txtStart = this.FindControl<TextBox>("txtAssembledStartPoint");
            _txtRelocationAddress = this.FindControl<TextBox>("txtRelocationAddress");
            var lblNote = this.FindControl<TextBlock>("lblRelocateNote");
            
            if (lblName != null) lblName.Text = $"Program Name: {programName}";
            if (lblLength != null) lblLength.Text = $"Program Length: {programLength:X}(hex) bytes";
            if (lblRecords != null) lblRecords.Text = $"# Modification Records: {modRecordCount}";
            if (txtStart != null) txtStart.Text = startAddress.ToString("X6");
            if (_txtRelocationAddress != null) _txtRelocationAddress.Text = startAddress.ToString("X6");
            if (lblNote != null) lblNote.Text = $"Relocation Note:\nAttempting to relocate to an address larger than: {_maxAddress:X6}\nwill exceed available memory.";
        }

        private async void OnOkClick(object sender, RoutedEventArgs e)
        {
            if (_txtRelocationAddress == null) return;
            
            string temp = _txtRelocationAddress.Text ?? "";
            
            if (temp.Length == 0)
            {
                await ShowError("Please specify a relocation address.", "No Relocation Address Specified");
                _txtRelocationAddress.Focus();
                return;
            }
            
            int intValue;
            try
            {
                intValue = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);
            }
            catch (FormatException)
            {
                await ShowError($"{temp} is not a valid Hex value", "Invalid Input");
                return;
            }
            
            if (intValue > 32767)
            {
                await ShowError("The memory address specified is outside of SIC Memory Range", "Invalid Memory Address");
                _txtRelocationAddress.Focus();
                return;
            }
            
            if (intValue > _maxAddress)
            {
                await ShowError($"Invalid relocation. Not enough memory available to relocate this program to {intValue:X6}", "Invalid Memory Address");
                _txtRelocationAddress.Focus();
                return;
            }
            
            if (_programLengthInBytes == 0)
            {
                await ShowError("The program loaded has no records to modify", "No Records");
                return;
            }
            
            RelocatedToAddress = intValue;
            DialogResult = true;
            Close();
        }
        
        private async System.Threading.Tasks.Task ShowError(string message, string title)
        {
            var msgBox = new Window
            {
                Title = title,
                Width = 350,
                Height = 130,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false
            };
            
            var panel = new StackPanel { Margin = new global::Avalonia.Thickness(20) };
            panel.Children.Add(new TextBlock { Text = message, TextWrapping = global::Avalonia.Media.TextWrapping.Wrap });
            var btn = new Button { Content = "OK", Width = 75, HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center, Margin = new global::Avalonia.Thickness(0, 15, 0, 0) };
            btn.Click += (s, ev) => msgBox.Close();
            panel.Children.Add(btn);
            msgBox.Content = panel;
            
            await msgBox.ShowDialog(this);
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

