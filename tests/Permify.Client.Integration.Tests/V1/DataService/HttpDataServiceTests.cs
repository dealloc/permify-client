using Permify.Client.Contracts.V1;

namespace Permify.Client.Integration.Tests.V1.DataService;

/// <summary>
/// Tests for <see cref="IDataService" /> using gRPC protocol implementation.
/// </summary>
[InheritsTests]
[Category("HTTP")]
public class HttpDataServiceTests : DataServiceTestsBase
{
    protected override void ConfigureServicesAsync(IServiceCollection services)
    {
        services.AddPermifyHttpClients(
            PermifyContainer.HttpEndpoint.ToString()
        );
    }
}