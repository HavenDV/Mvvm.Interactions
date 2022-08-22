namespace Mvvm.CommonInteractions;

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
