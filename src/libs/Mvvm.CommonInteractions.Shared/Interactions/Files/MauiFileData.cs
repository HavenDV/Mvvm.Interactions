#if HAS_MAUI

namespace Mvvm.CommonInteractions.Models;

public class MauiFileData : FileData
{
    #region Properties

    public FileResult Result { get; }

    #endregion

    #region Constructors

    public MauiFileData(FileResult result) : 
        base(result?.FullPath ?? string.Empty)
    {
        Result = result ?? throw new ArgumentNullException(nameof(result));
    }

    #endregion

    #region Methods

    public async override Task<Stream> OpenStreamForReadAsync(
        CancellationToken cancellationToken = default)
    {
        return await Result.OpenReadAsync().ConfigureAwait(false);
    }

    public override Task<Stream> OpenStreamForWriteAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromException<Stream>(new NotImplementedException());
    }

    public override Task LaunchAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromException<Stream>(new NotImplementedException());
    }

    #endregion
}
#endif