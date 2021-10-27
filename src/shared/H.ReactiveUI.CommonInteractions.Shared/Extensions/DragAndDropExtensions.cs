using System.Windows.Input;
#if HAS_WPF
using System.IO;
#else
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Search;
#endif

#nullable enable

namespace H.ReactiveUI;

public static class DragAndDropExtensions
{
    #region DragFilesEnterCommand

    public static readonly DependencyProperty DragFilesEnterCommandProperty =
        DependencyProperty.RegisterAttached(
            nameof(DragFilesEnterCommandProperty).Replace("Property", string.Empty),
            typeof(ICommand),
            typeof(DragAndDropExtensions),
            new PropertyMetadata(null, OnDragFilesEnterCommandChanged));

#if HAS_WPF
    [AttachedPropertyBrowsableForType(typeof(UIElement))]
#endif
    public static ICommand? GetDragFilesEnterCommand(DependencyObject element)
    {
        element = element ?? throw new ArgumentNullException(nameof(element));

        return (ICommand?)element.GetValue(DragFilesEnterCommandProperty);
    }

    public static void SetDragFilesEnterCommand(DependencyObject element, ICommand? value)
    {
        element = element ?? throw new ArgumentNullException(nameof(element));

        element.SetValue(DragFilesEnterCommandProperty, value);
    }

    private static void OnDragFilesEnterCommandChanged(
        DependencyObject element,
        DependencyPropertyChangedEventArgs args)
    {
        if (element is not UIElement uiElement)
        {
            throw new ArgumentException($"Element should be {nameof(UIElement)}.");
        }
        if (args.OldValue is ICommand oldCommand)
        {
            uiElement.DragEnter -= OnDragFilesEnter;
        }
        if (args.NewValue is ICommand command)
        {
            uiElement.DragEnter += OnDragFilesEnter;
        }
    }

    private static
#if !HAS_WPF
    async
#endif
    void OnDragFilesEnter(object sender, DragEventArgs args)
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
                files.Any())
            {
                command.Execute(files.ToArray());
            }
        }
    }

    #endregion

    #region DragTextEnterCommand

    public static readonly DependencyProperty DragTextEnterCommandProperty =
        DependencyProperty.RegisterAttached(
            nameof(DragTextEnterCommandProperty).Replace("Property", string.Empty),
            typeof(ICommand),
            typeof(DragAndDropExtensions),
            new PropertyMetadata(null, OnDragTextEnterCommandChanged));

#if HAS_WPF
    [AttachedPropertyBrowsableForType(typeof(UIElement))]
#endif
    public static ICommand? GetDragTextEnterCommand(DependencyObject element)
    {
        element = element ?? throw new ArgumentNullException(nameof(element));

        return (ICommand?)element.GetValue(DragTextEnterCommandProperty);
    }

    public static void SetDragTextEnterCommand(DependencyObject element, ICommand? value)
    {
        element = element ?? throw new ArgumentNullException(nameof(element));

        element.SetValue(DragTextEnterCommandProperty, value);
    }

    private static void OnDragTextEnterCommandChanged(
        DependencyObject element,
        DependencyPropertyChangedEventArgs args)
    {
        if (element is not UIElement uiElement)
        {
            throw new ArgumentException($"Element should be {nameof(UIElement)}.");
        }
        if (args.OldValue is ICommand oldCommand)
        {
            uiElement.DragEnter -= OnDragTextEnter;
        }
        if (args.NewValue is ICommand command)
        {
            uiElement.DragEnter += OnDragTextEnter;
        }
    }

    private static
#if !HAS_WPF
    async
#endif
    void OnDragTextEnter(object sender, DragEventArgs args)
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

    public static readonly DependencyProperty DragLeaveCommandProperty =
        DependencyProperty.RegisterAttached(
            nameof(DragLeaveCommandProperty).Replace("Property", string.Empty),
            typeof(ICommand),
            typeof(DragAndDropExtensions),
            new PropertyMetadata(null, OnDragLeaveCommandChanged));

#if HAS_WPF
    [AttachedPropertyBrowsableForType(typeof(UIElement))]
