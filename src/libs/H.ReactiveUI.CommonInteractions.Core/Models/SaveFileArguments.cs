namespace H.ReactiveUI;

public class SaveFileArguments
{
    #region Properties

    public string SuggestedFileName { get; init; } = string.Empty;
    public string Extension { get; init; } = string.Empty;
    public string FilterName { get; init; } = string.Empty;
    public Func<Task<byte[]>>? BytesFunc { get; init; }
    public byte[] Bytes { get; init; } = Array.Empty<byte>();

    #endregion
}