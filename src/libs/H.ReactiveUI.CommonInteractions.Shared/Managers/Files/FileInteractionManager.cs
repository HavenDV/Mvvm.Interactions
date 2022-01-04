namespace H.ReactiveUI;

public partial class FileInteractionManager : BaseManager
{
    #region Constructors

    public FileInteractionManager(
        Func<string, string>? localizationFunc = null) : 
        base(localizationFunc)
    {
    }

    #endregion
}
