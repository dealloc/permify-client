using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Permify.Client.Options;
using Permify.Client.Options.Validators;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Contains extension methods for <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Permify core services and binds configuration from the "Permify" section.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to register the services in.</param>
    /// <param name="configuration">The configuration to bind from.</param>
    public static IServiceCollection AddPermifyCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IValidateOptions<PermifyOptions>, ValidatePermifyOptions>();

        services.Configure<PermifyOptions>(configuration)
            .AddOptionsWithValidateOnStart<PermifyOptions>();

        return AddPermifyServices(services);
    }

    /// <summary>
    /// Adds Permify core services with manual configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to register the services in.</param>
    /// <param name="configure">An action to configure the Permify options.</param>
    public static IServiceCollection AddPermifyCore(this IServiceCollection services, Action<PermifyOptions> configure)
    {
        services.Configure(configure);

        return AddPermifyServices(services);
    }

    /// <summary>
    /// Handles adding the services required for all implementations of the Permify client.
    /// </summary>
    private static IServiceCollection AddPermifyServices(this IServiceCollection services)
    {
        services.AddServiceDiscoveryCore();

        return services;
    }
}