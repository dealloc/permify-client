using Permify.Client.Contracts;
using Permify.Client.Exceptions;

namespace Permify.Client.Integration.Tests.BundleService;

/// <summary>
/// Abstract base class for testing <see cref="IBundleService" /> implementations.
/// Contains all test logic that applies to both HTTP and gRPC implementations.
/// </summary>
[Retry(3)]
[Timeout(1 * 60 * 1000)]
public abstract class BundleServiceTestsBase
{
    [ClassDataSource<PermifyContainer>(Shared = SharedType.None)]
    public required PermifyContainer PermifyContainer { get; init; }

    protected abstract IServiceProvider Services { get; set; }

    [Test]
    public async Task Bundle_Service_Can_Write(CancellationToken cancellationToken)
    {
        // Arrange
        var bundleService = Services.GetRequiredService<IBundleService>();

        // Act
        var response = await bundleService.WriteBundleAsync(new([
            new("bundle 1", [], [])
        ]), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.Names.Count).IsEqualTo(1);
        await Assert.That(response.Names[0]).IsEqualTo("bundle 1");
    }

    [Test]
    public async Task Bundle_Service_Can_Write_Complex(CancellationToken cancellationToken)
    {
        // Arrange
        var bundleService = Services.GetRequiredService<IBundleService>();

        // Act
        var response = await bundleService.WriteBundleAsync(new([
            new("bundle 1", [
                "creatorID",
                "organizationID"
            ], [
                new(
                    AttributesWrite:
                    [
                        "organization:{{.organizationID}}$public|boolean:false"
                    ],
                    AttributesDelete: [],
                    RelationshipsWrite:
                    [
                        "organization:{{.organizationID}}#admin@user:{{.creatorID}}",
                        "organization:{{.organizationID}}#manager@user:{{.creatorID}}"
                    ],
                    RelationshipsDelete: []
                )
            ])
        ]), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.Names.Count).IsEqualTo(1);
        await Assert.That(response.Names[0]).IsEqualTo("bundle 1");
    }

    [Test]
    public async Task Bundle_Service_Can_Write_Empty(CancellationToken cancellationToken)
    {
        // Arrange
        var bundleService = Services.GetRequiredService<IBundleService>();

        // Act
        var response = await bundleService.WriteBundleAsync(new([
        ]), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.Names.Count).IsEqualTo(0);
    }

    [Test]
    public async Task Bundle_Service_Cannot_Write_Without_Name(CancellationToken cancellationToken)
    {
        // Arrange
        var bundleService = Services.GetRequiredService<IBundleService>();

        // Act

        // Assert
        await Assert.ThrowsExactlyAsync<PermifyInternalException>(() => bundleService.WriteBundleAsync(new([
            new(string.Empty, [], []),
        ]), cancellationToken));
    }

    [Test]
    [DependsOn(nameof(Bundle_Service_Can_Write))]
    public async Task Bundle_Service_Cannot_Read_Non_Existing(CancellationToken cancellationToken)
    {
        // Arrange
        var name = "bundle 1";
        var bundleService = Services.GetRequiredService<IBundleService>();

        // Act

        // Assert
        await Assert.ThrowsExactlyAsync<PermifyNotFoundException>(() =>
            bundleService.ReadBundleAsync(new(name), cancellationToken));
    }

    [Test]
    [DependsOn(nameof(Bundle_Service_Can_Write))]
    public async Task Bundle_Service_Can_Read(CancellationToken cancellationToken)
    {
        // Arrange
        var name = "bundle 1";
        var bundleService = Services.GetRequiredService<IBundleService>();
        await bundleService.WriteBundleAsync(new([
            new(name, [], [])
        ]), cancellationToken);

        // Act
        var response = await bundleService.ReadBundleAsync(new(name), cancellationToken);

        // Assert
        await Assert.That(response.Bundle).IsNotNull();
        await Assert.That(response.Bundle.Name).IsEqualTo(name);
        await Assert.That(response.Bundle.Arguments.Count).IsEqualTo(0);
        await Assert.That(response.Bundle.Operations.Count).IsEqualTo(0);
    }

    [Test]
    [DependsOn(nameof(Bundle_Service_Can_Write_Complex))]
    public async Task Bundle_Service_Can_Read_Complex(CancellationToken cancellationToken)
    {
        // Arrange
        var name = "bundle 1";
        var bundleService = Services.GetRequiredService<IBundleService>();
        await bundleService.WriteBundleAsync(new([
            new(name, [
                "creatorID",
                "organizationID"
            ], [
                new(
                    AttributesWrite:
                    [
                        "organization:{{.organizationID}}$public|boolean:false"
                    ],
                    AttributesDelete: [],
                    RelationshipsWrite:
                    [
                        "organization:{{.organizationID}}#admin@user:{{.creatorID}}",
                        "organization:{{.organizationID}}#manager@user:{{.creatorID}}"
                    ],
                    RelationshipsDelete: []
                )
            ])
        ]), cancellationToken);

        // Act
        var response = await bundleService.ReadBundleAsync(new(name), cancellationToken);

        // Assert
        await Assert.That(response.Bundle).IsNotNull();
        await Assert.That(response.Bundle.Name).IsEqualTo(name);
        await Assert.That(response.Bundle.Arguments.Count).IsEqualTo(2);
        await Assert.That(response.Bundle.Arguments).Contains("creatorID");
        await Assert.That(response.Bundle.Arguments).Contains("organizationID");
        await Assert.That(response.Bundle.Operations.Count).IsEqualTo(1);
        await Assert.That(response.Bundle.Operations[0].AttributesWrite)
            .Contains("organization:{{.organizationID}}$public|boolean:false");
    }
}