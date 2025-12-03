// Cross-platform MessageBox shim to allow Assembler code to compile
// On macOS/Linux, dialogs will be shown using Avalonia
namespace System.Windows.Forms
{
    public static class MessageBox
    {
        public static void Show(string message)
        {
            // On non-Windows platforms, just log to console for now
            // In full implementation, would show Avalonia dialog
            Console.WriteLine($"[MessageBox] {message}");
        }

        public static void Show(string message, string caption)
        {
            Console.WriteLine($"[MessageBox - {caption}] {message}");
        }
    }
}

