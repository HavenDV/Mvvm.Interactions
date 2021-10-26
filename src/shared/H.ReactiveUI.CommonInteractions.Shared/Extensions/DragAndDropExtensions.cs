using System.Windows.Input;
#if WPF
using System.IO;
using System.Windows;
#else
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml;
#endif

#nullable enable

namespace H.ReactiveUI;

public static class DragAndDropExtensions
{
    #region DragEnterCommand

    public static readonly DependencyProperty DragEnterCommandProperty =
        DependencyProperty.RegisterAttached(
            nameof(DragEnterCommandProperty).Replace("Property", string.Empty),
            typeof(ICommand),
            typeof(DragAndDropExtensions),
            new PropertyMetadata(null, OnDragEnterCommandChanged));

#if WPF
    [AttachedPropertyBrowsableForType(typeof(UIElement))]
#endif
    public static ICommand? GetDragEnterCommand(DependencyObject element)
    {
        element = element ?? throw new ArgumentNullException(nameof(element));

        return (ICommand?)element.GetValue(DragEnterCommandProperty);
    }

    public static void SetDragEnterCommand(DependencyObject element, ICommand? value)
    {
        element = element ?? throw new ArgumentNullException(nameof(element));

        element.SetValue(DragEnterCommandProperty, value);
    }

    private static void OnDragEnterCommandChanged(
        DependencyObject element,
        DependencyPropertyChangedEventArgs args)
    {
        if (element is not UIElement uiElement)
        {
            throw new ArgumentException($"Element should be {nameof(UIElement)}.");
        }
        if (args.OldValue is ICommand oldCommand)
        {
            uiElement.DragEnter -= OnDragEnter;
        }
        if (args.NewValue is ICommand command)
        {
            uiElement.DragEnter += OnDragEnter;
        }
    }

    // Android requires full type name.
#if WPF
    private static void OnDragEnter(object sender, DragEventArgs args)
#else
    private async static void OnDragEnter(object sender, Windows.UI.Xaml.DragEventArgs args)
#endif
    {
        var names = new List<string>();
#if WPF
        if (!args.Data.GetDataPresent(DataFormats.FileDrop) ||
            args.Data.GetData(DataFormats.FileDrop) is not string[] paths)
        {
            return;
        }

        names.AddRange(paths
            .Select(static path => Path.GetFileName(path))
            .ToArray());
#else
        args.AcceptedOperation = DataPackageOperation.Copy;

        if (args.DragUIOverride != null)
        {
            args.DragUIOverride.IsCaptionVisible = false;
            args.DragUIOverride.IsGlyphVisible = false;
            args.DragUIOverride.IsContentVisible = true;
        }

        if (args.DataView.Contains(StandardDataFormats.StorageItems))
        {
            var items = await args.DataView.GetStorageItemsAsync();

            names.AddRange(items
                .OfType<StorageFile>()
                .Select(static file => file.Name)
                .ToArray());
            names.AddRange(items
                .OfType<StorageFolder>()
                .Select(static folder => folder.Name)
                .ToArray());
        }
#endif

        if (sender is UIElement element &&
            GetDragEnterCommand(element) is ICommand command)
        {
            command.Execute(names);
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

#if WPF
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

    // Android requires full type name.
#if WPF
    private static void OnDragLeave(object sender, DragEventArgs args)
#else
    private static void OnDragLeave(object sender, Windows.UI.Xaml.DragEventArgs args)
#endif
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

#if WPF
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

    // Android requires full type name.
#if WPF
    private static void OnDropFiles(object sender, DragEventArgs args)
#else
    private static async void OnDropFiles(object sender, Windows.UI.Xaml.DragEventArgs args)
#endif
    {
#if WPF
        if (args.Data.GetDataPresent(DataFormats.FileDrop) &&
            args.Data.GetData(DataFormats.FileDrop) is string[] paths)
#else
        if (args.DataView.Contains(StandardDataFormats.StorageItems))
#endif
        {
            var files = new List<FileData>();
#if WPF
            files.AddRange(paths
                .SelectMany(static path =>
                {
                    return Directory.Exists(path)
                        ? Directory
                            .EnumerateFiles(path, "*", SearchOption.AllDirectories)
                            .Select(static path => path.ToFile())
                            .ToArray()
                        : new[] { path.ToFile() };
                }));
#else
            var items = await args.DataView.GetStorageItemsAsync();

            files.AddRange(await Task.WhenAll(items
                .OfType<StorageFile>()
                .Select(static file => file.ToFileAsync())).ConfigureAwait(false));

            var folderFiles = await Task.WhenAll(items
                .OfType<StorageFolder>()
                .Select(async static folder => await folder.GetFilesAsync(
                    CommonFileQuery.OrderByName))).ConfigureAwait(false);

            files.AddRange(await Task.WhenAll(folderFiles
                .SelectMany(static files => files)
                .Select(static file => file.ToFileAsync())).ConfigureAwait(false));
#endif
            if (sender is UIElement element &&
                GetDropFilesCommand(element) is ICommand command)
            {
                command.Execute(files);
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

#if WPF
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

    // Android requires full type name.
#if WPF
    private static void OnDropText(object sender, DragEventArgs args)
#else
    private static async void OnDropText(object sender, Windows.UI.Xaml.DragEventArgs args)
#endif
    {
#if WPF
        if (args.Data.GetDataPresent(DataFormats.UnicodeText, true) &&
            args.Data.GetData(DataFormats.UnicodeText, true) is string text)
#else
        if (args.DataView.Contains(StandardDataFormats.Text))
#endif
        {
#if !WPF
            var text = await args.DataView.GetTextAsync();
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
