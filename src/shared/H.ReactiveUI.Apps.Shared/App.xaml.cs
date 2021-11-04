using H.ReactiveUI.Apps.Views;
#if !HAS_WPF
using Windows.ApplicationModel.Activation;
#endif

#nullable enable

namespace H.ReactiveUI.Apps;

public sealed partial class App
{
    #region Properties

    //public IHost Host { get; }
    private InteractionManager InteractionManager { get; } = new();

    #endregion

    #region Constructors

    public App()
    {
        InteractionManager.Register();
        InteractionManager.CatchUnhandledExceptions(this);

//        Host = Initialization.HostBuilder
//            .Create()
//            .AddViews()
//            .AddConverters()
//            .AddPlatformSpecificLoggers()
//#if __WASM__
//            .RemoveFileWatchers()
//#endif
//            .Build();

#if !HAS_WPF
        InitializeComponent();
#endif
    }

    #endregion

    #region Event Handlers

    //private IViewFor GetView<T>(out T viewModel) where T : notnull
    //{
    //    viewModel = Host.Services.GetRequiredService<T>();
    //    var view = Host.Services
    //        .GetRequiredService<IViewLocator>()
    //        .ResolveView(viewModel) ??
    //        throw new InvalidOperationException("View is null.");

    //    view.ViewModel = viewModel;
    //    return view;
    //}

#if !HAS_WPF

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
#if HAS_WINUI
        var window = new Window();
#else
        var window = Window.Current;
#endif
        if (window.Content is not Frame frame)
        {
            frame = new Frame();

            window.Content = frame;
        }

#if !HAS_WINUI
        if (args.PrelaunchActivated)
        {
            return;
        }
#endif

        if (frame.Content is null)
        {
            frame.Content = new MainView();
        }

        window.Activate();
    }

#endif

#endregion
}