#endif
    public static ICommand? GetDragLeaveCommand(DependencyObject element)
    {
        element = element ?? throw new ArgumentNullException(nameof(element));

        return (ICommand?)element.GetValue(DragLeaveCommandProperty);
    }

    public static void SetDragLeaveCommand(DependencyObject element, ICommand? value)
    {
        element = element ?? throw new ArgumentNullException(nameof(element));

        element.SetValue(DragLeaveCommandProperty, value);
    }

    private static void OnDragLeaveCommandChanged(
        DependencyObject element,
        DependencyPropertyChangedEventArgs args)
    {
        if (element is not UIElement uiElement)
        {
            throw new ArgumentException($"Element should be {nameof(UIElement)}.");
        }
        if (args.OldValue is ICommand oldCommand)
        {
            uiElement.DragLeave -= OnDragLeave;
        }
        if (args.NewValue is ICommand command)
        {
            uiElement.DragLeave += OnDragLeave;
        }
    }

    private static void OnDragLeave(object sender, DragEventArgs args)
    {
        if (sender is UIElement element &&
            GetDragLeaveCommand(element) is ICommand command)
        {
            command.Execute(null);
        }
    }

    #endregion

    #region DropFilesCommand

    public static readonly DependencyProperty DropFilesCommandProperty =
        DependencyProperty.RegisterAttached(
            nameof(DropFilesCommandProperty).Replace("Property", string.Empty),
            typeof(ICommand),
            typeof(DragAndDropExtensions),
            new PropertyMetadata(null, OnDropFilesCommandChanged));

#if HAS_WPF
    [AttachedPropertyBrowsableForType(typeof(UIElement))]
#endif
    public static ICommand? GetDropFilesCommand(DependencyObject element)
    {
        element = element ?? throw new ArgumentNullException(nameof(element));

        return (ICommand?)element.GetValue(DropFilesCommandProperty);
    }

    public static void SetDropFilesCommand(DependencyObject element, ICommand? value)
    {
        element = element ?? throw new ArgumentNullException(nameof(element));

        element.SetValue(DropFilesCommandProperty, value);
    }

    private static void OnDropFilesCommandChanged(
        DependencyObject element,
        DependencyPropertyChangedEventArgs args)
    {
        if (element is not UIElement uiElement)
        {
            throw new ArgumentException($"Element should be {nameof(UIElement)}.");
        }
        if (args.OldValue is ICommand oldCommand)
        {
            uiElement.Drop -= OnDropFiles;
        }
        if (args.NewValue is ICommand command)
        {
            uiElement.Drop += OnDropFiles;
        }
    }

    private static
#if !HAS_WPF
    async
#endif
    void OnDropFiles(object sender, DragEventArgs args)
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
                            .Select(static path => path.ToFile())
                            .ToArray()
                        : new[] { path.ToFile() };
                })
                .ToList();
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

            foreach (var storageFile in storageFiles)
            {
                FileInteractionManager.StorageFiles[storageFile.Path] = storageFile;
            }

            var files = await Task.WhenAll(storageFiles
                .Select(static file => file.ToFileAsync())).ConfigureAwait(true);
#endif
            if (sender is UIElement element &&
                GetDropFilesCommand(element) is ICommand command &&
                files.Any())
            {
                command.Execute(files.ToArray());
            }
        }
    }

    #endregion

    #region DropTextCommand

    public static readonly DependencyProperty DropTextCommandProperty =
        DependencyProperty.RegisterAttached(
            nameof(DropTextCommandProperty).Replace("Property", string.Empty),
            typeof(ICommand),
            typeof(DragAndDropExtensions),
            new PropertyMetadata(null, OnDropTextCommandChanged));

#if HAS_WPF
    [AttachedPropertyBrowsableForType(typeof(UIElement))]
#endif
    public static ICommand? GetDropTextCommand(DependencyObject element)
    {
        element = element ?? throw new ArgumentNullException(nameof(element));

        return (ICommand?)element.GetValue(DropTextCommandProperty);
    }

    public static void SetDropTextCommand(DependencyObject element, ICommand? value)
    {
        element = element ?? throw new ArgumentNullException(nameof(element));

        element.SetValue(DropTextCommandProperty, value);
    }

    private static void OnDropTextCommandChanged(
        DependencyObject element,
        DependencyPropertyChangedEventArgs args)
    {
        if (element is not UIElement uiElement)
        {
            throw new ArgumentException($"Element should be {nameof(UIElement)}.");
        }
        if (args.OldValue is ICommand oldCommand)
        {
            uiElement.Drop -= OnDropText;
        }
        if (args.NewValue is ICommand command)
        {
            uiElement.Drop += OnDropText;
        }
    }

    private static
#if !HAS_WPF
    async
#endif
    void OnDropText(object sender, DragEventArgs args)
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
