using Avalonia.Controls;
using Harp.SoundCard.Design.ViewModels;

namespace Harp.SoundCard.Design.Views;

public partial class SoundGenerationView : UserControl
{
    public SoundGenerationView()
    {
        InitializeComponent();

        // Subscribe if DataContext is already set
        SubscribeToPlot();

        // Re-subscribe if DataContext changes
        this.DataContextChanged += (_, _) => SubscribeToPlot();
    }

    private void SubscribeToPlot()
    {
        if (DataContext is not SoundGenerationViewModel vm)
            return;

        vm.Plot ??= SignalPlot.Plot;

        SignalPlot.Plot.Axes.SetLimitsY(-1.0, 1.0);

        vm.PlotUpdated += () =>
        {
            SignalPlot.Plot.Axes.AutoScale();
            SignalPlot.Refresh();
        };
    }
}
