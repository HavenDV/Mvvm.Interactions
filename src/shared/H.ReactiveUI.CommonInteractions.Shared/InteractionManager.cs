#if WPF
using System.Windows;
#else
using Windows.UI.Xaml;
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
        WebInteractionManager.Register();
    }

    public static void CatchUnhandledExceptions(Application application)
    {
        application = application ?? throw new ArgumentNullException(nameof(application));

#if WPF
        application.DispatcherUnhandledException += static (sender, args) =>
#else
        application.UnhandledException += static (sender, args) =>
#endif
        {
            args.Handled = true;

            _ = MessageInteractions.Exception
                .Handle(args.Exception)
                .Subscribe();
        };
    }

    #endregion
}
