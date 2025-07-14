using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Styling;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Harp.SoundCard.Design.ViewModels;

public class ViewModelBase : ReactiveObject
{
    [Reactive] public bool IsDarkMode { get; set; }
    [Reactive] public IBrush IconColor { get; set; }

    public ViewModelBase()
    {
        // Get the current theme on Application.Current!.RequestedTheme
        var currentTheme = Application.Current!.RequestedThemeVariant;
        IsDarkMode = currentTheme == ThemeVariant.Dark ||
                     (currentTheme == ThemeVariant.Default && IsSystemInDarkMode());

        // set initial color
        IconColor = IsDarkMode ? Brushes.White : Brushes.Black;

        // update icon color when IsDarkMode changes
        this.WhenAnyValue(x => x.IsDarkMode)
            .Subscribe(isDarkMode =>
            {
                IconColor = isDarkMode ? Brushes.White : Brushes.Black;
            });

        // Subscribe to changes in IsDarkMode
        this.WhenAnyValue(x => x.IsDarkMode)
            .Skip(1) // Skip the initial value to avoid unnecessary theme change on initialization
            .Subscribe(isDarkMode => Application.Current.RequestedThemeVariant = isDarkMode ? ThemeVariant.Dark : ThemeVariant.Light);
    }

    private bool IsSystemInDarkMode()
    {
        // detect using avalonia if system is in dark mode
        var colors = Application.Current!.PlatformSettings!.GetColorValues();
        return colors.ThemeVariant == PlatformThemeVariant.Dark;
    }
}
