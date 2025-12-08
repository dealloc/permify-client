using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.DependencyInjection;

using Permify.Client.Contracts.V1;
using Permify.Client.Models.V1.Schema;

namespace PermifyClient.Benchmarks.Schema;

public class SchemaPartialUpdateBenchmarks : PermifyBenchmarkBase
{
    [Params("http", "grpc")]
    public string Protocol { get; set; } = null!;

    private AsyncServiceScope _scope;
    private ISchemaService _service = null!;

    private const string BaseSchema = @"
entity user {}

entity organization {
    relation admin @user
    relation member @user

    permission delete = admin
}";

    public override async Task GlobalSetup()
    {
        await base.GlobalSetup();

        // Write base schema for partial updates - do this once per benchmark class
        var scope = GetServiceScope("grpc"); // Use grpc for setup
        var service = scope.ServiceProvider.GetRequiredService<ISchemaService>();
        await service.WriteSchemaAsync(
            new WriteSchemaRequest(Schema: BaseSchema),
            CancellationToken.None
        );
        scope.Dispose();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _scope = GetServiceScope(Protocol);
        _service = _scope.ServiceProvider.GetRequiredService<ISchemaService>();
    }

    [Benchmark]
    public async Task PartialUpdateSimpleWrite()
    {
        await _service.PartialUpdateSchemaAsync(
            new PartialSchemaUpdateRequest(
                Metadata: null,
                Partials: new Dictionary<string, PartialSchemaUpdateRequest.SchemaPartials>
                {
                    ["organization"] = new PartialSchemaUpdateRequest.SchemaPartials(
                        Write: ["permission create_repository = admin"],
                        Delete: [],
                        Update: []
                    )
                }
            ),
            CancellationToken.None
        );
    }

    [Benchmark]
    public async Task PartialUpdateComplexWrite()
    {
        await _service.PartialUpdateSchemaAsync(
            new PartialSchemaUpdateRequest(
                Metadata: null,
                Partials: new Dictionary<string, PartialSchemaUpdateRequest.SchemaPartials>
                {
                    ["organization"] = new PartialSchemaUpdateRequest.SchemaPartials(
                        Write: [
                            "permission view = admin or member",
                            "permission edit = admin"
                        ],
                        Delete: [],
                        Update: []
                    )
                }
            ),
            CancellationToken.None
        );
    }

    [Benchmark]
    public async Task PartialUpdateDelete()
    {
        // First, add an action to delete
        await _service.PartialUpdateSchemaAsync(
            new PartialSchemaUpdateRequest(
                Metadata: null,
                Partials: new Dictionary<string, PartialSchemaUpdateRequest.SchemaPartials>
                {
                    ["organization"] = new PartialSchemaUpdateRequest.SchemaPartials(
                        Write: ["permission temp_action = admin"],
                        Delete: [],
                        Update: []
                    )
                }
            ),
            CancellationToken.None
        );

        // Now delete it
        await _service.PartialUpdateSchemaAsync(
            new PartialSchemaUpdateRequest(
                Metadata: null,
                Partials: new Dictionary<string, PartialSchemaUpdateRequest.SchemaPartials>
                {
                    ["organization"] = new PartialSchemaUpdateRequest.SchemaPartials(
                        Write: [],
                        Delete: ["temp_action"],
                        Update: []
                    )
                }
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