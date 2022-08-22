using System.IO;

namespace Mvvm.CommonInteractions.Models;

public class SystemIOApiFolderData : FolderData
{
    #region Constructors

    public SystemIOApiFolderData(string path) :
        base(path)
    {
    }

    #endregion

    #region Methods

    public override Task<IReadOnlyList<FileData>> GetFilesAsync(
        CancellationToken cancellationToken = default)
    {
        var paths = Directory
            .EnumerateFiles(FullPath, "*.*", SearchOption.AllDirectories)
            .ToArray();
        var files = paths
            .Select(static path => new SystemIOApiFileData(path))
            .ToArray();

        return Task.FromResult<IReadOnlyList<FileData>>(files);
    }

    public override Task<FileData> GetFileAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        name = name ?? throw new ArgumentNullException(nameof(name));

        var path = Path.Combine(FullPath, name);
        var file = new SystemIOApiFileData(path);

        return Task.FromResult<FileData>(file);
    }

    #endregion
}