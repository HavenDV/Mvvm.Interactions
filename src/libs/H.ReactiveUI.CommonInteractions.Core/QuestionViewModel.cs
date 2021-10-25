namespace H.ReactiveUI.CommonInteractions;

public class QuestionViewModel
{
    #region Properties

    public string Message { get; } = $"{nameof(QuestionViewModel)}.DefaultMessage";
    public string Title { get; } = $"{nameof(QuestionViewModel)}.DefaultTitle";

    /// <summary>
    /// This data will not be localized and will be shown as is.
    /// </summary>
    public string AdditionalData { get; } = string.Empty;

    #endregion

    #region Constructors

    public QuestionViewModel(
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
