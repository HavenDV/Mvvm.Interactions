using Microsoft.Extensions.DependencyInjection;

namespace Mvvm.Interactions;

public static class HostBuilderExtensions
{
    public static IServiceCollection AddMvvmInteractions(
        this IServiceCollection services,
        Func<string, string>? localizationFunc = null)
    {
        services = services ?? throw new ArgumentNullException(nameof(services));

        _ = services
            .AddSingleton<IFileInteractions, FileInteractions>(_ => new FileInteractions(localizationFunc))
            .AddSingleton<IMessageInteractions, MessageInteractions>(_ => new MessageInteractions(localizationFunc))
            .AddSingleton<IWebInteractions, WebInteractions>(_ => new WebInteractions(localizationFunc))
            ;

        return services;
    }
}