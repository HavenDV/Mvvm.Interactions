using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using H.ReactiveUI.Apps.ViewModels;

namespace H.ReactiveUI.Apps.Views;

public partial class WebInteractionsView : UserControl
{
    public WebInteractionsView()
    {
        InitializeComponent();

        DataContext = new WebInteractionsViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
