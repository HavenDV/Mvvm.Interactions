#if HAS_AVALONIA
using Avalonia.Data.Converters;
using System.Globalization;
using DependencyProperty = Avalonia.AvaloniaProperty;
#elif HAS_WINUI
using Microsoft.UI.Xaml.Data;
#elif HAS_WPF
using System.Windows.Data;
using System.Globalization;
#elif HAS_MAUI
using System.Globalization;
using DependencyProperty = Microsoft.Maui.Controls.BindableProperty;
#else
using Windows.UI.Xaml.Data;
#endif

#nullable enable

namespace Mvvm.Converters;

public class BaseConverter<TFrom, TTo> : IValueConverter
{
    #region Properties

    protected Func<TFrom, object?, TTo> ConvertFunc { get; set; }
    protected Func<TTo, object?, TFrom>? ConvertBackFunc { get; set; }

    public TTo? DefaultConvertValue { get; set; }
    public TFrom? DefaultConvertBackValue { get; set; }

    #endregion

    #region Constructors

    public BaseConverter(
        Func<TFrom, object?, TTo> convertFunc,
        Func<TTo, object?, TFrom>? convertBackFunc = null)
    {
        ConvertFunc = convertFunc ?? throw new ArgumentNullException(nameof(convertFunc));
        ConvertBackFunc = convertBackFunc;
    }

    public BaseConverter(
        Func<TFrom, TTo> convertFunc,
        Func<TTo, TFrom>? convertBackFunc = null)
    {
        convertFunc = convertFunc ?? throw new ArgumentNullException(nameof(convertFunc));

        ConvertFunc = (value, _) => convertFunc(value);
        if (convertBackFunc != null)
        {
            ConvertBackFunc = (value, _) => convertBackFunc(value);
        }
    }

    #endregion

    #region Methods

#if HAS_WPF || HAS_AVALONIA || HAS_MAUI
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
#else
    public object? Convert(object? value, Type targetType, object? parameter, string language)
#endif
    {
        return TryConvert(value, targetType, parameter, out var result)
            ? result
            : DependencyProperty.UnsetValue;
    }

#if HAS_WPF || HAS_AVALONIA || HAS_MAUI
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
#else
    public object? ConvertBack(object? value, Type targetType, object? parameter, string language)
#endif
    {
        return TryConvert(value, targetType, parameter, out var result)
            ? result
            : DependencyProperty.UnsetValue;
    }

    public bool TryConvert(object? from, Type toType, object? conversionHint, out object? result)
    {
        if (toType == typeof(TTo) || toType == typeof(object))
        {
            if (from == null)
            {
                result = DefaultConvertValue;
                return true;
            }

            if (from is TFrom fromCasted)
            {
                result = ConvertFunc(fromCasted, conversionHint);
                return true;
            }

            if (typeof(TFrom).IsEnum)
            {
                result = ConvertFunc((TFrom)from, conversionHint);
                return true;
            }
        }

        if (ConvertBackFunc != null &&
            toType == typeof(TFrom))
        {
            if (from == null)
            {
                result = DefaultConvertBackValue;
                return true;
            }

            if (from is TTo toCasted)
            {
                result = ConvertBackFunc(toCasted, conversionHint);
                return true;
            }

            if (typeof(TTo).IsEnum)
            {
                result = ConvertBackFunc((TTo)from, conversionHint);
                return true;
            }
        }

        result = null;
        return false;
    }

#endregion
}
