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
}
