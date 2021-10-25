using System.Text;

namespace H.ReactiveUI;

public readonly record struct FileData(
    string FileName,
    string Extension,
    byte[] Bytes)
{
    public string FileNameWithExtension => $"{FileName}{Extension}";
    public bool IsEmpty => !Bytes.Any();

    public FileData()
        : this(string.Empty, string.Empty, Array.Empty<byte>())
    {
    }

    public FileData(string fileName, string extension, string content, Encoding? encoding = null)
        : this(fileName, extension, (encoding ?? Encoding.UTF8).GetBytes(content))
    {
    }

    public string GetString(Encoding? encoding = null)
    {
        return (encoding ?? Encoding.UTF8).GetString(Bytes);
    }
}
