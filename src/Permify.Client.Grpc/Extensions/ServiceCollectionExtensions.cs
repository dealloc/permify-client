using Grpc.Net.ClientFactory;

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
    /// <param name="configureHttpClient">An optional delegate that is used to configure the underlying HTTP client.</param>
    public static IServiceCollection AddPermifyGrpcClients(
        this IServiceCollection services,
        Action<GrpcClientFactoryOptions> configure,
        Action<Type, IHttpClientBuilder>? configureHttpClient = null
    )
    {
        var schemaClient = services.AddGrpcClient<Base.V1.Schema.SchemaClient>(configure);
        var tenancyClient = services.AddGrpcClient<Base.V1.Tenancy.TenancyClient>(configure);
        var bundleClient = services.AddGrpcClient<Base.V1.Bundle.BundleClient>(configure);
        var dataClient = services.AddGrpcClient<Base.V1.Data.DataClient>(configure);
        var permissionClient = services.AddGrpcClient<Base.V1.Permission.PermissionClient>(configure);

        if (configureHttpClient != null)
        {
            configureHttpClient(typeof(Base.V1.Schema.SchemaClient), schemaClient);
            configureHttpClient(typeof(Base.V1.Tenancy.TenancyClient), tenancyClient);
            configureHttpClient(typeof(Base.V1.Bundle.BundleClient), bundleClient);
            configureHttpClient(typeof(Base.V1.Data.DataClient), dataClient);
            configureHttpClient(typeof(Base.V1.Permission.PermissionClient), permissionClient);
        }

        // Configure Watch client with special settings for long-running streams
#pragma warning disable EXTEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates.
        services.AddGrpcClient<Base.V1.Watch.WatchClient>(configure)
            // Remove resilience handler inherited from ConfigureHttpClientDefaults
            // Streaming operations can run indefinitely and shouldn't be subject to timeouts
            .RemoveAllResilienceHandlers()
            .ConfigureHttpClient(http => http.Timeout = Timeout.InfiniteTimeSpan);
#pragma warning restore EXTEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates.

        services.AddScoped<ISchemaService, GrpcSchemaService>();
        services.AddScoped<ITenancyService, GrpcTenantService>();
        services.AddScoped<IBundleService, GrpcBundleService>();
        services.AddScoped<IDataService, GrpcDataService>();
        services.AddScoped<IPermissionService, GrpcPermissionService>();
        services.AddScoped<IWatchService, GrpcWatchService>();

        return services;
    }
}