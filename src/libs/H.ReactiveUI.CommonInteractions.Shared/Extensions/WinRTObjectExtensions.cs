#if HAS_WINUI && !HAS_UNO
using System.Diagnostics;
using System.Runtime.InteropServices;
using WinRT;

namespace H.ReactiveUI;

[ComImport]
[Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IInitializeWithWindow
{
    void Initialize(IntPtr hwnd);
}

public static class WinRTObjectExtensions
{
    public static T Initialize<T>(this T obj) where T : IWinRTObject
    {
        var initialize = obj.As<IInitializeWithWindow>();
        initialize.Initialize(Process.GetCurrentProcess().MainWindowHandle);

        return obj;
    }
}
#endif
