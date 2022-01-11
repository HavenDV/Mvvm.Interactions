#if HAS_WPF
using System.Reactive;
using System.Diagnostics;
using System.Reactive.Linq;
using System.IO;
using Microsoft.Win32;

namespace H.ReactiveUI;

public partial class FileInteractionManager
{
    private string ToFilter(string filterName, params string[] extensions)
    {
        if (!extensions.Any())
        {
            return string.Empty;
        }

        var wildcards = extensions
            .Select(static extension => $"*{extension}")
            .ToArray();
        var filter = $@"{Localize(filterName)} ({string.Join(", ", wildcards)})|{string.Join(";", wildcards)}";

        return filter;
    }

    public void Register()
    {
        _ = FileInteractions.OpenFolder.RegisterHandler(context =>
        {
            var arguments = context.Input;

            var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                RootFolder = arguments.StartFolder,
                Description = arguments.Description,
                ShowNewFolderButton = arguments.ShowNewFolderButton,
                SelectedPath = arguments.SelectedPath,
            };

            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                context.SetOutput(null);
                return;
            }

            var path = dialog.SelectedPath;

            context.SetOutput(new SystemIOApiFolderData(path));
        });
        _ = FileInteractions.OpenFile.RegisterHandler(context =>
        {
            var arguments = context.Input;

            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = ToFilter(arguments.FilterName, arguments.Extensions),
                FileName = arguments.SuggestedFileName,
            };

            if (dialog.ShowDialog() != true)
            {
                context.SetOutput(null);
                return;
            }

            var path = dialog.FileName;

            context.SetOutput(new SystemIOApiFileData(path));
        });
        _ = FileInteractions.OpenFiles.RegisterHandler(context =>
        {
            var arguments = context.Input;

            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = ToFilter(arguments.FilterName, arguments.Extensions),
                Multiselect = true,
            };

            if (dialog.ShowDialog() != true)
            {
                context.SetOutput(Array.Empty<FileData>());
                return;
            }

            var paths = dialog.FileNames;

            var files = paths
                .Select(static path => (FileData)new SystemIOApiFileData(path))
                .ToArray();

            context.SetOutput(files);
        });
        _ = FileInteractions.SaveFile.RegisterHandler(context =>
        {
            var arguments = context.Input;

            var dialog = new SaveFileDialog
            {
                FileName = arguments.SuggestedFileName,
                DefaultExt = arguments.Extension,
                AddExtension = true,
                Filter = ToFilter(arguments.FilterName, arguments.Extension),
            };

            if (dialog.ShowDialog() != true)
            {
                context.SetOutput(null);
                return;
            }

            var path = dialog.FileName;

            context.SetOutput(new SystemIOApiFileData(path));
        });

        _ = FileInteractions.CreateTemporaryFile.RegisterHandler(static context =>
        {
            var fileName = context.Input;

            var folder = Path.Combine(
                Path.GetTempPath(),
                "H.ReactiveUI.CommonInteractions",
                $"{new Random().Next()}");
            var path = Path.Combine(folder, fileName);

            _ = Directory.CreateDirectory(folder);

            context.SetOutput(new SystemIOApiFileData(path));
        });
        _ = FileInteractions.OpenPath.RegisterHandler(static context =>
        {
            var path = context.Input;

            context.SetOutput(new SystemIOApiFileData(path));
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
}
#endif
