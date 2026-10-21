using Avalonia.Controls;
using Avalonia.Interactivity;
using Harp.SoundCard.Design.ViewModels;

namespace Harp.SoundCard.Design.Views;

public partial class ImportOptionsDialog : Window
{
    public ImportOptionsDialog()
    {
        InitializeComponent();
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e)
    {
        Close(null);
    }

    private void OnImportClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ImportOptionsDialogViewModel vm)
            Close(vm.ToResult());
        else
            Close(null);
    }
}
