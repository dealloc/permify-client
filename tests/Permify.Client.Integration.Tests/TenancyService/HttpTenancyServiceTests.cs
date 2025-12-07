using Permify.Client.Contracts;

namespace Permify.Client.Integration.Tests.TenancyService;

/// <summary>
/// Tests for <see cref="ITenancyService"/> using gRPC protocol implementation.
/// </summary>
[InheritsTests]
public sealed class HttpTenancyServiceTests() : TenancyServiceTestsBase("http")
{
    protected override void ConfigurePermifyClients(IServiceCollection services, string endpoint)
        => services.AddPermifyHttpClients(endpoint);
}