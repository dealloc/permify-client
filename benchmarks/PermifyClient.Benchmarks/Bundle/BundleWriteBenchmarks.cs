using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.DependencyInjection;

using Permify.Client.Contracts.V1;
using Permify.Client.Models.V1.Bundles;

namespace PermifyClient.Benchmarks.Bundles;

public class BundleWriteBenchmarks : PermifyBenchmarkBase
{
    [Params("http", "grpc")]
    public string Protocol { get; set; } = null!;

    private AsyncServiceScope _scope;
    private IBundleService _service = null!;

    [IterationSetup]
    public void IterationSetup()
    {
        _scope = GetServiceScope(Protocol);
        _service = _scope.ServiceProvider.GetRequiredService<IBundleService>();
    }

    [Benchmark]
    public async Task WriteSimpleBundle()
    {
        await _service.WriteBundleAsync(
            new WriteBundleRequest(
                Bundles: [
                    new Bundle(
                        Name: "simple_bundle",
                        Arguments: [],
                        Operations: []
                    )
                ]
            ),
            CancellationToken.None
        );
    }

    [Benchmark]
    public async Task WriteComplexBundle()
    {
        await _service.WriteBundleAsync(
            new WriteBundleRequest(
                Bundles: [
                    new Bundle(
                        Name: "complex_bundle",
                        Arguments: [
                            "creatorID",
                            "organizationID"
                        ],
                        Operations: [
                            new Operation(
                                AttributesWrite: [
                                    "organization:{{.organizationID}}$public|boolean:false"
                                ],
                                AttributesDelete: [],
                                RelationshipsWrite: [
                                    "organization:{{.organizationID}}#admin@user:{{.creatorID}}",
                                    "organization:{{.organizationID}}#manager@user:{{.creatorID}}"
                                ],
                                RelationshipsDelete: []
                            )
                        ]
                    )
                ]
            ),
            CancellationToken.None
        );
    }

    [Benchmark]
    public async Task WriteEmptyBundles()
    {
        await _service.WriteBundleAsync(
            new WriteBundleRequest(Bundles: []),
            CancellationToken.None
        );
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        _scope.Dispose();
    }
}