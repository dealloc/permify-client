using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.DependencyInjection;

using Permify.Client.Contracts.V1;
using Permify.Client.Models.V1.Schema;

namespace PermifyClient.Benchmarks.Schema;

public class SchemaListBenchmarks : PermifyBenchmarkBase
{
    [Params("http", "grpc")]
    public string Protocol { get; set; } = null!;

    private AsyncServiceScope _scope;
    private ISchemaService _service = null!;

    private const string SimpleSchema = "entity user {}";

    [IterationSetup]
    public void IterationSetup()
    {
        _scope = GetServiceScope(Protocol);
        _service = _scope.ServiceProvider.GetRequiredService<ISchemaService>();
    }

    [Benchmark]
    public async Task ListAfter1Write()
    {
        // Write 1 schema version
        await _service.WriteSchemaAsync(
            new WriteSchemaRequest(Schema: SimpleSchema),
            CancellationToken.None
        );

        // List all versions
        await _service.ListSchemaAsync(
            new ListSchemaRequest(),
            CancellationToken.None
        );
    }

    [Benchmark]
    public async Task ListAfter5Writes()
    {
        // Write 5 schema versions
        for (int i = 0; i < 5; i++)
        {
            await _service.WriteSchemaAsync(
                new WriteSchemaRequest(Schema: SimpleSchema),
                CancellationToken.None
            );
        }

        // List all versions
        await _service.ListSchemaAsync(
            new ListSchemaRequest(),
            CancellationToken.None
        );
    }

    [Benchmark]
    public async Task ListAfter10Writes()
    {
        // Write 10 schema versions
        for (int i = 0; i < 10; i++)
        {
            await _service.WriteSchemaAsync(
                new WriteSchemaRequest(Schema: SimpleSchema),
                CancellationToken.None
            );
        }

        // List all versions
        await _service.ListSchemaAsync(
            new ListSchemaRequest(),
            CancellationToken.None
        );
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        _scope.Dispose();
    }
}