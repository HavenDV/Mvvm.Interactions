using System.Windows;

namespace H.ReactiveUI.Apps.Wpf;

public partial class App : Application
{
    public InteractionManager InteractionManager { get; } = new();

    public App()
    {
        InteractionManager.Register();
        InteractionManager.CatchUnhandledExceptions(this);
    }
}