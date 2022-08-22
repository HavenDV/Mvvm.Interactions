#if HAS_WPF
namespace Mvvm.CommonInteractions;

public partial class MessageInteractions
{
    public Task ShowMessageAsync(string arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var message = GetMessage(arguments);

        _ = MessageBox.Show(
            message,
            "Message:",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        return Task.CompletedTask;
    }

    public Task ShowWarningAsync(string arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var warning = GetWarning(arguments);

        _ = MessageBox.Show(
            warning,
            "Warning:",
            MessageBoxButton.OK,
            MessageBoxImage.Warning);

        return Task.CompletedTask;
    }

    public Task ShowErrorAsync(string arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var error = GetError(arguments);

        _ = MessageBox.Show(
            error,
            "Error:",
            MessageBoxButton.OK,
            MessageBoxImage.Error);

        return Task.CompletedTask;
    }

    public Task ShowExceptionAsync(Exception arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var exception = GetException(arguments);

        _ = MessageBox.Show(
            $"{exception}",
            "Exception:",
            MessageBoxButton.OK,
            MessageBoxImage.Error);

        return Task.CompletedTask;
    }

    public Task<bool> ShowQuestionAsync(QuestionData arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var (title, body) = GetQuestion(arguments);

        var result = MessageBox.Show(
            body,
            title,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question,
            MessageBoxResult.No);
        var output = result == MessageBoxResult.Yes;

        return Task.FromResult(output);
    }
}
#endif