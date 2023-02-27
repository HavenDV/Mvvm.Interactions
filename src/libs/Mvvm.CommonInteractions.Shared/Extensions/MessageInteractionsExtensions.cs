namespace Mvvm.CommonInteractions;

public static class MessageInteractionsExtensions
{
    /// <summary>
    /// Unsupported platforms:<br/>
    /// Avalonia - https://github.com/AvaloniaUI/Avalonia/issues/5290
    /// </summary>
    /// <param name="application"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void CatchUnhandledExceptions(this IMessageInteractions interactions, Application application)
    {
        application = application ?? throw new ArgumentNullException(nameof(application));

#if !HAS_AVALONIA && !HAS_MAUI
#if HAS_WPF
        application.DispatcherUnhandledException += (sender, args) =>
#else
        application.UnhandledException += (sender, args) =>
#endif
        {
            args.Handled = true;

            _ = interactions.ShowExceptionAsync(args.Exception);
        };
#endif
    }
}
