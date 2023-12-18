#if HAS_AVALONIA
using System.Diagnostics;

namespace Mvvm.Interactions;

public partial class WebInteractions
{
    public Task OpenUrlAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        uri = uri ?? throw new ArgumentNullException(nameof(uri));

        _ = Process.Start(new ProcessStartInfo(uri.OriginalString)
        {
            UseShellExecute = true,
        });

        return Task.CompletedTask;
    }
}
#endif