#nullable enable
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using System;
using System.Threading.Tasks;
using SIC.Avalonia.Views.Dialogs;

namespace SIC.Avalonia;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // Set up global exception handlers
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        
        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            ShowExceptionDialog(ex);
        }
    }
    
    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            ShowExceptionDialog(ex);
        }
    }
    
    private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        e.SetObserved();
        Dispatcher.UIThread.Post(() => ShowExceptionDialog(e.Exception));
    }
    
    public static void ShowExceptionDialog(Exception ex)
    {
        try
        {
            Dispatcher.UIThread.Invoke(async () =>
            {
                var dialog = new ExceptionDialog(ex);
                
                if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop 
                    && desktop.MainWindow != null)
                {
                    await dialog.ShowDialog(desktop.MainWindow);
                }
                else
                {
                    dialog.Show();
                }
            });
        }
        catch
        {
            // If we can't show the dialog, just exit
            Console.WriteLine($"Fatal error: {ex.Message}\n{ex.StackTrace}");
            Environment.Exit(1);
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
