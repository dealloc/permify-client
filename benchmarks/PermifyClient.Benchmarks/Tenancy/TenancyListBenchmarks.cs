using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.DependencyInjection;

using Permify.Client.Contracts.V1;
using Permify.Client.Models.V1.Tenancy;

namespace PermifyClient.Benchmarks.Tenancy;

public class TenancyListBenchmarks : PermifyBenchmarkBase
{
    [Params("http", "grpc")]
    public string Protocol { get; set; } = null!;

    private AsyncServiceScope _scope;
    private ITenancyService _service = null!;

    public override async Task GlobalSetup()
    {
        await base.GlobalSetup();

        // Create multiple tenants for listing - do this once per benchmark class
        var scope = GetServiceScope("grpc"); // Use grpc for setup
        var service = scope.ServiceProvider.GetRequiredService<ITenancyService>();
        for (int i = 0; i < 20; i++)
        {
            await service.CreateTenantAsync(
                new CreateTenantRequest(
                    Id: $"list_tenant_{i}",
                    Name: $"List Tenant {i}"
                ),
                CancellationToken.None
            );
        }
        scope.Dispose();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _scope = GetServiceScope(Protocol);
        _service = _scope.ServiceProvider.GetRequiredService<ITenancyService>();
    }

    [Benchmark]
    public async Task ListTenantsPageSize5()
    {
        await _service.ListTenantsAsync(
            new ListTenantRequest(PageSize: 5),
            CancellationToken.None
        );
    }

    [Benchmark]
    public async Task ListTenantsPageSize20()
    {
        await _service.ListTenantsAsync(
            new ListTenantRequest(PageSize: 20),
            CancellationToken.None
        );
    }

    [Benchmark]
    public async Task ListTenantsPageSize50()
    {
        await _service.ListTenantsAsync(
            new ListTenantRequest(PageSize: 50),
            CancellationToken.None
        );
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        _scope.Dispose();
    }
}