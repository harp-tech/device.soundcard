using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Avalonia.Data.Converters;

namespace Harp.SoundCard.Design.Converters;

public class TypeDescriptionConverter : IValueConverter
{
    static readonly ConcurrentDictionary<Type, string> _cache = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Type t)
            return value;
        return _cache.GetOrAdd(t, static key =>
            key.GetCustomAttribute<DescriptionAttribute>()?.Description ?? key.Name);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
