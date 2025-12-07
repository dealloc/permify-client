namespace Permify.Client.Integration.Tests.SchemaService;

/// <summary>
/// Tests for ISchemaService using HTTP protocol implementation.
/// </summary>
[InheritsTests]
public sealed class HttpSchemaServiceTests() : SchemaServiceTestsBase("http")
{
    protected override void ConfigurePermifyClients(IServiceCollection services, string endpoint)
        => services.AddPermifyHttpClients(endpoint);
}