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
    public static void AddPermifyGrpcClients(this IServiceCollection services)
    {
        services.AddGrpcClient<Schema.SchemaClient>(options => options.Address = new Uri("http://_grpc.permify"));
        services.AddScoped<ISchemaService, GrpcSchemaService>();
    }
}