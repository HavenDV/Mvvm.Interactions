using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace H.ReactiveUI.Apps.Views;

public partial class WebInteractionsView : UserControl
{
    public WebInteractionsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
