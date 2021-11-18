using System.Diagnostics;
using System.Reactive;
#if HAS_WPF
#else
using Windows.UI.Popups;
using Windows.Foundation.Metadata;
#endif

namespace H.ReactiveUI;

public partial class MessageInteractionManager : BaseManager
{
    #region Static properties

    public static Window? Window { get; set; }

    #endregion

    #region Constructors

    public MessageInteractionManager(
        Func<string, string>? localizationFunc = null) :
        base(localizationFunc)
    {
    }

    #endregion

    #region Methods

    public void Register()
    {
        _ = MessageInteractions.Message.RegisterHandler(
#if !HAS_WPF
        async
#endif
        context =>
        {
            var message = context.Input;

            message = Localize(message);

            Trace.WriteLine($"Message: {message}");

#if HAS_WPF
            _ = MessageBox.Show(
                message,
                "Message:",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
#else
            var dialog = new MessageDialog(message, "Message:")
#if HAS_WINUI && !HAS_UNO
                .Initialize()
#endif
                ;
#endif

            context.SetOutput(Unit.Default);

#if !HAS_WPF
            _ = await dialog.ShowAsync();
#endif
        });

        _ = MessageInteractions.Warning.RegisterHandler(
#if !HAS_WPF
        async
#endif
        context =>
        {
            var warning = context.Input;

            warning = Localize(warning);

            Trace.WriteLine($"Warning: {warning}");

#if HAS_WPF
            _ = MessageBox.Show(
                warning,
                "Warning:",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
#else
            var dialog = new MessageDialog(warning, "Warning:")
#if HAS_WINUI && !HAS_UNO
                .Initialize()
#endif
                ;
#endif

            context.SetOutput(Unit.Default);

#if !HAS_WPF
            _ = await dialog.ShowAsync();
#endif
        });

        _ = MessageInteractions.Exception.RegisterHandler(
#if !HAS_WPF
        async
#endif
        static context =>
        {
            var exception = context.Input;

            Trace.WriteLine($"Exception: {exception}");

#if HAS_WPF
            _ = MessageBox.Show(
                $"{exception}",
                "Exception:",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
#else
            var dialog = new MessageDialog($"{exception}", "Exception:")
#if HAS_WINUI && !HAS_UNO
                .Initialize()
#endif
                ;
#endif

            context.SetOutput(Unit.Default);

#if !HAS_WPF
            _ = await dialog.ShowAsync();
#endif
        });

        _ = MessageInteractions.Question.RegisterHandler(
#if !HAS_WPF
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

#if HAS_WPF
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
#if HAS_WINUI
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
            {
                dialog.XamlRoot = Window?.Content.XamlRoot;
            }
#endif

            var result = await dialog.ShowAsync();
            var output = result == ContentDialogResult.Primary;
#endif

            context.SetOutput(output);
        });

        #endregion
    }
}
