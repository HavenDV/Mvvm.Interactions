namespace Mvvm.CommonInteractions;

public abstract class InteractionsBase
{
    #region Properties

    private Func<string, string>? LocalizationFunc { get; }

    #endregion

    #region Constructors

    protected InteractionsBase(Func<string, string>? localizationFunc = null)
    {
        LocalizationFunc = localizationFunc;
    }

    #endregion

    #region Methods

    protected string Localize(string value)
    {
        return LocalizationFunc?.Invoke(value) ?? value;
    }

    #endregion
}
