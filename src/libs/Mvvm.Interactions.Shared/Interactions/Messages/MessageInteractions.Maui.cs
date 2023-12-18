#if HAS_MAUI
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Mvvm.Interactions;

public partial class MessageInteractions
{
    public async Task ShowMessageAsync(string arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var message = GetMessage(arguments);
        
        await Toast.Make(
            message: $"Message: {message}",
            duration: ToastDuration.Short,
            textSize: 14D).Show(cancellationToken).ConfigureAwait(true);
    }

    public async Task ShowWarningAsync(string arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var warning = GetWarning(arguments);

        await Toast.Make(
            message: $"Warning: {warning}",
            duration: ToastDuration.Short,
            textSize: 14D).Show(cancellationToken).ConfigureAwait(true);
    }

    public async Task ShowErrorAsync(string arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var error = GetError(arguments);

        await Toast.Make(
            message: $"Error: {error}",
            duration: ToastDuration.Long,
            textSize: 14D).Show(cancellationToken).ConfigureAwait(true);
    }

    public async Task ShowExceptionAsync(Exception arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var exception = GetException(arguments);

        await Toast.Make(
            message: $"Exception: {exception}",
            duration: ToastDuration.Long,
            textSize: 14D).Show(cancellationToken).ConfigureAwait(true);
    }

    public async Task<bool> ShowQuestionAsync(QuestionData arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var (title, body) = GetQuestion(arguments);

        await Toast.Make(
            message: $"{title}: {body}",
            duration: ToastDuration.Long,
            textSize: 14D).Show(cancellationToken).ConfigureAwait(true);

        return false;
    }
}
#endif