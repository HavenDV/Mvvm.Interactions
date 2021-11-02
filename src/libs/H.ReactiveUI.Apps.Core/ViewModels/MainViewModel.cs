namespace H.ReactiveUI.Apps.Core.ViewModels;

public class MainViewModel
{
    public FileInteractionsViewModel FileInteractionsViewModel { get; } = new();
    public MessageInteractionsViewModel MessageInteractionsViewModel { get; } = new();
    public WebInteractionsViewModel WebInteractionsViewModel { get; } = new();
}