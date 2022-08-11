namespace HReactiveUI.Apps.Views;

public partial class WebInteractionsView
#if !HAS_WPF
    : WebInteractionsViewBase
#endif
{
    #region Constructors

    public WebInteractionsView()
    {
        InitializeComponent();
    }

    #endregion
}
