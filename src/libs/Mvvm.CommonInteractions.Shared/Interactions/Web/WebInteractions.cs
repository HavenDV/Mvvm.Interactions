namespace Mvvm.CommonInteractions;

public partial class WebInteractions : InteractionsBase, IWebInteractions
{
    #region Constructors

    public WebInteractions(
        Func<string, string>? localizationFunc = null) :
        base(localizationFunc)
    {
    }

    #endregion
}
