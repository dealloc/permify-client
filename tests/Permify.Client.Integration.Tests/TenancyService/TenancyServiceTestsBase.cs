using Aspire.Hosting;

using Permify.Client.Contracts;
using Permify.Client.Exceptions;
using Permify.Client.Models.Tenancy;

namespace Permify.Client.Integration.Tests.TenancyService;

/// <summary>
/// Abstract base class for testing <see cref="ITenancyService" /> implementations.
/// Contains all test logic that applies to both HTTP and gRPC implementations.
/// </summary>
[Retry(3)]
[Timeout(1 * 60 * 1000)]
public abstract class TenancyServiceTestsBase(string endpointName) : ServiceTestsBase(endpointName)
{
    [Test]
    [Arguments("v", "Valid Name")]
    [Arguments("valid-id", "Valid Name")]
    public async Task Tenancy_Service_Can_Create(string id, string name, CancellationToken cancellationToken)
    {
        // Arrange
        var (app, serviceProvider) = await SetupTestEnvironmentAsync(cancellationToken);
        await using DistributedApplication _ = app;

        // Act
        var tenancyService = serviceProvider.GetRequiredService<ITenancyService>();
        var response = await tenancyService.CreateTenantAsync(
            new CreateTenantRequest(id, name),
            cancellationToken
        );

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.Tenant.Id).IsEqualTo(id);
        await Assert.That(response.Tenant.Name).IsEqualTo(name);
        await Assert.That(response.Tenant.CreatedAt.Date).IsEqualTo(DateTime.UtcNow.Date);
    }

    [Test]
    [Arguments("", "Inalid Name")]
    [Arguments("$", "Invalid Name")]
    public async Task Tenancy_Service_Cannot_Create_Invalid(string id, string name, CancellationToken cancellationToken)
    {
        // Arrange
        var (app, serviceProvider) = await SetupTestEnvironmentAsync(cancellationToken);
        await using DistributedApplication _ = app;

        // Act
        var tenancyService = serviceProvider.GetRequiredService<ITenancyService>();

        // Assert
        await Assert.ThrowsAsync<PermifyValidationException>(async () => await tenancyService.CreateTenantAsync(
            new CreateTenantRequest(id, name),
            cancellationToken
        ));
    }

    [Test]
    public async Task Tenancy_Service_Can_List_Empty(CancellationToken cancellationToken)
    {
        // Arrange
        var (app, serviceProvider) = await SetupTestEnvironmentAsync(cancellationToken);
        await using DistributedApplication _ = app;

        // Act
        var tenancyService = serviceProvider.GetRequiredService<ITenancyService>();
        var response = await tenancyService.ListTenantsAsync(
            new(20),
            cancellationToken
        );

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.ContinuousToken).IsEmpty();
        await Assert.That(response.Tenants).IsEmpty();
    }

    [Test]
    [Arguments(20, 1, 1, true)]
    [Arguments(20, 10, 10, true)]
    [Arguments(10, 10, 10, false)]
    [Arguments(10, 11, 10, false)]
    [DependsOn(nameof(Tenancy_Service_Can_Create))]
    public async Task Tenancy_Service_Can_List(
        uint amountOfTenants,
        uint pageSize,
        int amountReturned,
        bool hasMore,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        var (app, serviceProvider) = await SetupTestEnvironmentAsync(cancellationToken);
        await using DistributedApplication _ = app;

        // Act
        var tenancyService = serviceProvider.GetRequiredService<ITenancyService>();
        for (int i = 0; i < amountOfTenants; i++)
        {
            await tenancyService.CreateTenantAsync(
                new($"tenant-{i}", $"Tenant Name {i}"),
                cancellationToken
            );
        }

        var response = await tenancyService.ListTenantsAsync(new(pageSize), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(string.IsNullOrWhiteSpace(response.ContinuousToken)).IsEqualTo(!hasMore);
        await Assert.That(response.Tenants.Count).IsEqualTo(amountReturned);
    }

    [Test]
    [DependsOn(nameof(Tenancy_Service_Can_List))]
    [DependsOn(nameof(Tenancy_Service_Can_Create))]
    public async Task Tenancy_Service_Can_Delete(CancellationToken cancellationToken)
    {
        // Arrange
        var (app, serviceProvider) = await SetupTestEnvironmentAsync(cancellationToken);
        await using DistributedApplication _ = app;
        var id = Guid.NewGuid().ToString();
        var name = $"name of {id}";
        var tenancyService = serviceProvider.GetRequiredService<ITenancyService>();
        await tenancyService.CreateTenantAsync(
            new CreateTenantRequest(id, name),
            cancellationToken
        );

        // Act
        var response = await tenancyService.DeleteTenantAsync(new(id), cancellationToken);

        // Assert
        await Assert.That(response.TenantId).IsEqualTo(id);

        var list = await tenancyService.ListTenantsAsync(new(5), cancellationToken);
        await Assert.That(list.Tenants).IsEmpty();
    }

    [Test]
    public async Task Tenancy_Service_Cannot_Delete_Non_Existing(CancellationToken cancellationToken)
    {
        // Arrange
        var (app, serviceProvider) = await SetupTestEnvironmentAsync(cancellationToken);
        await using DistributedApplication _ = app;
        var id = Guid.NewGuid().ToString();
        var tenancyService = serviceProvider.GetRequiredService<ITenancyService>();

        // Act
        await tenancyService.CreateTenantAsync(
            new CreateTenantRequest(Guid.NewGuid().ToString(), "Dummy data"),
            cancellationToken
        );

        // Assert
        await Assert.ThrowsExactlyAsync<PermifyNotFoundException>(async () =>
            await tenancyService.DeleteTenantAsync(new(id), cancellationToken));
    }
}