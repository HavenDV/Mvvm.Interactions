using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace H.ReactiveUI.Apps.Views;

public partial class MessageInteractionsView : UserControl
{
    public MessageInteractionsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
