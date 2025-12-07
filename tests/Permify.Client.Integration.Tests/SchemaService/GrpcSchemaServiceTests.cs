namespace Permify.Client.Integration.Tests.SchemaService;

/// <summary>
/// Tests for ISchemaService using gRPC protocol implementation.
/// </summary>
[InheritsTests]
public sealed class GrpcSchemaServiceTests() : SchemaServiceTestsBase("grpc")
{
    protected override void ConfigurePermifyClients(IServiceCollection services, string endpoint)
        => services.AddPermifyGrpcClients(endpoint);
}