using System.Text;

namespace H.ReactiveUI;

public class FileData
{
    #region Properties

    public string FullPath { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public string Extension { get; init; } = string.Empty;
    public string FileNameWithoutExtension { get; init; } = string.Empty;

    #endregion

    #region Constructors

    public FileData()
    {
    }

    public FileData(string path)
    {
        FullPath = path ?? throw new ArgumentNullException(nameof(path));
        FileName = Path.GetFileName(FullPath);
        Extension = Path.GetExtension(FullPath);
        FileNameWithoutExtension = Path.GetFileNameWithoutExtension(FullPath);
    }

    #endregion

    #region Methods

    public async Task<byte[]> ReadBytesAsync(
        CancellationToken cancellationToken = default)
    {
        using var stream = await OpenStreamForReadAsync(cancellationToken).ConfigureAwait(false);
        using var memoryStream = new MemoryStream();

        await stream.CopyToAsync(
            memoryStream,
            81920,
            cancellationToken).ConfigureAwait(false);

        return memoryStream.ToArray();
    }

    public async Task WriteBytesAsync(
        byte[] bytes,
        CancellationToken cancellationToken = default)
    {
        bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));

        using var stream = await OpenStreamForWriteAsync(cancellationToken).ConfigureAwait(false);
        using var memoryStream = new MemoryStream(bytes);

        await memoryStream.CopyToAsync(
            stream,
            81920,
            cancellationToken).ConfigureAwait(false);
    }

    public async Task<string> ReadTextAsync(
        Encoding? encoding = null,
        CancellationToken cancellationToken = default)
    {
        encoding ??= Encoding.UTF8;

        var bytes = await ReadBytesAsync(cancellationToken).ConfigureAwait(false);

        return encoding.GetString(bytes);
    }

    public async Task WriteTextAsync(
        string text,
        Encoding? encoding = null,
        CancellationToken cancellationToken = default)
    {
        text = text ?? throw new ArgumentNullException(nameof(text));
        encoding ??= Encoding.UTF8;

        var bytes = encoding.GetBytes(text);

        await WriteBytesAsync(bytes, cancellationToken).ConfigureAwait(false);
    }

    #endregion

    #region Virtual Methods

    public virtual Task<Stream> OpenStreamForReadAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromException<Stream>(new NotImplementedException());
    }

    public virtual Task<Stream> OpenStreamForWriteAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromException<Stream>(new NotImplementedException());
    }

    public virtual Task LaunchAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromException(new NotImplementedException());
    }

    #endregion
}
