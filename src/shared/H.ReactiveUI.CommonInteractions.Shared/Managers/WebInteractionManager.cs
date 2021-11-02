using System.Reactive;
#if HAS_WPF
using System.Diagnostics;
#else
using Windows.System;
#endif

namespace H.ReactiveUI;

public partial class WebInteractionManager : BaseManager
{
    #region Constructors

    public WebInteractionManager(
        Func<string, string>? localizationFunc = null) :
        base(localizationFunc)
    {
    }

    #endregion

    #region Methods

#pragma warning disable CA1822 // Mark members as static
    public void Register()
#pragma warning restore CA1822 // Mark members as static
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
