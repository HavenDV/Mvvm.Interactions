namespace HReactiveUI.Apps.Views;

public partial class FileInteractionsView
#if !HAS_WPF
    : FileInteractionsViewBase
#endif
{
    #region Constructors

    public FileInteractionsView()
    {
        InitializeComponent();
    }

    #endregion
}
