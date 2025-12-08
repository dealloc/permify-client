using Permify.Client.Contracts.V1;
using Permify.Client.Integration.Tests.Helpers;

namespace Permify.Client.Integration.Tests.V1.SchemaService;

/// <summary>
/// Tests for <see cref="ISchemaService" /> using HTTP protocol implementation.
/// </summary>
[InheritsTests]
public sealed class HttpSchemaServiceTests : SchemaServiceTestsBase
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