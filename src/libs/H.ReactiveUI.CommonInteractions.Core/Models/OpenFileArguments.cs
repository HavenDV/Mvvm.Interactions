namespace H.ReactiveUI;

public class OpenFileArguments
{
    public string SuggestedFileName { get; init; } = string.Empty;
    public string[] Extensions { get; init; } = Array.Empty<string>();
    public string FilterName { get; init; } = string.Empty;
}
