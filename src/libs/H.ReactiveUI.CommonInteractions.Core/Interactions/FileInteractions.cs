namespace H.ReactiveUI;

/// <summary>
/// 
/// </summary>
public static class FileInteractions
{
    /// <summary>
    /// 
    /// </summary>
    public static Interaction<OpenFolderArguments, FolderData?> OpenFolder { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    public static Interaction<OpenFileArguments, FileData?> OpenFile { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    public static Interaction<OpenFileArguments, FileData[]> OpenFiles { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    public static Interaction<SaveFileArguments, FileData?> SaveFile { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    public static Interaction<string, FileData> CreateTemporaryFile { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    public static Interaction<string, FileData> OpenPath { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    public static Interaction<string, Unit> LaunchFolder { get; } = new();
}
