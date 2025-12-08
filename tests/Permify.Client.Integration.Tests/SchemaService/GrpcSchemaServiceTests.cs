using Permify.Client.Integration.Tests.Helpers;

namespace Permify.Client.Integration.Tests.SchemaService;

/// <summary>
/// Tests for ISchemaService using gRPC protocol implementation.
/// </summary>
[InheritsTests]
public sealed class GrpcSchemaServiceTests : SchemaServiceTestsBase
{
    protected override IServiceProvider Services { get; set; } = null!;

    [Before(Test)]
    public void Setup()
    {
        Services = ServicesHelper.CreatePermifyProvider(
            services => services.AddPermifyGrpcClients(
                PermifyContainer.GrpcEndpoint.ToString()
            )
        );
    }
}