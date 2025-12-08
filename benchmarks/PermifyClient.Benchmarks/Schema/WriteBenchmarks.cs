using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using Microsoft.Extensions.DependencyInjection;

using Permify.Client.Contracts;
using Permify.Client.Models.Schema;
using Permify.Client.Options;

namespace PermifyClient.Benchmarks.Schema;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0, baseline: true)]
public class WriteBenchmarks
{
    public IContainer PermifyContainer = null!;
    public ServiceProvider provider = null!;

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        PermifyContainer = new ContainerBuilder()
            .WithImage("ghcr.io/permify/permify:v1.5.3")
            .WithPortBinding(3476, assignRandomHostPort: true)
            .WithPortBinding(3478, assignRandomHostPort: true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPath("/healthz").ForPort(3476)))
            .Build();

        await PermifyContainer.StartAsync();
        var permifyEndpoint = new Uri($"http://{PermifyContainer.Hostname}:{PermifyContainer.GetMappedPublicPort(3478)}");

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
        await PermifyContainer.DisposeAsync();
        await provider.DisposeAsync();
    }
}