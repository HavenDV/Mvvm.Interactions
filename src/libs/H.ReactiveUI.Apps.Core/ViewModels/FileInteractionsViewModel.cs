namespace H.ReactiveUI.Apps.ViewModels;

public class FileInteractionsViewModel
{
    #region Properties

    public ReactiveCommand<Unit, FileData?> OpenFile { get; }
    public ReactiveCommand<Unit, FileData[]> OpenFiles { get; }
    public ReactiveCommand<Unit, string?> SaveFile { get; }
    public ReactiveCommand<Unit, string?> SaveOpenFile { get; }

    public ReactiveCommand<Unit, Unit> LaunchInTemp { get; }
    public ReactiveCommand<Unit, Unit> LaunchPath { get; }
    public ReactiveCommand<Unit, Unit> LaunchFolder { get; }

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
    }

    #endregion
}