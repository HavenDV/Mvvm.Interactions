namespace H.ReactiveUI.Apps.Uno.Extensions;

// Workaround for https://github.com/unoplatform/uno/issues/9241
public static partial class DependencyObjectExtensions
{
    public static void SetBinding(
        this DependencyObject dependencyObject,
        string dependencyProperty,
        BindingBase binding)
    {
        (dependencyObject as UIElement)?.SetBinding(dependencyProperty, binding);
    }
}