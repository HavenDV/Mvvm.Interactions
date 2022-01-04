#if !HAS_AVALONIA && !HAS_WPF
using System.Reactive;
using Windows.UI.Popups;
using Windows.Foundation.Metadata;

namespace H.ReactiveUI;

public partial class MessageInteractionManager : BaseManager
{
#if HAS_WINUI
    public static Window? Window { get; set; }
#endif

    public void Register()
    {
        _ = MessageInteractions.Message.RegisterHandler(async context =>
        {
            var message = GetMessage(context.Input);

            var dialog = new MessageDialog(message, "Message:")
#if HAS_WINUI && !HAS_UNO
                .Initialize()
#endif
                ;

            context.SetOutput(Unit.Default);

            _ = await dialog.ShowAsync();
        });

        _ = MessageInteractions.Warning.RegisterHandler(async context =>
        {
            var warning = GetWarning(context.Input);

            var dialog = new MessageDialog(warning, "Warning:")
#if HAS_WINUI && !HAS_UNO
                .Initialize()
#endif
                ;

            context.SetOutput(Unit.Default);

            _ = await dialog.ShowAsync();
        });

        _ = MessageInteractions.Exception.RegisterHandler(async static context =>
        {
            var exception = GetException(context.Input);

            var dialog = new MessageDialog($"{exception}", "Exception:")
#if HAS_WINUI && !HAS_UNO
                .Initialize()
#endif
                ;

            context.SetOutput(Unit.Default);

            _ = await dialog.ShowAsync();
        });

        _ = MessageInteractions.Question.RegisterHandler(async context =>
        {
            var (title, body) = GetQuestion(context.Input);

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

            context.SetOutput(output);
        });
    }
}
#endif