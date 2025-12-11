using Permify.Client.Contracts.V1;

namespace Permify.Client.Integration.Tests.V1.BundleService;

/// <summary>
/// Tests for <see cref="IBundleService"/> using gRPC protocol implementation.
/// </summary>
[InheritsTests]
[Category("gRPC")]
public class GrpcBundleServiceTests : BundleServiceTestsBase
{
    protected override void ConfigureServicesAsync(IServiceCollection services)
    {
        services.AddPermifyGrpcClients(
            PermifyContainer.GrpcEndpoint
        );
    }
}