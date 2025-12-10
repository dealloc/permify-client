using Permify.Client.Contracts.V1;
using Permify.Client.Integration.Tests.Helpers;
using Permify.Client.Integration.Tests.V1.TenancyService;

namespace Permify.Client.Integration.Tests.Base;

/// <summary>
/// Base class for tests that require a Permify container that can be shared.
/// Each test in the class will use the same container instance, but each test class will have its own tenant.
/// </summary>
/// <remarks>
/// Using this test automatically takes a dependency on <see cref="GrpcTenancyServiceTests" /> and <see cref="HttpTenancyServiceTests" />.
/// </remarks>
[Timeout(1 * 60 * 10000)]
[DependsOn<GrpcTenancyServiceTests>(nameof(TenancyServiceTestsBase.Tenancy_Service_Can_Create))]
[DependsOn<HttpTenancyServiceTests>(nameof(TenancyServiceTestsBase.Tenancy_Service_Can_Create))]
public abstract class SharedPermifyContainerTest
{
    [ClassDataSource<PermifyContainer>(Shared = SharedType.PerTestSession)]
    public required PermifyContainer PermifyContainer { get; init; }

    protected IServiceProvider Services { get; private set; } = null!;

    [Before(Test)]
    public async Task SetupSharedPermifyContainer(CancellationToken cancellationToken)
    {
        var isolated = Guid.NewGuid().ToString("N");
        Services = ServicesHelper.CreatePermifyProvider(
            configureServices: ConfigureServicesAsync,
            configureOptions: options => options.TenantId = isolated
        );

        // Create the tenant so that tests can actually use it.
        TestContext.Current?.OutputWriter?.WriteLineAsync($"Creating isolated tenant {isolated}");
        var tenancyService = Services.GetRequiredService<ITenancyService>();
        await tenancyService.CreateTenantAsync(new(isolated, isolated), cancellationToken);
    }

    /// <summary>
    /// Allows configuring the services collection before the container is created.
    /// Use this to register HTTP or gRPC clients.
    /// </summary>
    protected abstract void ConfigureServicesAsync(IServiceCollection services);
}