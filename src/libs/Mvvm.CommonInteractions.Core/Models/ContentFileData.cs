using System.Text;

namespace Mvvm.CommonInteractions.Models;

/// <summary>
/// 
/// </summary>
public class ContentFileData : FileData
{
    #region Properties

    /// <summary>
    /// 
    /// </summary>
    public byte[] Bytes { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="fileName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ContentFileData(
        byte[] bytes,
        string? fileName = null) :
        base(fileName ?? string.Empty)
    {
        Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="encoding"></param>
    /// <param name="fileName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ContentFileData(
        string text,
        Encoding? encoding = null,
        string? fileName = null) :
        base(fileName ?? string.Empty)
    {
        text = text ?? throw new ArgumentNullException(nameof(text));
        encoding ??= Encoding.UTF8;

        Bytes = encoding.GetBytes(text);
    }

    #endregion

    #region Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task<Stream> OpenStreamForReadAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Stream>(new MemoryStream(Bytes));
    }

    #endregion
}
