using Aspire.Hosting;

using Permify.Client.Options;

namespace Permify.Client.Integration.Tests;

/// <summary>
/// Contains helper methods for setting up the testing environments and fixtures.
/// </summary>
public abstract class ServiceTestsBase(string endpointName)
{
    protected static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Configures the Permify clients for the specific protocol (HTTP or gRPC).
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="endpoint">The Permify endpoint URL.</param>
    protected abstract void ConfigurePermifyClients(IServiceCollection services, string endpoint);

    /// <summary>
    /// Creates and starts an Aspire distributed application with Permify.
    /// </summary>
    protected async Task<DistributedApplication> CreateAndStartAppAsync(CancellationToken cancellationToken)
    {
        var apphost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.Permify_Client_Integration_AppHost>(cancellationToken);

        var app = await apphost
            .BuildAsync(cancellationToken)
            .WaitAsync(DefaultTimeout, cancellationToken);

        await app.StartAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);

        // Wait for Permify to be healthy
        await app
            .ResourceNotifications
            .WaitForResourceHealthyAsync("permify", cancellationToken)
            .WaitAsync(DefaultTimeout, cancellationToken);

        return app;
    }

    /// <summary>
    /// Creates a service provider configured with Permify clients.
    /// </summary>
    protected IServiceProvider CreateServiceProvider(DistributedApplication app)
    {
        var permifyEndpoint = app.GetEndpoint("permify", endpointName);

        var services = new ServiceCollection();
        services.Configure<PermifyOptions>(options =>
        {
            options.TenantId = "t1";
        });
        ConfigurePermifyClients(services, permifyEndpoint.ToString());

        return services.BuildServiceProvider();
    }

    /// <summary>
    /// Sets up a complete test environment with Aspire app and configured service provider.
    /// </summary>
    protected async Task<(DistributedApplication App, IServiceProvider ServiceProvider)> SetupTestEnvironmentAsync(
        CancellationToken cancellationToken)
    {
        var app = await CreateAndStartAppAsync(cancellationToken);
        var serviceProvider = CreateServiceProvider(app);
        return (app, serviceProvider);
    }
}