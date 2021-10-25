using System;
using System.IO;
#if WPF
#else
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
#endif

namespace H.ReactiveUI.CommonInteractions;

public static class IOExtensions
{
#if WPF
    public static FileViewModel ToFile(
        this string path)
    {
        path = path ?? throw new ArgumentNullException(nameof(path));

        return new(
            Path.GetFileNameWithoutExtension(path),
            Path.GetExtension(path),
            File.ReadAllBytes(path));
    }
#else
    public static async Task<FileViewModel> ToFileAsync(
        this StorageFile file,
        CancellationToken cancellationToken = default)
    {
        file = file ?? throw new ArgumentNullException(nameof(file));

        byte[] bytes;
        using (var stream = await file.OpenStreamForReadAsync().ConfigureAwait(false))
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(
                memoryStream, 81920, cancellationToken).ConfigureAwait(false);

            bytes = memoryStream.ToArray();
        }

        var path = file.Name;

        return new(
            Path.GetFileNameWithoutExtension(path),
            Path.GetExtension(path),
            bytes);
    }
#endif
}
