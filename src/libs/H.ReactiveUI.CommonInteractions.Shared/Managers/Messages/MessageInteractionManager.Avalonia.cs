#if HAS_AVALONIA
using System.Reactive;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;

namespace H.ReactiveUI;

public partial class MessageInteractionManager : BaseManager
{
    public void Register()
    {
        _ = MessageInteractions.Message.RegisterHandler(async context =>
        {
            var message = GetMessage(context.Input);

            _ = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ContentMessage = message,
                ContentTitle = "Message:",
                ButtonDefinitions = ButtonEnum.Ok,
                Icon = Icon.Info,
            }).Show().ConfigureAwait(true);

            context.SetOutput(Unit.Default);
        });

        _ = MessageInteractions.Warning.RegisterHandler(async context =>
        {
            var warning = GetWarning(context.Input);

            _ = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ContentMessage = warning,
                ContentTitle = "Warning:",
                ButtonDefinitions = ButtonEnum.Ok,
                Icon = Icon.Warning,
            }).Show().ConfigureAwait(true);

            context.SetOutput(Unit.Default);
        });

        _ = MessageInteractions.Exception.RegisterHandler(async context =>
        {
            var exception = GetException(context.Input);

            _ = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ContentMessage = $"{exception}",
                ContentTitle = "Exception:",
                ButtonDefinitions = ButtonEnum.Ok,
                Icon = Icon.Error,
            }).Show().ConfigureAwait(true);

            context.SetOutput(Unit.Default);
        });

        _ = MessageInteractions.Question.RegisterHandler(async context =>
        {
            var (title, body) = GetQuestion(context.Input);

            var result = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ContentMessage = body,
                ContentTitle = title,
                ButtonDefinitions = ButtonEnum.YesNo,
                EnterDefaultButton = ClickEnum.No,
            }).Show().ConfigureAwait(true);
            var output = result == ButtonResult.Yes;

            context.SetOutput(output);
        });
    }
}
#endif