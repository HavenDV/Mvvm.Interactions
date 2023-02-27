#if HAS_MAUI
namespace Mvvm.CommonInteractions;

public partial class WebInteractions
{
    public async Task OpenUrlAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        uri = uri ?? throw new ArgumentNullException(nameof(uri));

        _ = await Launcher.TryOpenAsync(uri).ConfigureAwait(true);
    }
}
#endif