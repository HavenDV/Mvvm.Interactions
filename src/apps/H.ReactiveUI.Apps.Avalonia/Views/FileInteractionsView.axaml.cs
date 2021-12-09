using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using H.ReactiveUI.Apps.ViewModels;

namespace H.ReactiveUI.Apps.Views;

public partial class FileInteractionsView : UserControl
{
    public FileInteractionsView()
    {
        InitializeComponent();

        DataContext = new FileInteractionsViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
