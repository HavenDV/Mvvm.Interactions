namespace H.ReactiveUI;

/// <summary>
/// 
/// </summary>
public static class MessageInteractions
{
    /// <summary>
    /// 
    /// </summary>
    public static Interaction<string, Unit> Message { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    public static Interaction<string, Unit> Warning { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    public static Interaction<string, Unit> Error { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    public static Interaction<Exception, Unit> Exception { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    public static Interaction<QuestionData, bool> Question { get; } = new();
}
