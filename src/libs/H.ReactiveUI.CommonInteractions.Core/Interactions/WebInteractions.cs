namespace H.ReactiveUI;

public static class WebInteractions
{
    public static Interaction<string, Unit> OpenUrl { get; } = new();
}
