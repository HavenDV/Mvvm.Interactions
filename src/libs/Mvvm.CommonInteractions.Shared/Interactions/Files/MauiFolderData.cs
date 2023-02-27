#if HAS_MAUI
using CommunityToolkit.Maui.Core.Primitives;

namespace Mvvm.CommonInteractions.Models;

public class MauiFolderData : FolderData
{
    #region Properties

    public Folder Folder { get; }

    #endregion

    #region Constructors

    public MauiFolderData(Folder folder) : 
        base(folder?.Path ?? string.Empty)
    {
        Folder = folder ?? throw new ArgumentNullException(nameof(folder));
    }

    #endregion

    #region Methods

    public override Task<IReadOnlyList<FileData>> GetFilesAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromException<IReadOnlyList<FileData>>(new NotImplementedException());
    }

    public override Task<FileData> GetFileAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        name = name ?? throw new ArgumentNullException(nameof(name));

        return Task.FromException<FileData>(new NotImplementedException());
    }

    #endregion
}
#endif