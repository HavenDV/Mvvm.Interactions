#if HAS_AVALONIA
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace Mvvm.Interactions;

public partial class MessageInteractions
{
    public async Task ShowMessageAsync(string arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var message = GetMessage(arguments);

        _ = await MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentMessage = message,
            ContentTitle = "Message:",
            ButtonDefinitions = ButtonEnum.Ok,
            Icon = Icon.Info,
        }).ShowAsync().ConfigureAwait(true);
    }

    public async Task ShowWarningAsync(string arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var warning = GetWarning(arguments);

        _ = await MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentMessage = warning,
            ContentTitle = "Warning:",
            ButtonDefinitions = ButtonEnum.Ok,
            Icon = Icon.Warning,
        }).ShowAsync().ConfigureAwait(true);
    }

    public async Task ShowErrorAsync(string arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var error = GetError(arguments);

        _ = await MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentMessage = error,
            ContentTitle = "Error:",
            ButtonDefinitions = ButtonEnum.Ok,
            Icon = Icon.Error,
        }).ShowAsync().ConfigureAwait(true);
    }

    public async Task ShowExceptionAsync(Exception arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var exception = GetException(arguments);

        _ = await MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentMessage = $"{exception}",
            ContentTitle = "Exception:",
            ButtonDefinitions = ButtonEnum.Ok,
            Icon = Icon.Error,
        }).ShowAsync().ConfigureAwait(true);
    }

    public async Task<bool> ShowQuestionAsync(QuestionData arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var (title, body) = GetQuestion(arguments);

        var result = await MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentMessage = body,
            ContentTitle = title,
            ButtonDefinitions = ButtonEnum.YesNo,
            EnterDefaultButton = ClickEnum.No,
        }).ShowAsync().ConfigureAwait(true);
        var output = result == ButtonResult.Yes;

        return output;
    }
}
#endif
