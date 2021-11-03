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

    public ReactiveCommand<Unit, Unit> OpenFile { get; }
    public ReactiveCommand<Unit, Unit> OpenFiles { get; }
    public ReactiveCommand<Unit, Unit> SaveFile { get; }
    public ReactiveCommand<Unit, Unit> SaveOpenFile { get; }

    public ReactiveCommand<Unit, Unit> LaunchInTemp { get; }
    public ReactiveCommand<Unit, Unit> LaunchPath { get; }
    public ReactiveCommand<Unit, Unit> LaunchFolder { get; }

    public ReactiveCommand<FileData[], Unit> DragFilesEnter { get; }
    public ReactiveCommand<string, Unit> DragTextEnter { get; }
    public ReactiveCommand<Unit, Unit> DragLeave { get; }
    public ReactiveCommand<FileData[], Unit> DropFiles { get; }
    public ReactiveCommand<string, Unit> DropText { get; }

    #endregion

    #region Constructors

    public FileInteractionsViewModel()
    {
        OpenFile = ReactiveCommand.CreateFromTask(async () =>
        {
            var file = await FileInteractions.OpenFile.Handle(new OpenFileArguments());
            if (file == null)
            {
                return;
            }

            SelectedFile = file;
            Content = file.Text;
        });
        OpenFiles = ReactiveCommand.CreateFromTask(async () =>
        {
            var files = await FileInteractions.OpenFiles.Handle(new OpenFileArguments());
            if (!files.Any())
            {
                return;
            }

            SelectedFile = files.First();
            Content = SelectedFile.Text;
        });
        SaveFile = ReactiveCommand.CreateFromTask(async () =>
        {
            if (SelectedFile == null)
            {
                return;
            }

            var _ = await FileInteractions.SaveFile.Handle(new SaveFileArguments
            {
                Text = Content,
            });
        });
        SaveOpenFile = ReactiveCommand.CreateFromTask(async () =>
        {
            var _ = await FileInteractions.SaveOpenFile.Handle(new SaveOpenFileArguments());
        });

        LaunchInTemp = ReactiveCommand.CreateFromObservable(() =>
            FileInteractions.LaunchInTemp.Handle(new FileData()));
        LaunchPath = ReactiveCommand.CreateFromObservable(() =>
            FileInteractions.LaunchPath.Handle("Are you sure?"));
        LaunchFolder = ReactiveCommand.CreateFromObservable(() =>
            FileInteractions.LaunchFolder.Handle("Are you sure?"));

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
        DropFiles = ReactiveCommand.Create<FileData[]>(files =>
        {
            PreviewDropViewModel.IsVisible = false;

            SelectedFile = files.First();
            Content = SelectedFile.Text;
        });
        DropText = ReactiveCommand.Create<string>(text =>
        {
            PreviewDropViewModel.IsVisible = false;
            Content = text;
        });
    }

    #endregion
}