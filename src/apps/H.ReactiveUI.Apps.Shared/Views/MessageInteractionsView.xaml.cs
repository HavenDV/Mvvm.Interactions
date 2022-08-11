namespace HReactiveUI.Apps.Views;

public partial class MessageInteractionsView
#if !HAS_WPF
    : MessageInteractionsViewBase
#endif
{
    #region Constructors

    public MessageInteractionsView()
    {
        InitializeComponent();
    }

    #endregion
}
