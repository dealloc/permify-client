using Permify.Client.Contracts.V1;
using Permify.Client.Integration.Tests.Helpers;

namespace Permify.Client.Integration.Tests.V1.TenancyService;

/// <summary>
/// Tests for <see cref="ITenancyService"/> using gRPC protocol implementation.
/// </summary>
[InheritsTests]
public sealed class HttpTenancyServiceTests : TenancyServiceTestsBase
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