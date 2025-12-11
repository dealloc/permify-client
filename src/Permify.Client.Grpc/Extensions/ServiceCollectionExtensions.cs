using System.Diagnostics.CodeAnalysis;

using Grpc.Net.ClientFactory;

using Microsoft.Extensions.Http.Resilience;

using Permify.Client.Contracts.V1;
using Permify.Client.Grpc.Services.V1;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Contains extension methods for <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds clients to interact with the Permify gRPC API.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to register services into.</param>
    /// <param name="baseAddress">The base address of the gRPC API.</param>
    public static IServiceCollection AddPermifyGrpcClients(
        this IServiceCollection services,
        Uri baseAddress
    ) => AddPermifyGrpcClients(services, options => options.Address = baseAddress);

    /// <summary>
    /// Adds clients to interact with the Permify gRPC API.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to register services into.</param>
    /// <param name="configure">A delegate that is used to configure the gRPC client.</param>
    public static IServiceCollection AddPermifyGrpcClients(
        this IServiceCollection services,
        Action<GrpcClientFactoryOptions> configure
    )
    {
        services.AddGrpcClient<Base.V1.Schema.SchemaClient>(configure);
        services.AddGrpcClient<Base.V1.Tenancy.TenancyClient>(configure);
        services.AddGrpcClient<Base.V1.Bundle.BundleClient>(configure);
        services.AddGrpcClient<Base.V1.Data.DataClient>(configure);
        services.AddGrpcClient<Base.V1.Permission.PermissionClient>(configure);

        // Configure Watch client with special settings for long-running streams
        services.AddGrpcClient<Base.V1.Watch.WatchClient>(configure)
            // Remove resilience handler inherited from ConfigureHttpClientDefaults
            // Streaming operations can run indefinitely and shouldn't be subject to timeouts
            .RemoveAllResilienceHandlers()
            .ConfigureHttpClient(http => http.Timeout = Timeout.InfiniteTimeSpan);

        services.AddScoped<ISchemaService, GrpcSchemaService>();
        services.AddScoped<ITenancyService, GrpcTenantService>();
        services.AddScoped<IBundleService, GrpcBundleService>();
        services.AddScoped<IDataService, GrpcDataService>();
        services.AddScoped<IPermissionService, GrpcPermissionService>();
        services.AddScoped<IWatchService, GrpcWatchService>();

        return services;
    }
}