#if HAS_WINUI && !HAS_UNO
using WinRT;
using WinRT.Interop;

namespace Mvvm.Interactions;

public static class WinRTObjectExtensions
{
    public static T Initialize<T>(this T obj, Window window) where T : IWinRTObject
    {
        obj = obj ?? throw new ArgumentNullException(nameof(obj));
        window = window ?? throw new ArgumentNullException(nameof(window));

        var hWnd = WindowNative.GetWindowHandle(window);

        InitializeWithWindow.Initialize(obj, hWnd);

        return obj;
    }
}
#endif
