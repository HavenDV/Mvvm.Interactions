namespace H.ReactiveUI.Apps.ViewModels;

public class FileInteractionsViewModel : ReactiveObject
{
    #region Properties

    [Reactive]
    public string Name { get; set; } = "None";

    [Reactive]
    public string Content { get; set; } = string.Empty;

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
        OpenFile = ReactiveCommand.CreateFromObservable(
            () => FileInteractions.OpenFile.Handle(new OpenFileArguments()));
        OpenFiles = ReactiveCommand.CreateFromObservable(
            () => FileInteractions.OpenFiles.Handle(new OpenFileArguments()));
        SaveFile = ReactiveCommand.CreateFromObservable(
            () => FileInteractions.SaveFile.Handle(new SaveFileArguments()));
        SaveOpenFile = ReactiveCommand.CreateFromObservable(
            () => FileInteractions.SaveOpenFile.Handle(new SaveOpenFileArguments()));

        LaunchInTemp = ReactiveCommand.CreateFromObservable(
            () => FileInteractions.LaunchInTemp.Handle(new FileData()));
        LaunchPath = ReactiveCommand.CreateFromObservable(
            () => FileInteractions.LaunchPath.Handle("Are you sure?"));
        LaunchFolder = ReactiveCommand.CreateFromObservable(
            () => FileInteractions.LaunchFolder.Handle("Are you sure?"));

        DragFilesEnter = ReactiveCommand.Create<FileData[]>(
            static files => { });
        DragTextEnter = ReactiveCommand.Create<string>(
            static text => { });
        DragLeave = ReactiveCommand.Create(
            static () => { });
        DropFiles = ReactiveCommand.Create<FileData[]>(
            static files => { });
        DropText = ReactiveCommand.Create<string>(
            text => Content = text);
    }

    #endregion
}