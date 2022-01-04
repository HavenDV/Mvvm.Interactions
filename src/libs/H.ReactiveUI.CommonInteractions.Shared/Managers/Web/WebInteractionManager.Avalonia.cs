#if HAS_AVALONIA
using System.Reactive;
using System.Diagnostics;

namespace H.ReactiveUI;

#pragma warning disable CA1822 // Mark members as static

public partial class WebInteractionManager
{
    public void Register()
    {
        _ = WebInteractions.OpenUrl.RegisterHandler(static context =>
        {
            var url = context.Input;

            _ = Process.Start(new ProcessStartInfo(url)
            {
                UseShellExecute = true,
            });

            context.SetOutput(Unit.Default);
        });
    }
}
#endif