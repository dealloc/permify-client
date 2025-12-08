using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

using Permify.Client.Contracts;
using Permify.Client.Http.Generated;
using Permify.Client.Http.Services;

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
    public static IServiceCollection AddPermifyHttpClients(this IServiceCollection services, string baseUrl)
    {
        services.AddHttpClient(nameof(ApiClient));
        services.AddScoped<ApiClient>(provider =>
        {
            var factory = provider.GetRequiredService<IHttpClientFactory>();
            var auth = new AnonymousAuthenticationProvider();
            var adapter = new HttpClientRequestAdapter(
                auth,
                httpClient: factory.CreateClient(nameof(ApiClient))
            );
            adapter.BaseUrl = baseUrl;

            return new ApiClient(adapter);
        });

        services.AddScoped<ISchemaService, HttpSchemaService>();
        services.AddScoped<ITenancyService, HttpTenancyService>();
        services.AddScoped<IBundleService, HttpBundleService>();
        return services;
    }
}