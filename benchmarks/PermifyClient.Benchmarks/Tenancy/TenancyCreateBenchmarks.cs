using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.DependencyInjection;

using Permify.Client.Contracts.V1;
using Permify.Client.Models.V1.Tenancy;

namespace PermifyClient.Benchmarks.Tenancy;

public class TenancyCreateBenchmarks : PermifyBenchmarkBase
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
    }

    [Benchmark]
    public async Task CreateTenant()
    {
        // Use a counter to ensure unique tenant IDs per iteration
        var tenantId = $"tenant_{_tenantCounter++}";

        await _service.CreateTenantAsync(
            new CreateTenantRequest(
                Id: tenantId,
                Name: $"Tenant {tenantId}"
            ),
            CancellationToken.None
        );
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        _scope.Dispose();
    }
}
