using System.Text;

namespace H.ReactiveUI.CommonInteractions.Models;

public class ContentFileData : FileData
{
    #region Properties

    public byte[] Bytes { get; }

    #endregion

    #region Constructors

    public ContentFileData(
        byte[] bytes,
        string? fileName = null) :
        base(fileName ?? string.Empty)
    {
        Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
    }

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

    public override Task<Stream> OpenStreamForReadAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Stream>(new MemoryStream(Bytes));
    }

    #endregion
}
