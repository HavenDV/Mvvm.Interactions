namespace Mvvm.CommonInteractions;

/// <summary>
/// 
/// </summary>
public class FolderData
{
    #region Properties

    /// <summary>
    /// 
    /// </summary>
    public string FullPath { get; init; } = string.Empty;

    #endregion

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    public FolderData()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public FolderData(string path)
    {
        FullPath = path ?? throw new ArgumentNullException(nameof(path));
    }

    #endregion

    #region Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual Task<IReadOnlyList<FileData>> GetFilesAsync(
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual Task<FileData> GetFileAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #endregion
}
