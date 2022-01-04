#if !HAS_WPF && !HAS_AVALONIA
using System.Reactive;
using System.Diagnostics;
using Windows.System;

namespace H.ReactiveUI;

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