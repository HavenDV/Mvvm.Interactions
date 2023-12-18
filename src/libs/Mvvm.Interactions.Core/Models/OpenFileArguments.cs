namespace Mvvm.Interactions;

/// <summary>
/// 
/// </summary>
public class OpenFileArguments
{
    #region Properties

    /// <summary>
    /// Default: <see cref="string.Empty"/>. <br/>
    /// Supported platfroms: WPF/Avalonia.
    /// </summary>
    public string SuggestedFileName { get; init; } = string.Empty;

    /// <summary>
    /// Dot is required. <br/>
    /// Example: '.txt'. <br/>
    /// Default: <see cref="Array.Empty"/>. <br/>
    /// Supported platfroms: WPF/UWP/WinUI/Uno/Avalonia.
    /// </summary>
    public string[] Extensions { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Example: 'Text files'. <br/>
    /// Default: <see cref="string.Empty"/>. <br/>
    /// Supported platfroms: WPF/Avalonia.
    /// </summary>
    public string FilterName { get; init; } = string.Empty;

    #endregion
}
