namespace H.ReactiveUI.CommonInteractions;

public record FileViewModel(
    string FileName,
    string Extension,
    byte[] Bytes)
{
    public string FileNameWithExtension => $"{FileName}{Extension}";
    public bool IsEmpty => !Bytes.Any();

    public FileViewModel()
        : this(string.Empty, string.Empty, Array.Empty<byte>())
    {
    }
}
