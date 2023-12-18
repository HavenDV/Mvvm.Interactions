namespace Mvvm.Interactions;

/// <summary>
/// 
/// </summary>
public interface IWebInteractions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task OpenUrlAsync(
        Uri uri,
        CancellationToken cancellationToken = default);
}
