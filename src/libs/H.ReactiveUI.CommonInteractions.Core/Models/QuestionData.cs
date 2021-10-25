namespace H.ReactiveUI;

public class QuestionData
{
    #region Properties

    public string Message { get; } = string.Empty;
    public string Title { get; } = string.Empty;

    /// <summary>
    /// This data will not be localized and will be shown as is.
    /// </summary>
    public string AdditionalData { get; } = string.Empty;

    #endregion

    #region Constructors

    public QuestionData(
        string message,
        string? title = null,
        string? additionalData = null)
    {
        Message = message ?? throw new System.ArgumentNullException(nameof(message));
        Title = title ?? string.Empty;
        AdditionalData = additionalData ?? string.Empty;
    }

    #endregion
}
