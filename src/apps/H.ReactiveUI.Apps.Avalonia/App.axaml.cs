using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using H.ReactiveUI.Apps.ViewModels;
using H.ReactiveUI.Apps.Views;

namespace H.ReactiveUI.Apps;

public class App : Application
{
    #region Properties

    private InteractionManager InteractionManager { get; } = new();

    #endregion

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        InteractionManager.Register();
        InteractionManager.CatchUnhandledExceptions(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var view = new MainView();
            desktop.MainWindow = new MainView();
            FileInteractionManager.Window = view;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
