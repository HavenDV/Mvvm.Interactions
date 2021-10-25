namespace H.ReactiveUI;

public static class MessageInteractions
{
    public static Interaction<string, Unit> Message { get; } = new();
    public static Interaction<string, Unit> Warning { get; } = new();
    public static Interaction<Exception, Unit> Exception { get; } = new();
    public static Interaction<QuestionData, bool> Question { get; } = new();
}
