using System.Reactive;
#if HAS_WPF
using System.Diagnostics;
#else
using Windows.System;
#endif

namespace H.ReactiveUI;

public static class WebInteractionManager
{
    #region Methods

    public static void Register()
    {
        _ = WebInteractions.OpenUrl.RegisterHandler(
#if !HAS_WPF
        async
#endif
        static context =>
        {
            var url = context.Input;

#if HAS_WPF
            _ = Process.Start(new ProcessStartInfo(url)
            {
                UseShellExecute = true,
            });
#else
            _ = await Launcher.LaunchUriAsync(new Uri(url))
    #if HAS_UNO
                .ConfigureAwait(true)
    #endif
                ;
#endif

            context.SetOutput(Unit.Default);
        });
    }

    #endregion
}
