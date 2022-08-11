namespace HReactiveUI.Apps.Views;

public partial class MainView
#if !HAS_WPF
    : MainViewBase
#endif
{
    #region Constructors

    public MainView()
    {
        InitializeComponent();
    }

    #endregion
}
