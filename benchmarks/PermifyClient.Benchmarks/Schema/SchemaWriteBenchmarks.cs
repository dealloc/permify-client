using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.DependencyInjection;

using Permify.Client.Contracts.V1;
using Permify.Client.Models.V1.Schema;

namespace PermifyClient.Benchmarks.Schema;

public class SchemaWriteBenchmarks : PermifyBenchmarkBase
{
    [Params("http", "grpc")]
    public string Protocol { get; set; } = null!;

    private AsyncServiceScope _scope;
    private ISchemaService _service = null!;

    private const string SimpleSchema = "entity user {}";

    private const string ComplexSchema = @"
entity user {}

entity organization {
    relation admin @user
    relation member @user

    permission create_repository = admin
    permission delete = admin
    permission view = admin or member
}

entity repository {
    relation parent @organization
    relation owner @user
    relation maintainer @user
    relation reader @user

    permission push = owner or maintainer
    permission read = (owner or maintainer or reader) or parent.view
    permission delete = parent.admin
}";

    [IterationSetup]
    public void IterationSetup()
    {
        _scope = GetServiceScope(Protocol);
        _service = _scope.ServiceProvider.GetRequiredService<ISchemaService>();
    }

    [Benchmark]
    public async Task WriteSimpleSchema()
    {
        await _service.WriteSchemaAsync(
            new WriteSchemaRequest(Schema: SimpleSchema),
            CancellationToken.None
        );
    }

    [Benchmark]
    public async Task WriteComplexSchema()
    {
        await _service.WriteSchemaAsync(
            new WriteSchemaRequest(Schema: ComplexSchema),
            CancellationToken.None
        );
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        _scope.Dispose();
    }
}
