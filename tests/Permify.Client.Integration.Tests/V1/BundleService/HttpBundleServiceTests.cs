using Permify.Client.Contracts.V1;
using Permify.Client.Integration.Tests.Helpers;

namespace Permify.Client.Integration.Tests.V1.BundleService;

/// <summary>
/// Tests for <see cref="IBundleService"/> using gRPC protocol implementation.
/// </summary>
[InheritsTests]
[Category("HTTP")]
public class HttpBundleServiceTests : BundleServiceTestsBase
{
    protected override IServiceProvider Services { get; set; } = null!;

    [Before(Test)]
    public void Setup()
    {
        Services = ServicesHelper.CreatePermifyProvider(
            services => services.AddPermifyHttpClients(
                PermifyContainer.HttpEndpoint.ToString()
            )
        );
    }
}