using Permify.Client.Contracts.V1;

namespace Permify.Client.Integration.Tests.V1.PermissionService;

/// <summary>
/// Tests for <see cref="IPermissionService" /> using HTTP protocol implementation.
/// </summary>
[InheritsTests]
[Category("HTTP")]
public class HttpPermissionServiceTests : PermissionServiceTestsBase
{
    protected override void ConfigureServicesAsync(IServiceCollection services)
    {
        services.AddPermifyHttpClients(
            PermifyContainer.HttpEndpoint.ToString()
        );
    }
}