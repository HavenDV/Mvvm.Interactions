#if !HAS_WPF && !HAS_AVALONIA
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;

namespace Mvvm.CommonInteractions;

public partial class FileInteractions
{
#if HAS_WINUI
    public static Window? Window { get; set; }

    private Window GetRequiredWindow()
    {
        return Window ?? throw new InvalidOperationException("FileInteractionManager.Window is required.");
    }
#endif

    public async Task<FolderData?> OpenFolderAsync(OpenFolderArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var picker = new FolderPicker
        {
            SuggestedStartLocation = arguments.StartFolder switch
            {
                Environment.SpecialFolder.Desktop => PickerLocationId.Desktop,
                Environment.SpecialFolder.MyDocuments => PickerLocationId.DocumentsLibrary,
                Environment.SpecialFolder.MyPictures => PickerLocationId.PicturesLibrary,
                Environment.SpecialFolder.MyMusic => PickerLocationId.MusicLibrary,
                Environment.SpecialFolder.MyVideos => PickerLocationId.VideosLibrary,
                Environment.SpecialFolder.MyComputer => PickerLocationId.ComputerFolder,
                _ => PickerLocationId.Unspecified,
            },
            CommitButtonText = arguments.ButtonText,
        }
#if HAS_WINUI && !HAS_UNO
            .Initialize(GetRequiredWindow())
#endif
            ;

        var file = await picker.PickSingleFolderAsync();
        if (file == null)
        {
            return null;
        }

        return new StorageApiFolderData(file);
    }

    public async Task<FileData?> OpenFileAsync(OpenFileArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var picker = new FileOpenPicker()
#if HAS_WINUI && !HAS_UNO
            .Initialize(GetRequiredWindow())
#endif
            ;

        var extensions = arguments.Extensions.Any()
            ? arguments.Extensions
            : new[] { "*" };

        foreach (var extension in extensions)
        {
            picker.FileTypeFilter.Add(extension);
        }

        var file = await picker.PickSingleFileAsync();
        if (file == null)
        {
            return null;
        }

        return new StorageApiFileData(file);
    }

    public async Task<FileData[]> OpenFilesAsync(OpenFileArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var picker = new FileOpenPicker()
#if HAS_WINUI && !HAS_UNO
            .Initialize(GetRequiredWindow())
#endif
            ;
        var extensions = arguments.Extensions.Any()
            ? arguments.Extensions
            : new[] { "*" };

        foreach (var extension in extensions)
        {
            picker.FileTypeFilter.Add(extension);
        }

        var files = await picker.PickMultipleFilesAsync();
        if (files == null)
        {
            return Array.Empty<FileData>();
        }

        var models = files
            .Select(static file => (FileData)new StorageApiFileData(file))
            .ToArray();

        return models;
    }

    public async Task<FileData?> SaveFileAsync(SaveFileArguments arguments, CancellationToken cancellationToken = default)
    {
        arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

        var picker = new FileSavePicker
        {
            SuggestedStartLocation = PickerLocationId.Downloads,
            SuggestedFileName = arguments.SuggestedFileName,
            FileTypeChoices =
            {
                { arguments.Extension, new List<string> { arguments.Extension } },
            },
        }
#if HAS_WINUI && !HAS_UNO
            .Initialize(GetRequiredWindow())
#endif
            ;
        var file = await picker.PickSaveFileAsync();
        if (file == null)
        {
            return null;
        }

        return new StorageApiFileData(file);
    }

    public
#if !HAS_WINUI
        async
#endif
        Task<FileData> CreateTemporaryFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

#if HAS_WINUI
        var folder = Path.Combine(
            Path.GetTempPath(),
            "Mvvm.CommonInteractions",
            $"{new Random().Next()}");
        var path = Path.Combine(folder, fileName);

        _ = Directory.CreateDirectory(folder);

        return Task.FromResult<FileData>(new SystemIOApiFileData(path));
#else
        var file = await ApplicationData.Current.TemporaryFolder
            .CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

        return new StorageApiFileData(file);
#endif
    }

    public async Task<FileData> OpenPathAsync(string path, CancellationToken cancellationToken = default)
    {
        path = path ?? throw new ArgumentNullException(nameof(path));

        var file = await StorageFile.GetFileFromPathAsync(path);

        return new StorageApiFileData(file);
    }

    public async Task LaunchFolderAsync(string path, CancellationToken cancellationToken = default)
    {
        path = path ?? throw new ArgumentNullException(nameof(path));

        var folder = await StorageFolder.GetFolderFromPathAsync(path);

        _ = await Launcher.LaunchFolderAsync(folder);
    }
}
#endif
