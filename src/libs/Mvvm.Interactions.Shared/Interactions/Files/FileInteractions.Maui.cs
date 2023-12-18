#if HAS_MAUI
using System.Diagnostics;
using System.Runtime.Versioning;
using CommunityToolkit.Maui.Storage;

namespace Mvvm.Interactions;

public partial class FileInteractions
{
    private string ToFilter(string filterName, params string[] extensions)
    {
        if (extensions.Length == 0)
        {
            return string.Empty;
        }

        var wildcards = extensions
            .Select(static extension => $"*{extension}")
            .ToArray();
        var filter = $@"{Localize(filterName)} ({string.Join(", ", wildcards)})|{string.Join(";", wildcards)}";

        return filter;
    }

    [SupportedOSPlatform("Android26.0")]
    [SupportedOSPlatform("iOS14.0")]
    [SupportedOSPlatform("MacCatalyst14.0")]
    [SupportedOSPlatform("Tizen")]
    [SupportedOSPlatform("Windows")]
    public async Task<FolderData?> OpenFolderAsync(OpenFolderArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        try
        {
            var folder = await FolderPicker.Default.PickAsync(
                initialPath: arguments.SelectedPath,
                cancellationToken: cancellationToken).ConfigureAwait(true);
            if (!folder.IsSuccessful)
            {
                return null;
            }
            
            return new MauiFolderData(folder.Folder);
        }
        catch (OperationCanceledException)
        {
            return null;
        }
    }

    public async Task<FileData?> OpenFileAsync(OpenFileArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        try
        {
            var result = await FilePicker.Default.PickAsync(PickOptions.Default).ConfigureAwait(true);
            if (result == null)
            {
                return null;
            }

            return new MauiFileData(result);
        }
        catch (OperationCanceledException)
        {
            return null;
        }
    }

    public async Task<FileData[]> OpenFilesAsync(OpenFileArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        try
        {
            var result = await FilePicker.Default.PickAsync(PickOptions.Default).ConfigureAwait(true);
            if (result == null)
            {
                return Array.Empty<FileData>();
            }

            return new FileData[]{ new MauiFileData(result) };
        }
        catch (OperationCanceledException)
        {
            return Array.Empty<FileData>();
        }
    }

    public async Task<FileData?> SaveFileAsync(SaveFileArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        try
        {
            //var location = await FileSaver.Default.SaveAsync("test.txt", stream, cancellationToken);
            var result = await FilePicker.Default.PickAsync(PickOptions.Default).ConfigureAwait(true);
            if (result == null)
            {
                return null;
            }

            return new MauiFileData(result);
        }
        catch (OperationCanceledException)
        {
            return null;
        }
    }

    public Task<FileData> CreateTemporaryFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

        var folder = Path.Combine(
            Path.GetTempPath(),
            "Mvvm.Interactions",
            $"{new Random().Next()}");
        var path = Path.Combine(folder, fileName);

        _ = Directory.CreateDirectory(folder);

        return Task.FromResult<FileData>(new SystemIOApiFileData(path));
    }

    public Task<FileData> OpenPathAsync(string path, CancellationToken cancellationToken = default)
    {
        path = path ?? throw new ArgumentNullException(nameof(path));

        return Task.FromResult<FileData>(new SystemIOApiFileData(path));
    }

#if NET6_0_OR_GREATER
    [System.Runtime.Versioning.UnsupportedOSPlatform("ios")]
    [System.Runtime.Versioning.UnsupportedOSPlatform("tvos")]
    [System.Runtime.Versioning.SupportedOSPlatform("maccatalyst")]
#endif
    public Task LaunchFolderAsync(string path, CancellationToken cancellationToken = default)
    {
        path = path ?? throw new ArgumentNullException(nameof(path));

        _ = Process.Start(new ProcessStartInfo(path)
        {
            UseShellExecute = true,
        });

        return Task.CompletedTask;
    }
}
#endif
