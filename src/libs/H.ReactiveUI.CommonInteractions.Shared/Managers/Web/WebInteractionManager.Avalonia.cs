#if HAS_AVALONIA
using System.Reactive;
using System.Diagnostics;

namespace H.ReactiveUI;

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