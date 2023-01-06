using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mvvm.CommonInteractions;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddCommonInteractions(
        this IHostBuilder hostBuilder,
        Func<string, string>? localizationFunc = null)
    {
        hostBuilder = hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder));

        hostBuilder.ConfigureServices(services =>
        {
            _ = services.AddCommonInteractions(localizationFunc);
        });

        return hostBuilder;
    }
    
    public static IServiceCollection AddCommonInteractions(
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