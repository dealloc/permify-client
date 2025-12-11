using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using Microsoft.Extensions.DependencyInjection;

using Permify.Client.Options;

namespace PermifyClient.Benchmarks;

/// <summary>
/// Abstract base class for all Permify benchmarks providing shared infrastructure.
/// </summary>
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0, baseline: true)]
public abstract class PermifyBenchmarkBase
{
    protected IContainer PermifyContainer = null!;
    protected ServiceProvider HttpProvider = null!;
    protected ServiceProvider GrpcProvider = null!;

    [GlobalSetup]
    public virtual async Task GlobalSetup()
    {
        // Initialize Permify container with both HTTP and gRPC ports
        PermifyContainer = new ContainerBuilder()
            .WithImage("ghcr.io/permify/permify:v1.5.3")
            .WithPortBinding(3476, assignRandomHostPort: true)
            .WithPortBinding(3478, assignRandomHostPort: true)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilHttpRequestIsSucceeded(r => r.ForPath("/healthz").ForPort(3476)))
            .Build();

        await PermifyContainer.StartAsync();

        // Get endpoint URLs
        var httpEndpoint = new Uri($"http://{PermifyContainer.Hostname}:{PermifyContainer.GetMappedPublicPort(3476)}");
        var grpcEndpoint = new Uri($"http://{PermifyContainer.Hostname}:{PermifyContainer.GetMappedPublicPort(3478)}");

        // Create HTTP provider
        HttpProvider = CreateProvider(services =>
            services.AddPermifyHttpClients(httpEndpoint));

        // Create gRPC provider
        GrpcProvider = CreateProvider(services =>
            services.AddPermifyGrpcClients(grpcEndpoint));
    }

    /// <summary>
    /// Creates a service provider with Permify client configured.
    /// </summary>
    protected ServiceProvider CreateProvider(Action<ServiceCollection> configure)
    {
        var services = new ServiceCollection();
        services.Configure<PermifyOptions>(options =>
        {
            options.TenantId = "t1";
        });

        configure(services);
        return services.BuildServiceProvider();
    }

    /// <summary>
    /// Gets a service scope for the specified protocol.
    /// </summary>
    protected AsyncServiceScope GetServiceScope(string protocol)
    {
        return protocol.ToLowerInvariant() switch
        {
            "http" => HttpProvider.CreateAsyncScope(),
            "grpc" => GrpcProvider.CreateAsyncScope(),
            _ => throw new ArgumentException($"Unknown protocol: {protocol}", nameof(protocol))
        };
    }

    [GlobalCleanup]
    public async Task GlobalCleanup()
    {
        await HttpProvider.DisposeAsync();
        await GrpcProvider.DisposeAsync();
        await PermifyContainer.DisposeAsync();
    }
}