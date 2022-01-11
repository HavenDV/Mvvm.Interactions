namespace H.ReactiveUI;

/// <summary>
/// 
/// </summary>
public class OpenFolderArguments
{
    #region Properties

    /// <summary>
    /// Default: <see cref="string.Empty"/>. <br/>
    /// Supported platfroms: Avalonia.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Default: <see cref="string.Empty"/>. <br/>
    /// Supported platfroms: WPF.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Default: <see cref="string.Empty"/>. <br/>
    /// Supported platfroms: UWP/WinUI/Uno.
    /// </summary>
    public string ButtonText { get; init; } = string.Empty;

    /// <summary>
    /// Default: <see cref="string.Empty"/>. <br/>
    /// Supported platfroms: WPF, Avalonia.
    /// </summary>
    public string SelectedPath { get; init; } = string.Empty;

    /// <summary>
    /// Default: <see cref="Environment.SpecialFolder.MyDocuments"/>. <br/>
    /// Supported platfroms: WPF/UWP/WinUI/Uno.
    /// </summary>
    public Environment.SpecialFolder StartFolder { get; init; } = Environment.SpecialFolder.Desktop;

    /// <summary>
    /// Default: <see langword="true"/>. <br/>
    /// Supported platfroms: WPF.
    /// </summary>
    public bool ShowNewFolderButton { get; init; }

    #endregion
}
