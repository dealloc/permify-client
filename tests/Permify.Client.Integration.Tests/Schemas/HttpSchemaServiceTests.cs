namespace Permify.Client.Integration.Tests.Schemas;

/// <summary>
/// Tests for ISchemaService using HTTP protocol implementation.
/// </summary>
public sealed class HttpSchemaServiceTests() : SchemaServiceTestsBase("http")
{
    protected override void ConfigurePermifyClients(IServiceCollection services, string endpoint)
        => services.AddPermifyHttpClients(endpoint);
}