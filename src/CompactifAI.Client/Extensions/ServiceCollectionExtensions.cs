using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CompactifAI.Client.Extensions;

/// <summary>
/// Extension methods for registering CompactifAI services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the CompactifAI client to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">Action to configure the client options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddCompactifAI(
        this IServiceCollection services,
        Action<CompactifAIOptions> configureOptions)
    {
        services.Configure(configureOptions);

        services.AddHttpClient<ICompactifAIClient, CompactifAIClient>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<CompactifAIOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        });

        return services;
    }

    /// <summary>
    /// Adds the CompactifAI client to the service collection with a simple API key.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="apiKey">The API key.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddCompactifAI(
        this IServiceCollection services,
        string apiKey)
    {
        return services.AddCompactifAI(options => options.ApiKey = apiKey);
    }

    /// <summary>
    /// Adds the CompactifAI client to the service collection using configuration binding.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration section to bind from.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddCompactifAI(
        this IServiceCollection services,
        Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        services.Configure<CompactifAIOptions>(configuration.GetSection(CompactifAIOptions.SectionName));

        services.AddHttpClient<ICompactifAIClient, CompactifAIClient>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<CompactifAIOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        });

        return services;
    }
}
