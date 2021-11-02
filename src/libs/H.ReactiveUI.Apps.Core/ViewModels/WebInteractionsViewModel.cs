namespace H.ReactiveUI.Apps.ViewModels;

public class WebInteractionsViewModel
{
    #region Properties

    public ReactiveCommand<Unit, Unit> OpenUrl { get; }

    #endregion

    #region Constructors

    public WebInteractionsViewModel()
    {
        OpenUrl = ReactiveCommand.CreateFromObservable(
            () => WebInteractions.OpenUrl.Handle("https://www.google.com/"));
    }

    #endregion
}