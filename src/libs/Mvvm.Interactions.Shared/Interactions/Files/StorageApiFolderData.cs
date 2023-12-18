#if !HAS_WPF && !HAS_AVALONIA && !HAS_MAUI
using Windows.Storage;

namespace Mvvm.Interactions.Models;

public class StorageApiFolderData : FolderData
{
    #region Properties

    public StorageFolder StorageFolder { get; }

    #endregion

    #region Constructors

    public StorageApiFolderData(StorageFolder folder) : 
        base(folder?.Path ?? throw new ArgumentNullException(nameof(folder)))
    {
        StorageFolder = folder;
    }

    #endregion

    #region Methods

    public override async Task<IReadOnlyList<FileData>> GetFilesAsync(
        CancellationToken cancellationToken = default)
    {
        var files = await StorageFolder.GetFilesAsync();

        return files
            .Select(static file => new StorageApiFileData(file))
            .ToArray();
    }

    public override async Task<FileData> GetFileAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        name = name ?? throw new ArgumentNullException(nameof(name));

        var file = await StorageFolder.GetFileAsync(name);

        return new StorageApiFileData(file);
    }

    #endregion
}
#endif