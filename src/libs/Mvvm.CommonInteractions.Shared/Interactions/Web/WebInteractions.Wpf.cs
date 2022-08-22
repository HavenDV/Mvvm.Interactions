#if HAS_WPF
using System.Diagnostics;

namespace Mvvm.CommonInteractions;

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