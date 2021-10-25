namespace H.ReactiveUI;

public static class FileInteractions
{
    public static Interaction<(string[] Extensions, string FilterName), FileData> OpenFile { get; } = new();
    public static Interaction<(string[] Extensions, string FilterName), FileData[]> OpenFiles { get; } = new();
    public static Interaction<FileData, string?> SaveFile { get; } = new();
    public static Interaction<FileData, Unit> LaunchInTemp { get; } = new();
    public static Interaction<string, Unit> LaunchPath { get; } = new();
    public static Interaction<string, Unit> LaunchFolder { get; } = new();
}
