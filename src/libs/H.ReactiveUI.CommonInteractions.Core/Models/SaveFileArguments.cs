namespace H.ReactiveUI;

public class SaveFileArguments
{
    #region Properties

    public string Extension { get; }

    public string SuggestedFileName { get; init; } = string.Empty;
    public string FilterName { get; init; } = string.Empty;

    #endregion

    #region Constructors

    public SaveFileArguments(string extension)
    {
        Extension = extension ?? throw new ArgumentNullException(nameof(extension));
    }

    #endregion
}