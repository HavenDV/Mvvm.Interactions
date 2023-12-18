#if !HAS_WPF && !HAS_AVALONIA && !HAS_MAUI
using Windows.System;

namespace Mvvm.CommonInteractions;

public partial class WebInteractions
{
    public async Task OpenUrlAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        uri = uri ?? throw new ArgumentNullException(nameof(uri));

        _ = await Launcher.LaunchUriAsync(uri);
    }
}
#endif