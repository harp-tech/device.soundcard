using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Harp.SoundCard.Design.Converters;

public class EnumDisplayConverter : IValueConverter
{
    public Dictionary<string, string> Mappings { get; set; } = new Dictionary<string, string>();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return null;
        var key = value.ToString();
        return Mappings!.GetValueOrDefault(key, key);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
