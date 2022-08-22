#if HAS_AVALONIA
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;

namespace Mvvm.CommonInteractions;

public partial class MessageInteractions
{
    public async Task ShowMessageAsync(string arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var message = GetMessage(arguments);

        _ = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
        {
            ContentMessage = message,
            ContentTitle = "Message:",
            ButtonDefinitions = ButtonEnum.Ok,
            Icon = Icon.Info,
        }).Show().ConfigureAwait(true);
    }

    public async Task ShowWarningAsync(string arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var warning = GetWarning(arguments);

        _ = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
        {
            ContentMessage = warning,
            ContentTitle = "Warning:",
            ButtonDefinitions = ButtonEnum.Ok,
            Icon = Icon.Warning,
        }).Show().ConfigureAwait(true);
    }

    public async Task ShowErrorAsync(string arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var error = GetError(arguments);

        _ = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
        {
            ContentMessage = error,
            ContentTitle = "Error:",
            ButtonDefinitions = ButtonEnum.Ok,
            Icon = Icon.Error,
        }).Show().ConfigureAwait(true);
    }

    public async Task ShowExceptionAsync(Exception arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var exception = GetException(arguments);

        _ = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
        {
            ContentMessage = $"{exception}",
            ContentTitle = "Exception:",
            ButtonDefinitions = ButtonEnum.Ok,
            Icon = Icon.Error,
        }).Show().ConfigureAwait(true);
    }

    public async Task<bool> ShowQuestionAsync(QuestionData arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var (title, body) = GetQuestion(arguments);

        var result = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
        {
            ContentMessage = body,
            ContentTitle = title,
            ButtonDefinitions = ButtonEnum.YesNo,
            EnterDefaultButton = ClickEnum.No,
        }).Show().ConfigureAwait(true);
        var output = result == ButtonResult.Yes;

        return output;
    }
}
#endif
