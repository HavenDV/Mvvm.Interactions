#if HAS_MAUI
namespace Mvvm.Interactions;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseMvvmInteractions(
        this MauiAppBuilder builder,
        Func<string, string>? localizationFunc = null)
    {
        builder = builder ?? throw new ArgumentNullException(nameof(builder));

        _ = builder.Services
            .AddMvvmInteractions(localizationFunc)
            ;

        return builder;
    }
}
#endif