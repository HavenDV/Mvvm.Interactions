#if !HAS_WPF
using System.Reactive;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;

namespace H.ReactiveUI;

public partial class FileInteractionManager
{
    #region Methods

    public void Register()
    {
        _ = FileInteractions.OpenFile.RegisterHandler(async context =>
        {
            var arguments = context.Input;

            var picker = new FileOpenPicker();
            foreach (var extension in arguments.Extensions)
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

            var picker = new FileOpenPicker();
            foreach (var extension in arguments.Extensions)
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
            };
            var file = await picker.PickSaveFileAsync();
            if (file == null)
            {
                context.SetOutput(null);
                return;
            }

            context.SetOutput(new StorageApiFileData(file));
        });

        _ = FileInteractions.CreateTemporaryFile.RegisterHandler(async static context =>
        {
            var fileName = context.Input;

            var file = await ApplicationData.Current.TemporaryFolder
                .CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            context.SetOutput(new StorageApiFileData(file));
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
