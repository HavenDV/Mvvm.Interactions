namespace H.ReactiveUI.Apps.ViewModels;

public class FileInteractionsViewModel : ReactiveObject
{
    #region Properties

    [Reactive]
    public string Name { get; set; } = "None";

    [Reactive]
    public string Content { get; set; } = string.Empty;

    public PreviewDropViewModel PreviewDropViewModel { get; set; } = new();

    #endregion

    #region Commands

    public ReactiveCommand<Unit, FileData?> OpenFile { get; }
    public ReactiveCommand<Unit, FileData[]> OpenFiles { get; }
    public ReactiveCommand<Unit, string?> SaveFile { get; }
    public ReactiveCommand<Unit, string?> SaveOpenFile { get; }

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
        OpenFile = ReactiveCommand.CreateFromObservable(() =>
            FileInteractions.OpenFile.Handle(new OpenFileArguments()));
        OpenFiles = ReactiveCommand.CreateFromObservable(() =>
            FileInteractions.OpenFiles.Handle(new OpenFileArguments()));
        SaveFile = ReactiveCommand.CreateFromObservable(() =>
            FileInteractions.SaveFile.Handle(new SaveFileArguments()));
        SaveOpenFile = ReactiveCommand.CreateFromObservable(() =>
            FileInteractions.SaveOpenFile.Handle(new SaveOpenFileArguments()));

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
        });
        DropText = ReactiveCommand.Create<string>(text =>
        {
            PreviewDropViewModel.IsVisible = false;
            Content = text;
        });
    }

    #endregion
}