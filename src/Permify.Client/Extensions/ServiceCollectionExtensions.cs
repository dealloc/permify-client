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
    /// <param name="key">The key under which to register the services</param>
    /// <param name="configuration">The configuration to bind from.</param>
    /// <param name="services">The <see cref="IServiceCollection" /> to register the services in.</param>
    public static IServiceCollection AddKeyedPermifyCore(this IServiceCollection services, object? key, IConfiguration configuration)
        => AddPermifyServices(
            services,
            key,
            configuration: configuration
        );

    /// <summary>
    /// Adds Permify core services and binds configuration from the "Permify" section.
    /// </summary>
    /// <param name="configuration">The configuration to bind from.</param>
    /// <param name="services">The <see cref="IServiceCollection" /> to register the services in.</param>
    public static IServiceCollection AddPermifyCore(this IServiceCollection services, IConfiguration configuration)
        => AddKeyedPermifyCore(services, null, configuration);

    /// <summary>
    /// Adds Permify core services with manual configuration.
    /// </summary>
    /// <param name="key">The key under which to register the services</param>
    /// <param name="configureOptions">An action to configure the Permify options.</param>
    /// <param name="services">The <see cref="IServiceCollection" /> to register the services in.</param>
    public static IServiceCollection AddKeyedPermifyCore(this IServiceCollection services, object? key, Action<PermifyOptions>? configureOptions = null)
        => AddPermifyServices(
            services,
            configureOptions: configureOptions
        );

    /// <summary>
    /// Adds Permify core services with manual configuration.
    /// </summary>
    /// <param name="configure">An action to configure the Permify options.</param>
    /// <param name="services">The <see cref="IServiceCollection" /> to register the services in.</param>
    public static IServiceCollection AddPermifyCore(this IServiceCollection services, Action<PermifyOptions>? configure = null)
        => AddKeyedPermifyCore(services, configure);

    /// <summary>
    /// Handles adding the services required for all implementations of the Permify client.
    /// </summary>
#if NET5_0_OR_GREATER
    [UnconditionalSuppressMessage("AOT", "IL3050:RequiresDynamicCode",
        Justification = "Configuration binding is handled by source generator when EnableConfigurationBindingGenerator is enabled")]
    [UnconditionalSuppressMessage("Trimming", "IL2026:RequiresUnreferencedCode",
        Justification = "Configuration binding is handled by source generator when EnableConfigurationBindingGenerator is enabled")]
#endif
    private static IServiceCollection AddPermifyServices(
        this IServiceCollection services,
        object? key = null,
        IConfiguration? configuration = null,
        Action<PermifyOptions>? configureOptions = null
    )
    {
        services.AddServiceDiscoveryCore();
        services.AddSingleton<IValidateOptions<PermifyOptions>, ValidatePermifyOptions>();

        if (configuration is not null)
            services.Configure<PermifyOptions>(configuration)
                .AddOptionsWithValidateOnStart<PermifyOptions>();

        if (configureOptions is not null)
            services.Configure(configureOptions);

        return services;
    }
}