using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.DependencyInjection;

using Permify.Client.Contracts.V1;
using Permify.Client.Models.V1.Bundles;

namespace PermifyClient.Benchmarks.Bundles;

public class BundleReadBenchmarks : PermifyBenchmarkBase
{
    [Params("http", "grpc")]
    public string Protocol { get; set; } = null!;

    private AsyncServiceScope _scope;
    private IBundleService _service = null!;

    private const string SimpleBundleName = "simple_bundle";
    private const string ComplexBundleName = "complex_bundle";

    public override async Task GlobalSetup()
    {
        await base.GlobalSetup();

        // Write bundles for reading - do this once per benchmark class
        var scope = GetServiceScope("grpc"); // Use grpc for setup
        var service = scope.ServiceProvider.GetRequiredService<IBundleService>();
        await service.WriteBundleAsync(
            new WriteBundleRequest(
                Bundles: [
                    new Bundle(
                        Name: SimpleBundleName,
                        Arguments: [],
                        Operations: []
                    ),
                    new Bundle(
                        Name: ComplexBundleName,
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
        scope.Dispose();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _scope = GetServiceScope(Protocol);
        _service = _scope.ServiceProvider.GetRequiredService<IBundleService>();
    }

    [Benchmark]
    public async Task ReadSimpleBundle()
    {
        await _service.ReadBundleAsync(
            new ReadBundleRequest(Name: SimpleBundleName),
            CancellationToken.None
        );
    }

    [Benchmark]
    public async Task ReadComplexBundle()
    {
        await _service.ReadBundleAsync(
            new ReadBundleRequest(Name: ComplexBundleName),
            CancellationToken.None
        );
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        _scope.Dispose();
    }
}