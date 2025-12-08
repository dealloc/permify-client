using DotNet.Testcontainers.Containers;

using Permify.Client.Options;

namespace Permify.Client.Integration.Tests.Helpers;

/// <summary>
/// Container helper methods to build and configure <see cref="IServiceProvider" /> instances for tests.
/// </summary>
public static class ServicesHelper
{
    /// <summary>
    /// Creates a <see cref="IServiceProvider" /> instance with Permify client configured to use the given <see cref="IContainer" />.
    /// </summary>
    public static IServiceProvider CreatePermifyProvider(
        Action<ServiceCollection>? configureServices = null,
        Action<PermifyOptions>? configureOptions = null
    )
    {
        var services = new ServiceCollection();
        configureOptions ??= (options) =>
        {
            options.TenantId = "t1";
        };

        services.Configure(configureOptions);
        configureServices?.Invoke(services);

        return services.BuildServiceProvider();
    }
}