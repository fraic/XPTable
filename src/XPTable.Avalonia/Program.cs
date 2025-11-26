using Avalonia;
using Avalonia.ReactiveUI;
using System;

namespace XPTable.AvaloniaApp
{
    internal class Program
    {
        // Entry point. Samples use this to run the Avalonia app.
        public static void Main(string[] args) => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .UseSkia()
                         .LogToTrace()
                         .UseReactiveUI();
    }
}