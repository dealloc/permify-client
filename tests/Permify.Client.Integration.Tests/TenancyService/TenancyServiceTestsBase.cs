using Aspire.Hosting;

using Permify.Client.Contracts;
using Permify.Client.Exceptions;
using Permify.Client.Models.Tenancy;

namespace Permify.Client.Integration.Tests.TenancyService;

public abstract class TenancyServiceTestsBase(string endpointName) : ServiceTestsBase(endpointName)
{
    [Theory]
    [InlineData("valid-id", "Valid Name")]
    [InlineData("v", "Valid Name")]
    public async Task Tenancy_Service_Can_Create(string id, string name)
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var (app, serviceProvider) = await SetupTestEnvironmentAsync(cancellationToken);
        await using DistributedApplication _ = app;

        // Act
        var tenancyService = serviceProvider.GetRequiredService<ITenancyService>();
        var response = await tenancyService.CreateTenantAsync(
            new CreateTenantRequest(id, name),
            cancellationToken
        );

        // Assert
        Assert.NotNull(response);
        Assert.Equal(response.Tenant.Id, id);
        Assert.Equal(response.Tenant.Name, name);
        Assert.Equal(response.Tenant.CreatedAt.Date, DateTime.UtcNow.Date);
    }

    [Theory]
    [InlineData("", "Inalid Name")]
    [InlineData("$", "Invalid Name")]
    public async Task Tenancy_Service_Cannot_Create_Invalid(string id, string name)
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
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

    [Fact]
    public async Task TenancyService_Can_List_Empty()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var (app, serviceProvider) = await SetupTestEnvironmentAsync(cancellationToken);
        await using DistributedApplication _ = app;

        // Act
        var tenancyService = serviceProvider.GetRequiredService<ITenancyService>();
        var response = await tenancyService.ListTenantsAsync(
            new(20),
            cancellationToken
        );

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response.ContinuousToken);
        Assert.Empty(response.Tenants);
    }

    [Theory]
    [InlineData(20, 1, 1, true)]
    [InlineData(20, 10, 10, true)]
    [InlineData(10, 10, 10, false)]
    [InlineData(10, 11, 10, false)]
    public async Task TenancyService_Can_List(uint amountOfTenants, uint pageSize, int amountReturned, bool hasMore)
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
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
        Assert.NotNull(response);
        Assert.Equal(hasMore, !string.IsNullOrEmpty(response.ContinuousToken));
        Assert.Equal(amountReturned, response.Tenants.Count);
    }
}