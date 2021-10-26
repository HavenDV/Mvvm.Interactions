using System.Reactive;
#if WPF
using System.Diagnostics;
#else
using Windows.System;
#endif

namespace H.ReactiveUI;

public class InteractionManager
{
    #region Properties

    private MessageInteractionManager MessageInteractionManager { get; }
    private FileInteractionManager FileInteractionManager { get; }

    #endregion

    #region Constructors

    public InteractionManager(Func<string, string>? localizationFunc = null)
    {
        MessageInteractionManager = new MessageInteractionManager(localizationFunc);
        FileInteractionManager = new FileInteractionManager(localizationFunc);
    }

    #endregion

    #region Methods

    public void Register()
    {
        MessageInteractionManager.Register();
        FileInteractionManager.Register();
#if WPF
        _ = WebInteractions.OpenUrl.RegisterHandler(static context =>
        {
            var url = context.Input;

            _ = Process.Start(new ProcessStartInfo(url)
            {
                UseShellExecute = true,
            });

            context.SetOutput(Unit.Default);
        });
#else
        _ = WebInteractions.OpenUrl.RegisterHandler(static async context =>
        {
            var url = context.Input;

            _ = await Launcher.LaunchUriAsync(new Uri(url))
#if Uno
                .ConfigureAwait(false)
#endif
                ;

            context.SetOutput(Unit.Default);
        });
#endif

        #endregion
    }
}
