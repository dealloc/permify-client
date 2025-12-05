using Aspire.Hosting;
using Aspire.Hosting.Testing;

using BenchmarkDotNet.Attributes;

using Microsoft.Extensions.DependencyInjection;

using Permify.Client.Contracts;
using Permify.Client.Models.Schema;
using Permify.Client.Options;

namespace PermifyClient.Benchmarks.Schema;

public class WriteBenchmarks
{
    public DistributedApplication application = null!;
    public ServiceProvider provider = null!;

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        var apphost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.Permify_Client_Integration_AppHost>(CancellationToken.None);

        application = await apphost
            .BuildAsync(CancellationToken.None);

        await application.StartAsync(CancellationToken.None);

        var permifyEndpoint = application.GetEndpoint("permify", "grpc");

        var services = new ServiceCollection();
        services.Configure<PermifyOptions>(options =>
        {
            options.TenantId = "t1";
        });

        services.AddPermifyGrpcClients(permifyEndpoint.ToString());
        provider = services.BuildServiceProvider();
    }

    [Benchmark]
    public async Task WriteSchema()
    {
        await using var scope = provider.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<ISchemaService>();

        await service.WriteSchemaAsync(
            new WriteSchemaRequest(
                Schema: "entity user {}"
            ),
            CancellationToken.None
        );
    }

    [GlobalCleanup]
    public async Task GlobalCleanUp()
    {
        await application.DisposeAsync();
        await provider.DisposeAsync();
    }
}