using Permify.Client.Contracts.V1;

namespace Permify.Client.Integration.Tests.V1.BundleService;

/// <summary>
/// Tests for <see cref="IBundleService"/> using gRPC protocol implementation.
/// </summary>
[InheritsTests]
[Category("HTTP")]
public class HttpBundleServiceTests : BundleServiceTestsBase
{
    protected override void ConfigureServicesAsync(IServiceCollection services)
    {
        services.AddPermifyHttpClients(
            PermifyContainer.HttpEndpoint.ToString()
        );
    }
}