#if HAS_WPF
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

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

    public Task<FolderData?> OpenFolderAsync(OpenFolderArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        using var dialog = new System.Windows.Forms.FolderBrowserDialog
        {
            RootFolder = arguments.StartFolder,
            Description = arguments.Description,
            ShowNewFolderButton = arguments.ShowNewFolderButton,
            SelectedPath = arguments.SelectedPath,
        };

        if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        {
            return Task.FromResult<FolderData?>(null);
        }

        var path = dialog.SelectedPath;

        return Task.FromResult<FolderData?>(new SystemIOApiFolderData(path));
    }

    public Task<FileData?> OpenFileAsync(OpenFileArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var dialog = new OpenFileDialog
        {
            CheckFileExists = true,
            CheckPathExists = true,
            Filter = ToFilter(arguments.FilterName, arguments.Extensions),
            FileName = arguments.SuggestedFileName,
        };

        if (dialog.ShowDialog() != true)
        {
            return Task.FromResult<FileData?>(null);
        }

        var path = dialog.FileName;

        return Task.FromResult<FileData?>(new SystemIOApiFileData(path));
    }

    public Task<FileData[]> OpenFilesAsync(OpenFileArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var dialog = new OpenFileDialog
        {
            CheckFileExists = true,
            CheckPathExists = true,
            Filter = ToFilter(arguments.FilterName, arguments.Extensions),
            Multiselect = true,
        };

        if (dialog.ShowDialog() != true)
        {
            return Task.FromResult(Array.Empty<FileData>());
        }

        var paths = dialog.FileNames;

        var files = paths
            .Select(static path => (FileData)new SystemIOApiFileData(path))
            .ToArray();

        return Task.FromResult(files);
    }

    public Task<FileData?> SaveFileAsync(SaveFileArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var dialog = new SaveFileDialog
        {
            FileName = arguments.SuggestedFileName,
            DefaultExt = arguments.Extension,
            AddExtension = true,
            Filter = ToFilter(arguments.FilterName, arguments.Extension),
        };

        if (dialog.ShowDialog() != true)
        {
            return Task.FromResult<FileData?>(null);
        }

        var path = dialog.FileName;

        return Task.FromResult<FileData?>(new SystemIOApiFileData(path));
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
