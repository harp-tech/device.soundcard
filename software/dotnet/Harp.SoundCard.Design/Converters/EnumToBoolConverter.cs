using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Harp.SoundCard.Design.Converters;

public class EnumToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;
        return value.ToString() == parameter.ToString();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is true && parameter?.ToString() is string name)
            return Enum.Parse(targetType, name);
        return AvaloniaProperty.UnsetValue;
    }
}
