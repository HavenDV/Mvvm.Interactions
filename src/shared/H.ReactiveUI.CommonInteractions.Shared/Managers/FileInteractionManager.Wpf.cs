#if HAS_WPF
using System.Reactive;
using System.IO;
using System.Diagnostics;
using System.Reactive.Linq;
using Microsoft.Win32;

namespace H.ReactiveUI;

public partial class FileInteractionManager
{
    #region Methods

    public void Register()
    {
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
                : await arguments.BytesFunc().ConfigureAwait(true);
            var path = dialog.FileName;

            File.WriteAllBytes(path, bytes);

            context.SetOutput(path);
        });
        _ = FileInteractions.SaveOpenFile.RegisterHandler(context =>
        {
            var arguments = context.Input;

            File.WriteAllBytes(arguments.FullPath, arguments.Bytes);

            context.SetOutput(arguments.FullPath);
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
    }


    #endregion
}
#endif
