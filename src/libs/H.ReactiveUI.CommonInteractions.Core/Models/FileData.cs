namespace H.ReactiveUI;

public record FileData(
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
}
