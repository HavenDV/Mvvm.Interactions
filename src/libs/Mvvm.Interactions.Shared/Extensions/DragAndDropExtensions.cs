#if !HAS_AVALONIA && !HAS_MAUI
using System.Windows.Input;
#if HAS_WPF
using System.IO;
#elif HAS_MAUI
#else
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Search;
#endif

#nullable enable

namespace Mvvm.Interactions;

[AttachedDependencyProperty<ICommand, UIElement>("DragFilesEnterCommand", BindEvent = nameof(UIElement.DragEnter))]
[AttachedDependencyProperty<ICommand, UIElement>("DragTextEnterCommand", BindEvent = nameof(UIElement.DragEnter))]
[AttachedDependencyProperty<ICommand, UIElement>("DragLeaveCommand", BindEvent = nameof(UIElement.DragLeave))]
[AttachedDependencyProperty<ICommand, UIElement>("DropFilesCommand", BindEvent = nameof(UIElement.Drop))]
[AttachedDependencyProperty<ICommand, UIElement>("DropTextCommand", BindEvent = nameof(UIElement.Drop))]
public static partial class DragAndDropExtensions
{
    #region DragFilesEnterCommand

    private static
#if !HAS_WPF
    async
#endif
    void OnDragFilesEnterCommandChanged_DragEnter(object sender, DragEventArgs args)
    {
#if !HAS_WPF
        args.AcceptedOperation = DataPackageOperation.Copy;

        if (args.DragUIOverride != null)
        {
            args.DragUIOverride.IsCaptionVisible = false;
            args.DragUIOverride.IsGlyphVisible = false;
            args.DragUIOverride.IsContentVisible = true;
        }
#endif

#if HAS_WPF
        if (args.Data.GetDataPresent(DataFormats.FileDrop) &&
            args.Data.GetData(DataFormats.FileDrop) is string[] paths)
#else
        if (args.DataView.Contains(StandardDataFormats.StorageItems))
#endif
        {
            var files = new List<FileData>();
#if HAS_WPF
            files.AddRange(paths
                .SelectMany(static path =>
                {
                    return Directory.Exists(path)
                        ? Directory
                            .EnumerateFiles(path, "*", SearchOption.AllDirectories)
                            .Select(static path => new FileData(path))
                            .ToArray()
                        : new[] { new FileData(path) };
                }));
#else
            var items = await args.DataView.GetStorageItemsAsync();

            files.AddRange(items
                .OfType<StorageFile>()
                .Select(static file => new FileData(file.Path)));

            var folderFiles = await Task.WhenAll(items
                .OfType<StorageFolder>()
                .Select(async static folder => await folder.GetFilesAsync(
                    CommonFileQuery.OrderByName))).ConfigureAwait(true);

            files.AddRange(folderFiles
                .SelectMany(static files => files)
                .Select(static file => new FileData(file.Path)));
#endif

            if (sender is UIElement element &&
                GetDragFilesEnterCommand(element) is ICommand command &&
                files.Count != 0)
            {
                command.Execute(files.ToArray());
            }
        }
    }

    #endregion

    #region DragTextEnterCommand

    private static
#if !HAS_WPF
    async
#endif
    void OnDragTextEnterCommandChanged_DragEnter(object sender, DragEventArgs args)
    {
#if HAS_WPF
        if (args.Data.GetDataPresent(DataFormats.UnicodeText, true) &&
            args.Data.GetData(DataFormats.UnicodeText, true) is string text)
#else
        if (args.DataView.Contains(StandardDataFormats.Text))
#endif
        {
#if !HAS_WPF
            var text = await args.DataView.GetTextAsync() ?? string.Empty;
#endif
            if (sender is UIElement element &&
                GetDragTextEnterCommand(element) is ICommand command)
            {
                command.Execute(text);
            }
        }
    }

    #endregion

    #region DragLeaveCommand

    private static void OnDragLeaveCommandChanged_DragLeave(object sender, DragEventArgs args)
    {
        if (sender is UIElement element &&
            GetDragLeaveCommand(element) is ICommand command)
        {
            command.Execute(null);
        }
    }

    #endregion

    #region DropFilesCommand

    private static
#if !HAS_WPF
    async
#endif
    void OnDropFilesCommandChanged_Drop(object sender, DragEventArgs args)
    {
#if HAS_WPF
        if (args.Data.GetDataPresent(DataFormats.FileDrop) &&
            args.Data.GetData(DataFormats.FileDrop) is string[] paths)
#else
        if (args.DataView.Contains(StandardDataFormats.StorageItems))
#endif
        {
#if HAS_WPF
            var files = paths
                .SelectMany(static path =>
                {
                    return Directory.Exists(path)
                        ? Directory
                            .EnumerateFiles(path, "*", SearchOption.AllDirectories)
                            .Select(static path => (FileData)new SystemIOApiFileData(path))
                            .ToArray()
                        : new[] { (FileData)new SystemIOApiFileData(path) };
                })
                .ToArray();
#else
            var items = await args.DataView.GetStorageItemsAsync();

            var storageFiles = items
                .OfType<StorageFile>()
                .ToList();
            var folderFiles = await Task.WhenAll(
                items
                    .OfType<StorageFolder>()
                    .Select(async static folder => await folder.GetFilesAsync(CommonFileQuery.OrderByName)))
                .ConfigureAwait(true);

            storageFiles.AddRange(folderFiles
                .SelectMany(static files => files));

            var files = storageFiles
                .Select(static file => (FileData)new StorageApiFileData(file))
                .ToArray();
#endif
            if (sender is UIElement element &&
                GetDropFilesCommand(element) is ICommand command &&
                files.Length != 0)
            {
                command.Execute(files);
            }
        }
    }

    #endregion

    #region DropTextCommand

    private static
#if !HAS_WPF
    async
#endif
    void OnDropTextCommandChanged_Drop(object sender, DragEventArgs args)
    {
#if HAS_WPF
        if (args.Data.GetDataPresent(DataFormats.UnicodeText, true) &&
            args.Data.GetData(DataFormats.UnicodeText, true) is string text)
#else
        if (args.DataView.Contains(StandardDataFormats.Text))
#endif
        {
#if !HAS_WPF
            var text = await args.DataView.GetTextAsync() ?? string.Empty;
#endif
            if (sender is UIElement element &&
                GetDropTextCommand(element) is ICommand command)
            {
                command.Execute(text);
            }
        }
    }

    #endregion
}
#endif