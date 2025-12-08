using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using TUnit.Core.Interfaces;

namespace Permify.Client.Integration.Tests;

public sealed class PermifyContainer : IAsyncInitializer, IAsyncDisposable
{
    public Uri HttpEndpoint => field ??= new Uri($"http://{Container.Hostname}:{Container.GetMappedPublicPort(3476)}");

    public Uri GrpcEndpoint => field ??= new Uri($"http://{Container.Hostname}:{Container.GetMappedPublicPort(3478)}");

    /// <summary>
    /// The Permify container instance.
    /// </summary>
    public IContainer Container => field ??= new ContainerBuilder()
        .WithImage("ghcr.io/permify/permify:v1.5.3")
        .WithPortBinding(3476, assignRandomHostPort: true)
        .WithPortBinding(3478, assignRandomHostPort: true)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPath("/healthz").ForPort(3476)))
        .Build();

    public async ValueTask DisposeAsync()
        => await Container.DisposeAsync();

    public async Task InitializeAsync()
        => await Container.StartAsync();
}