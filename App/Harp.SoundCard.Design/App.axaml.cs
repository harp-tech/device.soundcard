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
            desktop.MainWindow = new MainWindow
            {
                DataContext = new SoundCardViewModel()
            };
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
        about.ShowDialog((Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
            .MainWindow);
    }
}
