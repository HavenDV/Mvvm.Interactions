namespace H.ReactiveUI.Apps.ViewModels;

public class MessageInteractionsViewModel
{
    #region Properties

    public ReactiveCommand<Unit, Unit> Message { get; }
    public ReactiveCommand<Unit, Unit> Warning { get; }
    public ReactiveCommand<Unit, Unit> Exception { get; }
    public ReactiveCommand<Unit, bool> Question { get; }

    #endregion

    #region Constructors

    public MessageInteractionsViewModel()
    {
        Message = ReactiveCommand.CreateFromObservable(
            () => MessageInteractions.Message.Handle("https://www.google.com/"));
        Warning = ReactiveCommand.CreateFromObservable(
            () => MessageInteractions.Warning.Handle("https://www.google.com/"));
        Exception = ReactiveCommand.CreateFromObservable(
            () => MessageInteractions.Exception.Handle(new InvalidOperationException("Test")));
        Question = ReactiveCommand.CreateFromObservable(
            () => MessageInteractions.Question.Handle(new QuestionData("Are you sure?")));
    }

    #endregion
}
