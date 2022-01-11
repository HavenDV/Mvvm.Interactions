namespace H.ReactiveUI.Apps.ViewModels;

public class FileInteractionsViewModel : ReactiveObject
{
    #region Properties

    [Reactive]
    public FileData? SelectedFile { get; set; }

    [Reactive]
    public string Content { get; set; } = string.Empty;

    public PreviewDropViewModel PreviewDropViewModel { get; set; } = new();

    #endregion

    #region Commands

    public ReactiveCommand<Unit, Unit> OpenFolder { get; }
    public ReactiveCommand<Unit, Unit> OpenFile { get; }
    public ReactiveCommand<Unit, Unit> OpenFiles { get; }
    public ReactiveCommand<Unit, Unit> SaveFile { get; }
    public ReactiveCommand<Unit, Unit> SaveFileAs { get; }

    public ReactiveCommand<Unit, Unit> CreateTemporaryFileAndLaunch { get; }

    public ReactiveCommand<FileData[], Unit> DragFilesEnter { get; }
    public ReactiveCommand<string, Unit> DragTextEnter { get; }
    public ReactiveCommand<Unit, Unit> DragLeave { get; }
    public ReactiveCommand<FileData[], Unit> DropFiles { get; }
    public ReactiveCommand<string, Unit> DropText { get; }

    #endregion

    #region Constructors

    public FileInteractionsViewModel()
    {
        OpenFolder = ReactiveCommand.CreateFromTask(async () =>
        {
            var _ = await FileInteractions.OpenFolder.Handle(new OpenFolderArguments
            {
            });
        });
        OpenFile = ReactiveCommand.CreateFromTask(async () =>
        {
            var file = await FileInteractions.OpenFile.Handle(new OpenFileArguments
            {
                //Extensions = new[] { ".txt" },
                //FilterName = "Txt files",
                //SuggestedFileName = "text",
            });
            if (file == null)
            {
                return;
            }

            SelectedFile = file;
            Content = await SelectedFile.ReadTextAsync().ConfigureAwait(true);
        });
        OpenFiles = ReactiveCommand.CreateFromTask(async () =>
        {
            var files = await FileInteractions.OpenFiles.Handle(new OpenFileArguments
            {
                //Extensions = new[] { ".txt" },
                //FilterName = "Txt files",
                //SuggestedFileName = "text",
            });
            if (!files.Any())
            {
                return;
            }

            SelectedFile = files.First();
            Content = await SelectedFile.ReadTextAsync().ConfigureAwait(true);
        });
        SaveFile = ReactiveCommand.CreateFromTask(async () =>
        {
            if (SelectedFile == null)
            {
                return;
            }

            await SelectedFile.WriteTextAsync(Content).ConfigureAwait(false);
        }, this
            .WhenAnyValue(static x => x.SelectedFile)
            .Select(static x => x != null));
        SaveFileAs = ReactiveCommand.CreateFromTask(async () =>
        {
            var file = await FileInteractions.SaveFile.Handle(new SaveFileArguments(".txt")
            {
                //FilterName = "Txt files",
                //SuggestedFileName = "text",
            });
            if (file == null)
            {
                return;
            }

            SelectedFile = file;
            await SelectedFile.WriteTextAsync(Content).ConfigureAwait(false);
        });

        CreateTemporaryFileAndLaunch = ReactiveCommand.CreateFromTask(async () =>
        {
            var file = await FileInteractions.CreateTemporaryFile.Handle("TemporaryFile.txt");
            await file.WriteTextAsync(Content).ConfigureAwait(false);

            await file.LaunchAsync().ConfigureAwait(false);
        });

        DragFilesEnter = ReactiveCommand.Create<FileData[]>(files =>
        {
            PreviewDropViewModel.Name = "Drop to open this files:";
            PreviewDropViewModel.Content = string.Join(Environment.NewLine, files.Select(static x => x.FileName));
            PreviewDropViewModel.IsVisible = true;
        });
        DragTextEnter = ReactiveCommand.Create<string>(text =>
        {
            PreviewDropViewModel.Name = "Drop to copy this text:";
            PreviewDropViewModel.Content = text;
            PreviewDropViewModel.IsVisible = true;
        });
        DragLeave = ReactiveCommand.Create(() =>
        {
            PreviewDropViewModel.IsVisible = false;
        });
        DropFiles = ReactiveCommand.CreateFromTask<FileData[]>(async files =>
        {
            PreviewDropViewModel.IsVisible = false;

            SelectedFile = files.First();
            Content = await SelectedFile.ReadTextAsync().ConfigureAwait(true);
        });
        DropText = ReactiveCommand.Create<string>(text =>
        {
            PreviewDropViewModel.IsVisible = false;
            Content = text;
        });
    }

    #endregion
}