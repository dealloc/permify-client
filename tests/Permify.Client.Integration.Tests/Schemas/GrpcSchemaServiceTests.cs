namespace Permify.Client.Integration.Tests.Schemas;

/// <summary>
/// Tests for ISchemaService using gRPC protocol implementation.
/// </summary>
public sealed class GrpcSchemaServiceTests() : SchemaServiceTestsBase("grpc")
{
    protected override void ConfigurePermifyClients(IServiceCollection services, string endpoint)
        => services.AddPermifyGrpcClients(endpoint);
}