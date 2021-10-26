using System.Text;

namespace H.ReactiveUI;

public class SaveOpenFileArguments
{
    #region Properties

    public string FullPath { get; init; } = string.Empty;

    public byte[] Bytes { get; init; } = Array.Empty<byte>();

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