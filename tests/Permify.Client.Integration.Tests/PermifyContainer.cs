using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using TUnit.Core.Interfaces;

namespace Permify.Client.Integration.Tests;

public sealed class PermifyContainer : IAsyncInitializer, IAsyncDisposable
{
    public Uri HttpEndpoint => new Uri($"http://{Container.Hostname}:{Container.GetMappedPublicPort(3476)}");

    public Uri GrpcEndpoint => new Uri($"http://{Container.Hostname}:{Container.GetMappedPublicPort(3478)}");

#if NET10_0_OR_GREATER
#else
    // Polyfill the `field` keyword for < .NET 10
    private IContainer? field;
#endif
    /// <summary>
    /// The Permify container instance.
    /// </summary>
    public IContainer Container => field ??= new ContainerBuilder()
        .WithImage("ghcr.io/permify/permify:v1.5.4")
        .WithPortBinding(3476, assignRandomHostPort: true)
        .WithPortBinding(3478, assignRandomHostPort: true)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPath("/healthz").ForPort(3476)))
        .Build();

    public async ValueTask DisposeAsync()
        => await Container.DisposeAsync();

    public async Task InitializeAsync()
        => await Container.StartAsync();
}