#if !HAS_WPF && !HAS_AVALONIA && !HAS_MAUI
using Windows.Storage;
using Windows.System;

namespace Mvvm.Interactions.Models;

public class StorageApiFileData : FileData
{
    #region Properties

    public StorageFile StorageFile { get; }

    #endregion

    #region Constructors

    public StorageApiFileData(StorageFile file) : 
        base(file?.Path ?? throw new ArgumentNullException(nameof(file)))
    {
        StorageFile = file;
    }

    #endregion

    #region Methods

    public override async Task<Stream> OpenStreamForReadAsync(
        CancellationToken cancellationToken = default)
    {
        return await StorageFile.OpenStreamForReadAsync().ConfigureAwait(false);
    }

    public override async Task<Stream> OpenStreamForWriteAsync(
        CancellationToken cancellationToken = default)
    {
        return await StorageFile.OpenStreamForWriteAsync().ConfigureAwait(false);
    }

    public override async Task LaunchAsync(
        CancellationToken cancellationToken = default)
    {
        _ = await Launcher.LaunchFileAsync(StorageFile);
    }

    #endregion
}
#endif