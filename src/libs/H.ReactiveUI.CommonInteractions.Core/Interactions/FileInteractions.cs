namespace H.ReactiveUI;

public static class FileInteractions
{
    public static Interaction<OpenFileArguments, FileData?> OpenFile { get; } = new();
    public static Interaction<OpenFileArguments, FileData[]> OpenFiles { get; } = new();
    public static Interaction<SaveFileArguments, FileData?> SaveFile { get; } = new();

    public static Interaction<string, FileData> CreateTemporaryFile { get; } = new();
    public static Interaction<string, FileData> OpenPath { get; } = new();

    public static Interaction<string, Unit> LaunchFolder { get; } = new();
}
