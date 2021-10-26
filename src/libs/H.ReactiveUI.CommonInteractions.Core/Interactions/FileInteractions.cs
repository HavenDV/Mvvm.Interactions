namespace H.ReactiveUI;

public static class FileInteractions
{
    public static Interaction<OpenFileArguments, FileData?> OpenFile { get; } = new();
    public static Interaction<OpenFileArguments, FileData[]> OpenFiles { get; } = new();
    public static Interaction<SaveFileArguments, string?> SaveFile { get; } = new();
    public static Interaction<SaveOpenFileArguments, string?> SaveOpenFile { get; } = new();

    public static Interaction<FileData, Unit> LaunchInTemp { get; } = new();
    public static Interaction<string, Unit> LaunchPath { get; } = new();
    public static Interaction<string, Unit> LaunchFolder { get; } = new();
}
