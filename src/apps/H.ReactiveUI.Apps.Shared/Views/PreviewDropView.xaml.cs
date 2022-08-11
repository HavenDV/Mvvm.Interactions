namespace HReactiveUI.Apps.Views;

public partial class PreviewDropView
#if !HAS_WPF
    : PreviewDropViewBase
#endif
{
    #region Constructors

    public PreviewDropView()
    {
        InitializeComponent();
    }

    #endregion
}
