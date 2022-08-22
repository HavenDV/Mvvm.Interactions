namespace Mvvm.CommonInteractions;

/// <summary>
/// 
/// </summary>
public class QuestionData
{
    #region Properties

    /// <summary>
    /// 
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Title { get; init; } = "Question:";

    /// <summary>
    /// This data will not be localized and will be shown as is.
    /// </summary>
    public string AdditionalData { get; init; } = string.Empty;

    #endregion

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    public QuestionData()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public QuestionData(string message)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    #endregion
}
