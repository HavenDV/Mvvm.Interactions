namespace H.ReactiveUI;

public class BaseManager
{
    #region Properties

    private Func<string, string>? LocalizationFunc { get; }

    #endregion

    #region Constructors

    public BaseManager(Func<string, string>? localizationFunc = null)
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
