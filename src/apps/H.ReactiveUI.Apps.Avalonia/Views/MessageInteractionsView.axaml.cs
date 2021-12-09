using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using H.ReactiveUI.Apps.ViewModels;

namespace H.ReactiveUI.Apps.Views;

public partial class MessageInteractionsView : UserControl
{
    public MessageInteractionsView()
    {
        InitializeComponent();

        DataContext = new MessageInteractionsViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
