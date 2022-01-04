#if HAS_WPF
using System.Reactive;

namespace H.ReactiveUI;

public partial class MessageInteractionManager : BaseManager
{
    public void Register()
    {
        _ = MessageInteractions.Message.RegisterHandler(context =>
        {
            var message = GetMessage(context.Input);

            _ = MessageBox.Show(
                message,
                "Message:",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            context.SetOutput(Unit.Default);
        });

        _ = MessageInteractions.Warning.RegisterHandler(context =>
        {
            var warning = GetWarning(context.Input);

            _ = MessageBox.Show(
                warning,
                "Warning:",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            context.SetOutput(Unit.Default);
        });

        _ = MessageInteractions.Error.RegisterHandler(context =>
        {
            var error = GetError(context.Input);

            _ = MessageBox.Show(
                error,
                "Error:",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            context.SetOutput(Unit.Default);
        });

        _ = MessageInteractions.Exception.RegisterHandler(static context =>
        {
            var exception = GetException(context.Input);

            _ = MessageBox.Show(
                $"{exception}",
                "Exception:",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            context.SetOutput(Unit.Default);
        });

        _ = MessageInteractions.Question.RegisterHandler(context =>
        {
            var (title, body) = GetQuestion(context.Input);

            var result = MessageBox.Show(
                body,
                title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);
            var output = result == MessageBoxResult.Yes;

            context.SetOutput(output);
        });
    }
}
#endif