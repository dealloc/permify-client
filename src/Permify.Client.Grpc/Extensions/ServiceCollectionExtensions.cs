using Base.V1;

using Permify.Client.Contracts;
using Permify.Client.Grpc.Services;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Contains extension methods for <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds clients to interact with the Permify HTTP API.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="baseUrl"></param>
    public static IServiceCollection AddPermifyGrpcClients(this IServiceCollection services, string baseUrl)
    {
        services.AddGrpcClient<Schema.SchemaClient>(options => options.Address = new Uri(baseUrl));
        services.AddScoped<ISchemaService, GrpcSchemaService>();

        services.AddGrpcClient<Tenancy.TenancyClient>(options => options.Address = new Uri(baseUrl));
        services.AddScoped<ITenancyService, GrpcTenantService>();

        services.AddGrpcClient<Bundle.BundleClient>(options => options.Address = new Uri(baseUrl));
        services.AddScoped<IBundleService, GrpcBundleService>();

        return services;
    }
}