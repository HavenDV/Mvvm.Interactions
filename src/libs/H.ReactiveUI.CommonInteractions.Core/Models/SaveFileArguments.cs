using System.Text;

namespace H.ReactiveUI;

public class SaveFileArguments
{
    #region Properties

    public string SuggestedFileName { get; init; } = string.Empty;
    public string Extension { get; init; } = string.Empty;
    public string FilterName { get; init; } = string.Empty;
    public Func<Task<byte[]>>? BytesFunc { get; init; }
    public byte[] Bytes { get; init; } = Array.Empty<byte>();

    /// <summary>
    /// Uses <see cref="Encoding.UTF8"/> to convert.
    /// </summary>
    public Func<Task<string>> TextFunc
    {
        get => () => Task.FromResult(string.Empty);
        init => BytesFunc = async () => Encoding.UTF8.GetBytes(await value().ConfigureAwait(false));
    }

    /// <summary>
    /// Uses <see cref="Encoding.UTF8"/> to convert.
    /// </summary>
    public string Text
    {
        get => Encoding.UTF8.GetString(Bytes);
        init => Bytes = Encoding.UTF8.GetBytes(value);
    }

    #endregion
}