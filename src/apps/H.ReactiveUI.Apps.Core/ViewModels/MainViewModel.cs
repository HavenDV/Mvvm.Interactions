namespace H.ReactiveUI.Apps.ViewModels;

public class MainViewModel : ReactiveObject
{
    #region Properties

    public FileInteractionsViewModel FileInteractions { get; set; } = new();
    public MessageInteractionsViewModel MessageInteractions { get; set; } = new();
    public WebInteractionsViewModel WebInteractions { get; set; } = new();

    #endregion
}