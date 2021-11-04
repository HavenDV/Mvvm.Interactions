namespace H.ReactiveUI;

public class SaveFileArguments
{
    #region Properties

    public string SuggestedFileName { get; init; } = string.Empty;
    public string Extension { get; init; } = string.Empty;
    public string FilterName { get; init; } = string.Empty;

    #endregion
}