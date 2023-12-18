using System.Diagnostics;

namespace Mvvm.Interactions;

public partial class MessageInteractions : InteractionsBase, IMessageInteractions
{
    #region Constructors

    public MessageInteractions(
        Func<string, string>? localizationFunc = null) :
        base(localizationFunc)
    {
    }

    #endregion

    #region Methods

    private string GetMessage(string text)
    {
        var message = Localize(text);

        Trace.WriteLine($"Message: {message}");

        return message;
    }

    private string GetWarning(string text)
    {
        var warning = Localize(text);

        Trace.WriteLine($"Warning: {warning}");

        return warning;
    }

    private string GetError(string text)
    {
        var error = Localize(text);

        Trace.WriteLine($"Error: {error}");

        return error;
    }

    private static Exception GetException(Exception exception)
    {
        Trace.WriteLine($"Exception: {exception}");

        return exception;
    }

    private (string Title, string Body) GetQuestion(QuestionData question)
    {
        var message = Localize(question.Message);
        var title = Localize(question.Title);
        var body = message;
        if (!string.IsNullOrWhiteSpace(question.AdditionalData))
        {
            body += Environment.NewLine + question.AdditionalData;
        }

        Trace.WriteLine($@"Question: {title}
{body}");

        return (title, body);
    }

    #endregion
}
