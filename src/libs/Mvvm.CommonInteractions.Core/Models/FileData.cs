using System.Text;

namespace Mvvm.CommonInteractions;

/// <summary>
/// 
/// </summary>
public class FileData
{
    #region Properties

    /// <summary>
    /// 
    /// </summary>
    public string FullPath { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string FileName { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Extension { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string FileNameWithoutExtension { get; init; } = string.Empty;

    #endregion

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    public FileData()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public FileData(string path)
    {
        FullPath = path ?? throw new ArgumentNullException(nameof(path));
        FileName = Path.GetFileName(FullPath);
        Extension = Path.GetExtension(FullPath);
        FileNameWithoutExtension = Path.GetFileNameWithoutExtension(FullPath);
    }

    #endregion

    #region Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<byte[]> ReadBytesAsync(
        CancellationToken cancellationToken = default)
    {
        using var stream = await OpenStreamForReadAsync(cancellationToken).ConfigureAwait(false);
        using var memoryStream = new MemoryStream();

        await stream.CopyToAsync(
            destination: memoryStream,
            bufferSize: 81920,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return memoryStream.ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task WriteBytesAsync(
        byte[] bytes,
        CancellationToken cancellationToken = default)
    {
        bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));

        using var stream = await OpenStreamForWriteAsync(cancellationToken).ConfigureAwait(false);
        using var memoryStream = new MemoryStream(bytes);

        await memoryStream.CopyToAsync(
            destination: stream,
            bufferSize: 81920,
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="encoding"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> ReadTextAsync(
        Encoding? encoding = null,
        CancellationToken cancellationToken = default)
    {
        encoding ??= Encoding.UTF8;

        var bytes = await ReadBytesAsync(cancellationToken).ConfigureAwait(false);

        return encoding.GetString(bytes);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="encoding"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual Task<Stream> OpenStreamForReadAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromException<Stream>(new NotImplementedException());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual Task<Stream> OpenStreamForWriteAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromException<Stream>(new NotImplementedException());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual Task LaunchAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromException(new NotImplementedException());
    }

    #endregion
}
