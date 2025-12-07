using Aspire.Hosting;

using Permify.Client.Contracts;
using Permify.Client.Models.Tenancy;

namespace Permify.Client.Integration.Tests.TenancyService;

public abstract class TenancyServiceTestsBase(string endpointName) : ServiceTestsBase(endpointName)
{
    [Theory]
    [InlineData("valid-id", "Valid Name")]
    [InlineData("validid", "Valid Name")]
    [InlineData("validid", "")]
    [InlineData("valid-id-123", "Valid Name")]
    [InlineData("123", "Valid Name")]
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
}