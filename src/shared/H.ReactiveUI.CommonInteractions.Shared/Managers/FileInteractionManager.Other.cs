#if !HAS_WPF
using System.Reactive;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;

namespace H.ReactiveUI;

public partial class FileInteractionManager
{
    #region Properties

    public static Dictionary<string, StorageFile> StorageFiles { get; } = new();

    #endregion

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

            StorageFiles[file.Path] = file;

            var model = await file.ToFileAsync().ConfigureAwait(true);

            context.SetOutput(model);
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

            foreach (var file in files)
            {
                StorageFiles[file.Path] = file;
            }

            var models = await Task.WhenAll(files
                .Select(static file => file.ToFileAsync())).ConfigureAwait(true);

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

            var bytes = arguments.BytesFunc == null
                ? arguments.Bytes
                : await arguments.BytesFunc().ConfigureAwait(true);

            using (var stream = await file.OpenStreamForWriteAsync().ConfigureAwait(true))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(bytes);
            }

            StorageFiles[file.Path] = file;

            context.SetOutput(file.Path);
        });
        _ = FileInteractions.SaveOpenFile.RegisterHandler(async context =>
        {
            var arguments = context.Input;

            if (!StorageFiles.TryGetValue(arguments.FullPath, out var file))
            {
                context.SetOutput(null);
                return;
            }

            using (var stream = await file.OpenStreamForWriteAsync().ConfigureAwait(true))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(arguments.Bytes);
            }

            context.SetOutput(file.Path);
        });
        _ = FileInteractions.LaunchPath.RegisterHandler(async context =>
        {
            var path = context.Input;

            var file = StorageFiles.TryGetValue(path, out var result)
                ? result
                : await StorageFile.GetFileFromPathAsync(path);

            _ = await Launcher.LaunchFileAsync(file);

            context.SetOutput(Unit.Default);
        });
        _ = FileInteractions.LaunchInTemp.RegisterHandler(async static context =>
        {
            var file = context.Input;

            var storageFile = await ApplicationData.Current.TemporaryFolder
                .CreateFileAsync(file.FileName, CreationCollisionOption.ReplaceExisting);

            using (var stream = await storageFile.OpenStreamForWriteAsync().ConfigureAwait(true))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(file.Bytes);
            }

            _ = await Launcher.LaunchFileAsync(storageFile);

            context.SetOutput(Unit.Default);
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
