#if HAS_AVALONIA
using System.Diagnostics;

namespace Mvvm.CommonInteractions;

public partial class FileInteractions
{
    #region Properties

    public static Window? Window { get; set; }

    public static Window RequiredWindow =>
        Window ??
        throw new InvalidOperationException("FileInteractionManager.Window is null and required.");

    #endregion

    private static List<FileDialogFilter> ToFilters(string filterName, params string[] extensions)
    {
        if (!extensions.Any())
        {
            return new List<FileDialogFilter>();
        }

        return new List<FileDialogFilter>
        {
            new FileDialogFilter
            {
                Extensions = extensions.Select(static extension => extension.TrimStart('.')).ToList(),
                Name = filterName,
            }
        };
    }

    public async Task<FolderData?> OpenFolderAsync(OpenFolderArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var dialog = new OpenFolderDialog
        {
            Directory = arguments.SelectedPath,
            Title = arguments.Title,
        };

        var path = await dialog.ShowAsync(RequiredWindow).ConfigureAwait(true);
        if (path == null)
        {
            return null;
        }

        return new SystemIOApiFolderData(path);
    }

    public async Task<FileData?> OpenFileAsync(OpenFileArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var dialog = new OpenFileDialog
        {
            Filters = ToFilters(arguments.FilterName, arguments.Extensions),
            InitialFileName = arguments.SuggestedFileName,
        };

        var paths = await dialog.ShowAsync(RequiredWindow).ConfigureAwait(true);
        if (paths == null || !paths.Any())
        {
            return null;
        }

        var path = paths.First();

        return new SystemIOApiFileData(path);
    }

    public async Task<FileData[]> OpenFilesAsync(OpenFileArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var dialog = new OpenFileDialog
        {
            Filters = ToFilters(arguments.FilterName, arguments.Extensions),
            InitialFileName = arguments.SuggestedFileName,
            AllowMultiple = true,
        };

        var paths = await dialog.ShowAsync(RequiredWindow).ConfigureAwait(true);
        if (paths == null || !paths.Any())
        {
            return Array.Empty<FileData>();
        }

        var files = paths
            .Select(static path => (FileData)new SystemIOApiFileData(path))
            .ToArray();

        return files;
    }

    public async Task<FileData?> SaveFileAsync(SaveFileArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var dialog = new SaveFileDialog
        {
            InitialFileName = arguments.SuggestedFileName,
            DefaultExtension = arguments.Extension,
            Filters = ToFilters(arguments.FilterName, arguments.Extension),
        };

        var path = await dialog.ShowAsync(RequiredWindow).ConfigureAwait(true);
        if (path == null || string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        return new SystemIOApiFileData(path);
    }

    public Task<FileData> CreateTemporaryFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

        var folder = Path.Combine(
            Path.GetTempPath(),
            "Mvvm.CommonInteractions",
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
