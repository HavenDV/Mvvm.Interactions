using System.Diagnostics;
using System.Reactive;
#if WPF
using System.Windows;
#else
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
#endif

namespace H.ReactiveUI;

public class MessageInteractionManager
{
    #region Properties

    private Func<string, string>? LocalizationFunc { get; }

    #endregion

    #region Constructors

    public MessageInteractionManager(Func<string, string>? localizationFunc = null)
    {
        LocalizationFunc = localizationFunc;
    }

    #endregion

    #region Methods

    private string Localize(string value) => LocalizationFunc?.Invoke(value) ?? value;

    public void Register()
    {
        _ = MessageInteractions.Message.RegisterHandler(
#if !WPF
        async
#endif
        context =>
        {
            var message = context.Input;

            message = Localize(message);

            Trace.WriteLine($"Message: {message}");

#if WPF
            _ = MessageBox.Show(
                message,
                "Message:",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
#else
            var dialog = new MessageDialog(message, "Message:");
#endif

            context.SetOutput(Unit.Default);

#if !WPF
            _ = await dialog.ShowAsync();
#endif
        });

        _ = MessageInteractions.Warning.RegisterHandler(
#if !WPF
        async
#endif
        context =>
        {
            var warning = context.Input;

            warning = Localize(warning);

            Trace.WriteLine($"Warning: {warning}");

#if WPF
            _ = MessageBox.Show(
                warning,
                "Warning:",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
#else
            var dialog = new MessageDialog(warning, "Warning:");
#endif

            context.SetOutput(Unit.Default);

#if !WPF
            _ = await dialog.ShowAsync();
#endif
        });

        _ = MessageInteractions.Exception.RegisterHandler(
#if !WPF
        async
#endif
        static context =>
        {
            var exception = context.Input;

            Trace.WriteLine($"Exception: {exception}");

#if WPF
            _ = MessageBox.Show(
                $"{exception}",
                "Exception:",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
#else
            var dialog = new MessageDialog($"{exception}", "Exception:");
#endif

            context.SetOutput(Unit.Default);

#if !WPF
            _ = await dialog.ShowAsync();
#endif
        });

        _ = MessageInteractions.Question.RegisterHandler(
#if !WPF
        async
#endif
        context =>
        {
            var question = context.Input;

            var message = Localize(question.Message);
            var title = Localize(question.Title);
            var body = message;
            if (!string.IsNullOrWhiteSpace(question.AdditionalData))
            {
                body += Environment.NewLine + question.AdditionalData;
            }

            Trace.WriteLine($@"Question: {title}
{body}");

#if WPF
            var result = MessageBox.Show(
                body,
                title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);
            var output = result == MessageBoxResult.Yes;
#else
            var dialog = new ContentDialog
            {
                Title = title,
                Content = body,
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
            };

            var result = await dialog.ShowAsync();
            var output = result == ContentDialogResult.Primary;
#endif

            context.SetOutput(output);
        });

        #endregion
    }
}
