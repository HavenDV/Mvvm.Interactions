namespace H.ReactiveUI;

public class InteractionManager
{
    #region Properties

    private MessageInteractionManager MessageInteractionManager { get; }
    private FileInteractionManager FileInteractionManager { get; }
    private WebInteractionManager WebInteractionManager { get; }

    #endregion

    #region Constructors

    public InteractionManager(Func<string, string>? localizationFunc = null)
    {
        MessageInteractionManager = new MessageInteractionManager(localizationFunc);
        FileInteractionManager = new FileInteractionManager(localizationFunc);
        WebInteractionManager = new WebInteractionManager(localizationFunc);
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

#if HAS_WPF
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
