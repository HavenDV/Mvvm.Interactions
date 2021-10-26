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

    #endregion
}
