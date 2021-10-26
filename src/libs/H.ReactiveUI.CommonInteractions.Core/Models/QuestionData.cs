namespace H.ReactiveUI;

public class QuestionData
{
    #region Properties

    public string Message { get; init; } = string.Empty;

    public string Title { get; init; } = "Question:";

    /// <summary>
    /// This data will not be localized and will be shown as is.
    /// </summary>
    public string AdditionalData { get; init; } = string.Empty;

    #endregion

    #region Constructors

    public QuestionData()
    {
    }

    public QuestionData(string message)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    #endregion
}
