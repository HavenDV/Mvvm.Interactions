#if !HAS_WPF && !HAS_AVALONIA
using System.Reactive;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;

namespace H.ReactiveUI;

public partial class FileInteractionManager
{
#if HAS_WINUI
    public static Window? Window { get; set; }

    private Window GetRequiredWindow()
    {
        return Window ?? throw new InvalidOperationException("FileInteractionManager.Window is required.");
    }
#endif

    #region Methods

    public void Register()
    {
        _ = FileInteractions.OpenFolder.RegisterHandler(async context =>
        {
            var arguments = context.Input;

            var picker = new FolderPicker
            {
                SuggestedStartLocation =  arguments.StartFolder switch
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
                context.SetOutput(null);
                return;
            }

            context.SetOutput(new StorageApiFolderData(file));
        });
        _ = FileInteractions.OpenFile.RegisterHandler(async context =>
        {
            var arguments = context.Input;

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
                context.SetOutput(null);
                return;
            }

            context.SetOutput(new StorageApiFileData(file));
        });
        _ = FileInteractions.OpenFiles.RegisterHandler(async context =>
        {
            var arguments = context.Input;

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
                context.SetOutput(Array.Empty<FileData>());
                return;
            }

            var models = files
                .Select(static file => (FileData)new StorageApiFileData(file))
                .ToArray();

            context.SetOutput(models);
        });
        _ = FileInteractions.SaveFile.RegisterHandler(async context =>
        {
            var arguments = context.Input;

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
                context.SetOutput(null);
                return;
            }

            context.SetOutput(new StorageApiFileData(file));
        });

        _ = FileInteractions.CreateTemporaryFile.RegisterHandler(
#if !HAS_WINUI
        async
#endif
        static context =>
        {
            var fileName = context.Input;

#if HAS_WINUI
            var folder = Path.Combine(
                Path.GetTempPath(),
                "H.ReactiveUI.CommonInteractions",
                $"{new Random().Next()}");
            var path = Path.Combine(folder, fileName);

            _ = Directory.CreateDirectory(folder);

            context.SetOutput(new SystemIOApiFileData(path));
#else
            var file = await ApplicationData.Current.TemporaryFolder
                .CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            context.SetOutput(new StorageApiFileData(file));
#endif
        });

        _ = FileInteractions.OpenPath.RegisterHandler(async context =>
        {
            var path = context.Input;

            var file = await StorageFile.GetFileFromPathAsync(path);

            context.SetOutput(new StorageApiFileData(file));
        });
        _ = FileInteractions.LaunchFolder.RegisterHandler(static async context =>
        {
            var path = context.Input;

            var folder = await StorageFolder.GetFolderFromPathAsync(path);

            _ = await Launcher.LaunchFolderAsync(folder);

            context.SetOutput(Unit.Default);
        });

    }

#endregion
}
#endif
