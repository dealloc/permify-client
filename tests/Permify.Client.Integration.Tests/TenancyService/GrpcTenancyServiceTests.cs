using Permify.Client.Contracts;

namespace Permify.Client.Integration.Tests.TenancyService;

/// <summary>
/// Tests for <see cref="ITenancyService"/> using gRPC protocol implementation.
/// </summary>
public sealed class GrpcTenancyServiceTests() : TenancyServiceTestsBase("grpc")
{
    protected override void ConfigurePermifyClients(IServiceCollection services, string endpoint)
        => services.AddPermifyGrpcClients(endpoint);
}