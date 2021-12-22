namespace H.ReactiveUI.Apps.ViewModels;

public class PreviewDropViewModel : ReactiveObject
{
    #region Properties

    #region Public

    [Reactive]
    public bool IsVisible { get; set; }

    [Reactive]
    public string Name { get; set; } = string.Empty;

    [Reactive]
    public string Content { get; set; } = string.Empty;

    #endregion

    #endregion
}
