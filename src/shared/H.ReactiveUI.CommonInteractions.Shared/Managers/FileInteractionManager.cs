using System.Reactive;
#if WPF
using System.IO;
using System.Diagnostics;
using System.Reactive.Linq;
using Microsoft.Win32;
#else
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
#endif

namespace H.ReactiveUI;

public class FileInteractionManager
{
    #region Properties

    private Func<string, string>? LocalizationFunc { get; }

#if !WPF
    private Dictionary<string, StorageFile> StorageFiles { get; } = new();
#endif

    #endregion

    #region Constructors

    public FileInteractionManager(Func<string, string>? localizationFunc = null)
    {
        LocalizationFunc = localizationFunc;
    }

    #endregion

    #region Methods

    private string Localize(string value) => LocalizationFunc?.Invoke(value) ?? value;

    public void Register()
    {
#if WPF
        _ = FileInteractions.OpenFile.RegisterHandler(context =>
        {
            var arguments = context.Input;

            var wildcards = arguments.Extensions
                .Select(static extension => $"*{extension}")
                .ToArray();
            var filter = $@"{Localize(arguments.FilterName)} ({string.Join(", ", wildcards)})|{string.Join(";", wildcards)}";

            var dialog = new OpenFileDialog
            {
                FileName = arguments.SuggestedFileName,
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = filter,
            };
            if (dialog.ShowDialog() != true)
            {
                context.SetOutput(null);
                return;
            }

            var path = dialog.FileName;
            var model = path.ToFile();

            context.SetOutput(model);
        });
        _ = FileInteractions.OpenFiles.RegisterHandler(context =>
        {
            var arguments = context.Input;

            var wildcards = arguments.Extensions
                .Select(static extension => $"*{extension}")
                .ToArray();
            var filter = $@"{Localize(arguments.FilterName)} ({string.Join(", ", wildcards)})|{string.Join(";", wildcards)}";

            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = filter,
                Multiselect = true,
            };
            if (dialog.ShowDialog() != true)
            {
                context.SetOutput(Array.Empty<FileData>());
                return;
            }

            var paths = dialog.FileNames;
            var models = paths
                .Select(static path => path.ToFile())
                .ToArray();

            context.SetOutput(models);
        });
        _ = FileInteractions.SaveFile.RegisterHandler(async static context =>
        {
            var arguments = context.Input;

            var wildcards = new[] { $"*{arguments.Extension}" };
            var filter = $@"{arguments.FilterName} ({string.Join(", ", wildcards)})|{string.Join(";", wildcards)}";

            var dialog = new SaveFileDialog
            {
                FileName = arguments.SuggestedFileName,
                DefaultExt = arguments.Extension,
                AddExtension = true,
                Filter = filter,
            };
            if (dialog.ShowDialog() != true)
            {
                context.SetOutput(null);
                return;
            }

            var bytes = arguments.BytesFunc == null
                ? arguments.Bytes
                : await arguments.BytesFunc().ConfigureAwait(false);
            var path = dialog.FileName;

            File.WriteAllBytes(path, bytes);

            context.SetOutput(path);
        });
        _ = FileInteractions.LaunchPath.RegisterHandler(static context =>
        {
            var path = context.Input;

            _ = Process.Start(new ProcessStartInfo(path)
            {
                UseShellExecute = true,
            });

            context.SetOutput(Unit.Default);
        });
        _ = FileInteractions.LaunchInTemp.RegisterHandler(async static context =>
        {
            var file = context.Input;

            var folder = Path.Combine(
                Path.GetTempPath(),
                "H.ReactiveUI.CommonInteractions",
                $"{new Random().Next()}");
            var path = Path.Combine(
                folder,
                file.FileName);

            _ = Directory.CreateDirectory(folder);
            File.WriteAllBytes(path, file.Bytes);

            _ = await FileInteractions.LaunchPath.Handle(path);

            context.SetOutput(Unit.Default);
        });
        _ = FileInteractions.LaunchFolder.RegisterHandler(static context =>
        {
            var path = context.Input;

            _ = Process.Start(new ProcessStartInfo(path)
            {
                UseShellExecute = true,
            });

            context.SetOutput(Unit.Default);
        });
#else
        _ = FileInteractions.OpenFile.RegisterHandler(static async context =>
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

            var model = await file.ToFileAsync().ConfigureAwait(false);

            context.SetOutput(model);
        });
        _ = FileInteractions.OpenFiles.RegisterHandler(static async context =>
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

            var models = await Task.WhenAll(files
                .Select(static file => file.ToFileAsync())).ConfigureAwait(false);

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
                : await arguments.BytesFunc().ConfigureAwait(false);

            using (var stream = await file.OpenStreamForWriteAsync().ConfigureAwait(false))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(bytes);
            }

            StorageFiles[file.Path] = file;

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
#endif

        #endregion
    }
}
