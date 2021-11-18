using System.Diagnostics;

namespace H.ReactiveUI.CommonInteractions.Models;

public class SystemIOApiFileData : FileData
{
    #region Constructors

    public SystemIOApiFileData(string path) :
        base(path)
    {
    }

    #endregion

    #region Methods

    public override Task<Stream> OpenStreamForReadAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Stream>(File.OpenRead(FullPath));
    }

    public override Task<Stream> OpenStreamForWriteAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Stream>(File.OpenWrite(FullPath));
    }

    public override Task LaunchAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var process = Process.Start(new ProcessStartInfo(FullPath)
            {
                UseShellExecute = true,
            });

            return Task.CompletedTask;
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch (Exception exception)
#pragma warning restore CA1031 // Do not catch general exception types
        {
            return Task.FromException(exception);
        }
    }

    #endregion
}