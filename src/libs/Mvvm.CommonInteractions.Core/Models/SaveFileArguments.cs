namespace Mvvm.CommonInteractions;

/// <summary>
/// 
/// </summary>
public class SaveFileArguments
{
    #region Properties

    /// <summary>
    /// 
    /// </summary>
    public string Extension { get; }

    /// <summary>
    /// 
    /// </summary>
    public string SuggestedFileName { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string FilterName { get; init; } = string.Empty;

    #endregion

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="extension"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public SaveFileArguments(string extension)
    {
        Extension = extension ?? throw new ArgumentNullException(nameof(extension));
    }

    #endregion
}