namespace Mvvm.Interactions;

public partial class FileInteractions : InteractionsBase, IFileInteractions
{
    #region Constructors

    public FileInteractions(
        Func<string, string>? localizationFunc = null) : 
        base(localizationFunc)
    {
    }

    #endregion
}
