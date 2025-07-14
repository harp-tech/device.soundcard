using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Bonsai.Harp;

namespace Harp.SoundCard.Design.Converters;

/// <summary>
/// Converts between EnableFlag enum values and boolean values for two-way binding
/// </summary>
public class EnableFlagConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Convert from enum to bool (for IsChecked)
        if (value == null)
            return false;

        return value.ToString().Contains("Enable");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Convert from bool (IsChecked) to enum
        if (value is not bool isChecked || parameter == null)
            return null;

        // Create the appropriate enum value based on the checkbox state
        return isChecked ? EnableFlag.Enable : EnableFlag.Disable;
    }
}
