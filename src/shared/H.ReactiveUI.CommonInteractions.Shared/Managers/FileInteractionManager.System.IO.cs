#if HAS_WPF || HAS_AVALONIA
using System.Reactive;
using System.Diagnostics;
using System.Reactive.Linq;
#if !HAS_AVALONIA
using System.IO;
using Microsoft.Win32;
#endif

namespace H.ReactiveUI;

public partial class FileInteractionManager
{
    #region Static properties

#if HAS_AVALONIA
    public static Window? Window { get; set; }

    public static Window RequiredWindow =>
        Window ??
        throw new InvalidOperationException("FileInteractionManager.Window is null and required.");
#endif

    #endregion

    #region Methods

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

#pragma warning disable CA1822 // Mark members as static
    public void Register()
#pragma warning restore CA1822 // Mark members as static
    {
        _ = FileInteractions.OpenFile.RegisterHandler(
#if HAS_AVALONIA
        async
#endif
        context =>
        {
            var arguments = context.Input;

            var dialog = new OpenFileDialog
            {
#if HAS_AVALONIA
                Filters =
                {
                    new FileDialogFilter
                    {
                        Extensions = arguments.Extensions.ToList(),
                        Name = arguments.FilterName,
                    }
                },
                InitialFileName = arguments.SuggestedFileName,
#else
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = ToFilter(arguments.FilterName, arguments.Extensions),
                FileName = arguments.SuggestedFileName,
#endif
            };

#if HAS_AVALONIA
            var paths = await dialog.ShowAsync(RequiredWindow).ConfigureAwait(true);
            if (paths == null || !paths.Any())
            {
                context.SetOutput(null);
                return;
            }

            var path = paths.First();
#else
            if (dialog.ShowDialog() != true)
            {
                context.SetOutput(null);
                return;
            }

            var path = dialog.FileName;
#endif

            context.SetOutput(new SystemIOApiFileData(path));
        });
        _ = FileInteractions.OpenFiles.RegisterHandler(
#if HAS_AVALONIA
        async
#endif
        context =>
        {
            var arguments = context.Input;

            var dialog = new OpenFileDialog
            {
#if HAS_AVALONIA
                Filters =
                {
                    new FileDialogFilter
                    {
                        Extensions = arguments.Extensions.ToList(),
                        Name = arguments.FilterName,
                    }
                },
                InitialFileName = arguments.SuggestedFileName,
                AllowMultiple = true,
#else
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = ToFilter(arguments.FilterName, arguments.Extensions),
                Multiselect = true,
#endif
            };

#if HAS_AVALONIA
            var paths = await dialog.ShowAsync(RequiredWindow).ConfigureAwait(true);
            if (paths == null || !paths.Any())
            {
                context.SetOutput(Array.Empty<FileData>());
                return;
            }
#else
            if (dialog.ShowDialog() != true)
            {
                context.SetOutput(Array.Empty<FileData>());
                return;
            }

            var paths = dialog.FileNames;
#endif

            var files = paths
                .Select(static path => (FileData)new SystemIOApiFileData(path))
                .ToArray();

            context.SetOutput(files);
        });
        _ = FileInteractions.SaveFile.RegisterHandler(
#if HAS_AVALONIA
        async
#endif
        context =>
        {
            var arguments = context.Input;

            var dialog = new SaveFileDialog
            {
#if HAS_AVALONIA
                InitialFileName = arguments.SuggestedFileName,
                DefaultExtension = arguments.Extension,
                Filters =
                {
                    new FileDialogFilter
                    {
                        Extensions = new List<string>{ arguments.Extension },
                        Name = arguments.FilterName,
                    }
                },
#else
                FileName = arguments.SuggestedFileName,
                DefaultExt = arguments.Extension,
                AddExtension = true,
                Filter = ToFilter(arguments.FilterName, arguments.Extension),
#endif
            };

#if HAS_AVALONIA
            var path = await dialog.ShowAsync(RequiredWindow).ConfigureAwait(true);
            if (path == null || string.IsNullOrWhiteSpace(path))
            {
                context.SetOutput(null);
                return;
            }
#else
            if (dialog.ShowDialog() != true)
            {
                context.SetOutput(null);
                return;
            }

            var path = dialog.FileName;
#endif

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
