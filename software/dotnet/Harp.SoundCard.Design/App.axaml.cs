using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Harp.SoundCard.Design.ViewModels;
using Harp.SoundCard.Design.Views;

namespace Harp.SoundCard.Design;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var viewModel = new SoundCardViewModel();
            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel
            };

            var portName = GetPortName(desktop.Args);
            if (portName is not null)
                desktop.MainWindow.Opened += (_, _) => viewModel.ConnectToPort(portName);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new SoundCardView
            {
                DataContext = new SoundCardViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    private void NativeMenuItem_OnClick(object sender, EventArgs e)
    { 
        var about = new About() { DataContext = new AboutViewModel() };
        if (Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        if (desktop.MainWindow != null)
        {
            about.ShowDialog(desktop.MainWindow);
        }
    }

    private static string? GetPortName(string[]? args)
    {
        if (args is null)
            return null;

        var index = Array.IndexOf(args, "--port");
        return index >= 0 && index < args.Length - 1 ? args[index + 1] : null;
    }
}
