#if !HAS_AVALONIA && !HAS_WPF
using System.Reactive;
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

            var dialog = new ContentDialog
            {
                Title = "Message:",
                Content = message,
                CloseButtonText = "Close",
            };
#if HAS_WINUI
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
            {
                dialog.XamlRoot = Window?.Content.XamlRoot;
            }
#endif
            _ = await dialog.ShowAsync();

            context.SetOutput(Unit.Default);
        });

        _ = MessageInteractions.Warning.RegisterHandler(async context =>
        {
            var warning = GetWarning(context.Input);

            var dialog = new ContentDialog
            {
                Title = "Warning:",
                Content = warning,
                CloseButtonText = "Close",
            };
#if HAS_WINUI
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
            {
                dialog.XamlRoot = Window?.Content.XamlRoot;
            }
#endif
            _ = await dialog.ShowAsync();

            context.SetOutput(Unit.Default);
        });

        _ = MessageInteractions.Error.RegisterHandler(async context =>
        {
            var error = GetError(context.Input);

            var dialog = new ContentDialog
            {
                Title = "Error:",
                Content = error,
                CloseButtonText = "Close",
            };
#if HAS_WINUI
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
            {
                dialog.XamlRoot = Window?.Content.XamlRoot;
            }
#endif
            _ = await dialog.ShowAsync();

            context.SetOutput(Unit.Default);
        });

        _ = MessageInteractions.Exception.RegisterHandler(async static context =>
        {
            var exception = GetException(context.Input);

            var dialog = new ContentDialog
            {
                Title = "Exception:",
                Content = $"{exception}",
                CloseButtonText = "Close",
            };
#if HAS_WINUI
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
            {
                dialog.XamlRoot = Window?.Content.XamlRoot;
            }
#endif
            _ = await dialog.ShowAsync();

            context.SetOutput(Unit.Default);
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