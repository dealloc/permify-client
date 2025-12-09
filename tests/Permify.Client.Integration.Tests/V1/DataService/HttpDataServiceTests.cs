using Permify.Client.Contracts.V1;
using Permify.Client.Integration.Tests.Helpers;

namespace Permify.Client.Integration.Tests.V1.DataService;

/// <summary>
/// Tests for <see cref="IDataService" /> using gRPC protocol implementation.
/// </summary>
[InheritsTests]
[Category("HTTP")]
public class HttpDataServiceTests : DataServiceTestsBase
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