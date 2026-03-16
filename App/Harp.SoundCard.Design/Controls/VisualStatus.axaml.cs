using Avalonia;
using Avalonia.Controls;

namespace Harp.SoundCard.Design.Controls;

public partial class VisualStatus : ContentControl
{
    public static readonly StyledProperty<bool?> StatusProperty =
    AvaloniaProperty.Register<VisualStatus, bool?>(nameof(Status), null);

    public bool? Status
    {
        get => GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public VisualStatus()
    {
        InitializeComponent();
    }
}
