#if !HAS_WPF && !HAS_AVALONIA
using System.Reactive;
using System.Diagnostics;
using Windows.System;

namespace H.ReactiveUI;

#pragma warning disable CA1822 // Mark members as static

public partial class WebInteractionManager
{
    public void Register()
    {
        _ = WebInteractions.OpenUrl.RegisterHandler(async static context =>
        {
            var url = context.Input;

            _ = await Launcher.LaunchUriAsync(new Uri(url))
#if HAS_UNO
                .ConfigureAwait(true)
#endif
                ;

            context.SetOutput(Unit.Default);
        });
    }
}
#endif