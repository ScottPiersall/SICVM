using Avalonia.Controls;
using SIC.Avalonia.ViewModels;

namespace SIC.Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        var viewModel = new MainWindowViewModel();
        viewModel.SetMainWindow(this);
        DataContext = viewModel;
    }
}
