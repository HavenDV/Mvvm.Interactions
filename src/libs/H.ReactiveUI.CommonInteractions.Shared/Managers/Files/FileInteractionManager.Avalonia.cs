#if HAS_AVALONIA
using System.Reactive;
using System.Diagnostics;
using System.Reactive.Linq;

namespace H.ReactiveUI;

public partial class FileInteractionManager
{
    #region Static properties

    public static Window? Window { get; set; }

    public static Window RequiredWindow =>
        Window ??
        throw new InvalidOperationException("FileInteractionManager.Window is null and required.");

    #endregion

    #region Methods

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

    public void Register()
    {
        _ = FileInteractions.OpenFolder.RegisterHandler(async context =>
        {
            var arguments = context.Input;

            var dialog = new OpenFolderDialog
            {
                Directory = arguments.SelectedPath,
                Title = arguments.Title,
            };

            var path = await dialog.ShowAsync(RequiredWindow).ConfigureAwait(true);
            if (path == null)
            {
                context.SetOutput(null);
                return;
            }

            context.SetOutput(new SystemIOApiFolderData(path));
        });
        _ = FileInteractions.OpenFile.RegisterHandler(async context =>
        {
            var arguments = context.Input;

            var dialog = new OpenFileDialog
            {
                Filters = ToFilters(arguments.FilterName, arguments.Extensions),
                InitialFileName = arguments.SuggestedFileName,
            };

            var paths = await dialog.ShowAsync(RequiredWindow).ConfigureAwait(true);
            if (paths == null || !paths.Any())
            {
                context.SetOutput(null);
                return;
            }

            var path = paths.First();

            context.SetOutput(new SystemIOApiFileData(path));
        });
        _ = FileInteractions.OpenFiles.RegisterHandler(async context =>
        {
            var arguments = context.Input;

            var dialog = new OpenFileDialog
            {
                Filters = ToFilters(arguments.FilterName, arguments.Extensions),
                InitialFileName = arguments.SuggestedFileName,
                AllowMultiple = true,
            };

            var paths = await dialog.ShowAsync(RequiredWindow).ConfigureAwait(true);
            if (paths == null || !paths.Any())
            {
                context.SetOutput(Array.Empty<FileData>());
                return;
            }

            var files = paths
                .Select(static path => (FileData)new SystemIOApiFileData(path))
                .ToArray();

            context.SetOutput(files);
        });
        _ = FileInteractions.SaveFile.RegisterHandler(async context =>
        {
            var arguments = context.Input;

            var dialog = new SaveFileDialog
            {
                InitialFileName = arguments.SuggestedFileName,
                DefaultExtension = arguments.Extension,
                Filters = ToFilters(arguments.FilterName, arguments.Extension),
            };

            var path = await dialog.ShowAsync(RequiredWindow).ConfigureAwait(true);
            if (path == null || string.IsNullOrWhiteSpace(path))
            {
                context.SetOutput(null);
                return;
            }

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


#endregion
}
#endif
