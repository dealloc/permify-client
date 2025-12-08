using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.DependencyInjection;

using Permify.Client.Contracts.V1;
using Permify.Client.Models.V1.Tenancy;

namespace PermifyClient.Benchmarks.Tenancy;

public class TenancyDeleteBenchmarks : PermifyBenchmarkBase
{
    [Params("http", "grpc")]
    public string Protocol { get; set; } = null!;

    private AsyncServiceScope _scope;
    private ITenancyService _service = null!;
    private int _tenantCounter;

    [IterationSetup]
    public void IterationSetup()
    {
        _scope = GetServiceScope(Protocol);
        _service = _scope.ServiceProvider.GetRequiredService<ITenancyService>();

        // Create a tenant to delete
        var tenantId = $"delete_tenant_{_tenantCounter++}";
        _service.CreateTenantAsync(
            new CreateTenantRequest(
                Id: tenantId,
                Name: $"Delete Tenant {tenantId}"
            ),
            CancellationToken.None
        ).GetAwaiter().GetResult(); // Synchronous wait for setup
    }

    [Benchmark]
    public async Task DeleteTenant()
    {
        var tenantId = $"delete_tenant_{_tenantCounter - 1}";

        await _service.DeleteTenantAsync(
            new DeleteTenantRequest(Id: tenantId),
            CancellationToken.None
        );
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        _scope.Dispose();
    }
}
