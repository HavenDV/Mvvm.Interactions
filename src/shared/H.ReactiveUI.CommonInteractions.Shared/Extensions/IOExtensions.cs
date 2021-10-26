#if WPF
using System.IO;
#else
using Windows.Storage;
#endif

namespace H.ReactiveUI;

public static class IOExtensions
{
#if WPF
    public static FileData ToFile(
        this string path)
    {
        path = path ?? throw new ArgumentNullException(nameof(path));

        return new FileData(path)
        {
            Bytes = File.ReadAllBytes(path),
        };
    }
#else
    public static async Task<FileData> ToFileAsync(
        this StorageFile file,
        CancellationToken cancellationToken = default)
    {
        file = file ?? throw new ArgumentNullException(nameof(file));

        byte[] bytes;
        using (var stream = await file.OpenStreamForReadAsync().ConfigureAwait(true))
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(
                memoryStream, 81920, cancellationToken).ConfigureAwait(true);

            bytes = memoryStream.ToArray();
        }

        var path = file.Name;
        
        return new FileData(path)
        {
            Bytes = bytes,
        };
    }
#endif
}
