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
    public static void AddPermifyCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IValidateOptions<PermifyOptions>, ValidatePermifyOptions>();

        services.Configure<PermifyOptions>(configuration)
            .AddOptionsWithValidateOnStart<PermifyOptions>();
    }

    /// <summary>
    /// Adds Permify core services with manual configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to register the services in.</param>
    /// <param name="configure">An action to configure the Permify options.</param>
    public static void AddPermifyCore(this IServiceCollection services, Action<PermifyOptions> configure)
    {
        services.Configure(configure);
    }
}