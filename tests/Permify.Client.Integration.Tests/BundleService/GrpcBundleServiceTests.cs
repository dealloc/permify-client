using Permify.Client.Contracts;
using Permify.Client.Integration.Tests.Helpers;

namespace Permify.Client.Integration.Tests.BundleService;

/// <summary>
/// Tests for <see cref="IBundleService"/> using gRPC protocol implementation.
/// </summary>
[InheritsTests]
public class GrpcBundleServiceTests : BundleServiceTestsBase
{
    protected override IServiceProvider Services { get; set; } = null!;

    [Before(Test)]
    public void Setup()
    {
        Services = ServicesHelper.CreatePermifyProvider(
            services => services.AddPermifyGrpcClients(
                PermifyContainer.GrpcEndpoint.ToString()
            )
        );
    }
}