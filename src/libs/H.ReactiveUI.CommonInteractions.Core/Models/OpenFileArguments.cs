namespace H.ReactiveUI;

/// <summary>
/// 
/// </summary>
public class OpenFileArguments
{
    #region Properties

    /// <summary>
    /// 
    /// </summary>
    public string SuggestedFileName { get; init; } = string.Empty;

    /// <summary>
    /// Dot is required. <br/>
    /// Example: '.txt'
    /// </summary>
    public string[] Extensions { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Example: 'Text files'
    /// </summary>
    public string FilterName { get; init; } = string.Empty;

    #endregion
}
