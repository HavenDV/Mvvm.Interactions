using System.Text;

namespace H.ReactiveUI;

public class FileData
{
    #region Properties

    public string FullPath { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public string Extension { get; init; } = string.Empty;
    public string FileNameWithoutExtension { get; init; } = string.Empty;

    public byte[] Bytes { get; init; } = Array.Empty<byte>();
    public bool IsEmpty => !Bytes.Any();

    /// <summary>
    /// Uses <see cref="Encoding.UTF8"/> to convert.
    /// </summary>
    public string Text
    {
        get => Encoding.UTF8.GetString(Bytes);
        init => Bytes = Encoding.UTF8.GetBytes(value);
    }

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
}
