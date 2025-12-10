using Permify.Client.Contracts.V1;

namespace Permify.Client.Integration.Tests.V1.PermissionService;

/// <summary>
/// Tests for <see cref="IPermissionService" /> using gRPC protocol implementation.
/// </summary>
[InheritsTests]
[Category("gRPC")]
public class GrpcPermissionServiceTests : PermissionServiceTestsBase
{
    protected override void ConfigureServicesAsync(IServiceCollection services)
    {
        services.AddPermifyGrpcClients(
            PermifyContainer.GrpcEndpoint.ToString()
        );
    }
}