using System.Diagnostics;
using System.IO;

namespace Mvvm.CommonInteractions.Models;

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

#if NET6_0_OR_GREATER
    [System.Runtime.Versioning.UnsupportedOSPlatform("ios")]
    [System.Runtime.Versioning.UnsupportedOSPlatform("tvos")]
    [System.Runtime.Versioning.SupportedOSPlatform("maccatalyst")]
#endif
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