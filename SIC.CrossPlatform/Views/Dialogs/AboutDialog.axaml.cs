using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.ObjectModel;

namespace SIC.Avalonia.Views.Dialogs
{
    public partial class AboutDialog : Window
    {
        public AboutDialog()
        {
            InitializeComponent();
            
            var authors = new ObservableCollection<AuthorInfo>
            {
                new AuthorInfo { Name = "Scott Piersall", Role = "Chief Architect & Lead Developer VM" },
                new AuthorInfo { Name = "Riley Strickland", Role = "Pass 1 & 2 of SIC Assembler" },
                new AuthorInfo { Name = "Ellis Levine", Role = "Pass 1 & 2 of SIC Assembler" },
                new AuthorInfo { Name = "W. Daniel Hiromoto", Role = "Relocating Loader" },
                new AuthorInfo { Name = "Ben DeBruin", Role = "Relocating Loader" },
                new AuthorInfo { Name = "Ahmad Osmani", Role = "Relocating Loader" },
                new AuthorInfo { Name = "Kris Wieben", Role = "GUI & VM Testing" },
                new AuthorInfo { Name = "Brandon Woodrum", Role = "Absolute Loader" },
                new AuthorInfo { Name = "Francisco Romero", Role = "Decimal Memory View" },
                new AuthorInfo { Name = "Josselyn Munoz", Role = "Binary Memory View" },
                new AuthorInfo { Name = "Jacob McGee", Role = "ASCII Memory View" },
                new AuthorInfo { Name = "Carlos Garciagomez", Role = "ASCII Memory View" },
                new AuthorInfo { Name = "Brittany Santos", Role = "Next Instruction Display" },
                new AuthorInfo { Name = "Dylan Strickley", Role = "Bug Fixing & Device Features" },
                new AuthorInfo { Name = "Bryce Stremmel", Role = "Bug Fixing & Device Features" },
                new AuthorInfo { Name = "Adam Walton", Role = "Base Code Editor & Loading SIC Files" },
                new AuthorInfo { Name = "Rory Naughton", Role = "Base Code Editor & Saving SIC Files" },
                new AuthorInfo { Name = "Aaron Swartz", Role = "Base Code Editor & Dark Theme" }
            };
            
            var grid = this.FindControl<DataGrid>("dgAuthors");
            grid.ItemsSource = authors;
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class AuthorInfo
    {
        public string Name { get; set; }
        public string Role { get; set; }
    }
}

