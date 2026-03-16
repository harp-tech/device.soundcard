using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Harp.SoundCard.Design.Converters;

public class VisualStatusColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var status = value as bool?;
        return status switch
        {
            true => new SolidColorBrush(Colors.Green),
            false => new SolidColorBrush(Colors.Red),
            _ => new SolidColorBrush(Colors.Gray)
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
