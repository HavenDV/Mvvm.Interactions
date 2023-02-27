#if !HAS_AVALONIA && !HAS_WPF && !HAS_MAUI
using System.Diagnostics;
using Windows.Foundation.Metadata;

namespace Mvvm.CommonInteractions;

public partial class MessageInteractions
{
#if HAS_WINUI
    public static Window? Window { get; set; }
#endif

    [Conditional("HAS_WINUI")]
    private void SetXamlRoot(ContentDialog dialog)
    {
#if HAS_WINUI
        if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
        {
            dialog.XamlRoot = Window?.Content.XamlRoot;
        }
#endif
    }

    public async Task ShowMessageAsync(string arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var message = GetMessage(arguments);

#if HAS_UNO_MOBILE
        using
#endif
        var dialog = new ContentDialog
        {
            Title = "Message:",
            Content = message,
            CloseButtonText = "Close",
        };
        SetXamlRoot(dialog);

        _ = await dialog.ShowAsync();
    }

    public async Task ShowWarningAsync(string arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var warning = GetWarning(arguments);

#if HAS_UNO_MOBILE
        using
#endif
        var dialog = new ContentDialog
        {
            Title = "Warning:",
            Content = warning,
            CloseButtonText = "Close",
        };
        SetXamlRoot(dialog);

        _ = await dialog.ShowAsync();
    }

    public async Task ShowErrorAsync(string arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var error = GetError(arguments);

#if HAS_UNO_MOBILE
        using
#endif
        var dialog = new ContentDialog
        {
            Title = "Error:",
            Content = error,
            CloseButtonText = "Close",
        };
        SetXamlRoot(dialog);

        _ = await dialog.ShowAsync();
    }

    public async Task ShowExceptionAsync(Exception arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var exception = GetException(arguments);

#if HAS_UNO_MOBILE
        using
#endif
        var dialog = new ContentDialog
        {
            Title = "Exception:",
            Content = $"{exception}",
            CloseButtonText = "Close",
        };
        SetXamlRoot(dialog);

        _ = await dialog.ShowAsync();
    }

    public async Task<bool> ShowQuestionAsync(QuestionData arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var (title, body) = GetQuestion(arguments);

#if HAS_UNO_MOBILE
        using
#endif
        var dialog = new ContentDialog
        {
            Title = title,
            Content = body,
            PrimaryButtonText = "Yes",
            CloseButtonText = "No",
        };
        SetXamlRoot(dialog);

        var result = await dialog.ShowAsync();
        var output = result == ContentDialogResult.Primary;

        return output;
    }
}
#endif