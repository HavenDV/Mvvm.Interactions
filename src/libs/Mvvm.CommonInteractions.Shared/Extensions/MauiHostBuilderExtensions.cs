#if HAS_MAUI
namespace Mvvm.CommonInteractions;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseMvvmCommonInteractions(
        this MauiAppBuilder builder,
        Func<string, string>? localizationFunc = null)
    {
        builder = builder ?? throw new ArgumentNullException(nameof(builder));

        _ = builder.Services
            .AddCommonInteractions(localizationFunc)
            ;

        return builder;
    }
}
#endif