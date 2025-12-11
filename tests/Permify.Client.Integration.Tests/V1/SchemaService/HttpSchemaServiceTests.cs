using Permify.Client.Contracts.V1;

namespace Permify.Client.Integration.Tests.V1.SchemaService;

/// <summary>
/// Tests for <see cref="ISchemaService" /> using HTTP protocol implementation.
/// </summary>
[InheritsTests]
[Category("HTTP")]
public sealed class HttpSchemaServiceTests : SchemaServiceTestsBase
{
    protected override void ConfigureServicesAsync(IServiceCollection services)
    {
        services.AddPermifyHttpClients(
            PermifyContainer.HttpEndpoint
        );
    }
}