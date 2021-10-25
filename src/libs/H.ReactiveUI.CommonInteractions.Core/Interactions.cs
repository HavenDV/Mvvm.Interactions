namespace H.ReactiveUI.CommonInteractions;

public static class Interactions
{
    public static Interaction<string, Unit> Message { get; } = new();
    public static Interaction<string, Unit> Warning { get; } = new();
    public static Interaction<Exception, Unit> Exception { get; } = new();
    public static Interaction<QuestionViewModel, bool> Question { get; } = new();
    public static Interaction<(string[] Extensions, string FilterName), FileViewModel> OpenFile { get; } = new();
    public static Interaction<(string[] Extensions, string FilterName), FileViewModel[]> OpenFiles { get; } = new();
    public static Interaction<FileViewModel, string?> SaveFile { get; } = new();
    public static Interaction<FileViewModel, Unit> LaunchInTemp { get; } = new();
    public static Interaction<string, Unit> LaunchPath { get; } = new();
    public static Interaction<string, Unit> LaunchFolder { get; } = new();
    public static Interaction<string, Unit> OpenUrl { get; } = new();
}
