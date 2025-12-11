using Permify.Client.Contracts.V1;

namespace Permify.Client.Integration.Tests.V1.SchemaService;

/// <summary>
/// Tests for <see cref="ISchemaService" /> using gRPC protocol implementation.
/// </summary>
[InheritsTests]
[Category("gRPC")]
public sealed class GrpcSchemaServiceTests : SchemaServiceTestsBase
{
    protected override void ConfigureServicesAsync(IServiceCollection services)
    {
        services.AddPermifyGrpcClients(
            PermifyContainer.GrpcEndpoint
        );
    }
}