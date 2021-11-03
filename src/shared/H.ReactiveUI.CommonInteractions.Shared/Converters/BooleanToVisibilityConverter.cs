#nullable enable

namespace H.ReactiveUI.Converters;

public class BooleanToVisibilityConverter : BaseConverter<bool, Visibility>
{
    #region Static methods

    public static Visibility Convert(bool value)
    {
        return value
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    public static bool ConvertBack(Visibility value)
    {
        return value == Visibility.Visible;
    }

    #endregion

    #region Constructors

    public BooleanToVisibilityConverter() : 
        base(Convert, ConvertBack)
    {
    }

    #endregion
}
