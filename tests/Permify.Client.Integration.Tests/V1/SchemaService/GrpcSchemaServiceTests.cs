using Permify.Client.Contracts.V1;
using Permify.Client.Integration.Tests.Helpers;

namespace Permify.Client.Integration.Tests.V1.SchemaService;

/// <summary>
/// Tests for <see cref="ISchemaService" /> using gRPC protocol implementation.
/// </summary>
[InheritsTests]
[Category("gRPC")]
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