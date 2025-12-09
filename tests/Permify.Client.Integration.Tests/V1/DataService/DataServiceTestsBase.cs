using Permify.Client.Contracts.V1;
using Permify.Client.Integration.Tests.Helpers;
using Permify.Client.Integration.Tests.V1.SchemaService;
using Permify.Client.Models.V1;
using Permify.Client.Models.V1.Data;
using Permify.Client.Models.V1.Schema;

using Tuple = Permify.Client.Models.V1.Tuple;

namespace Permify.Client.Integration.Tests.V1.DataService;

/// <summary>
/// Abstract base class for testing <see cref="IDataService" /> implementations.
/// Contains all test logic that applies to both HTTP and gRPC implementations.
/// </summary>
[Retry(3)]
[Timeout(1 * 60 * 1000)]
public abstract class DataServiceTestsBase
{
    [ClassDataSource<PermifyContainer>(Shared = SharedType.None)]
    public required PermifyContainer PermifyContainer { get; init; }

    protected abstract IServiceProvider Services { get; set; }

    [Test]
    [DependsOn<GrpcSchemaServiceTests>(nameof(SchemaServiceTestsBase.Schema_Service_Can_Write))]
    public async Task Data_Service_Can_Write_Empty(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/organization-hierarchy.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        await schemaService.WriteSchemaAsync(new WriteSchemaRequest(schema), cancellationToken);

        // Act
        var response = await dataService.WriteDataAsync(new WriteDataRequest(
            Metadata: new WriteDataRequest.RequestMetadata(),
            Tuples: [],
            Attributes: []
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.SnapToken).IsNotNull();
    }

    [Test]
    [DependsOn<GrpcSchemaServiceTests>(nameof(SchemaServiceTestsBase.Schema_Service_Can_Write))]
    public async Task Data_Service_Can_Write_Tuple(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/organization-hierarchy.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        await schemaService.WriteSchemaAsync(new WriteSchemaRequest(schema), cancellationToken);

        // Act
        var response = await dataService.WriteDataAsync(new WriteDataRequest(
            Metadata: new WriteDataRequest.RequestMetadata(),
            Tuples: [
                new Tuple(
                    Entity: new Entity("document", "1"),
                    Relation: "owner",
                    Subject: new Entity("user", "1")
                )
            ],
            Attributes: []
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.SnapToken).IsNotNull();
    }

    [Test]
    [DependsOn<GrpcSchemaServiceTests>(nameof(SchemaServiceTestsBase.Schema_Service_Can_Write))]
    public async Task Data_Service_Can_Write_Tuples(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/organization-hierarchy.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        await schemaService.WriteSchemaAsync(new WriteSchemaRequest(schema), cancellationToken);

        // Act
        var response = await dataService.WriteDataAsync(new WriteDataRequest(
            Metadata: new WriteDataRequest.RequestMetadata(),
            Tuples: [
                new Tuple(
                    Entity: new Entity("document", "1"),
                    Relation: "owner",
                    Subject: new Entity("user", "1")
                ),
                new Tuple(
                    Entity: new Entity("document", "2"),
                    Relation: "owner",
                    Subject: new Entity("user", "2")
                )
            ],
            Attributes: []
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.SnapToken).IsNotNull();
    }

    [Test]
    [DependsOn(nameof(Data_Service_Can_Write_Tuple))]
    [DependsOn<GrpcSchemaServiceTests>(nameof(SchemaServiceTestsBase.Schema_Service_Can_Write))]
    public async Task Data_Service_Can_Write_Attribute(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/organization-hierarchy.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        await schemaService.WriteSchemaAsync(new WriteSchemaRequest(schema), cancellationToken);
        await dataService.WriteDataAsync(new WriteDataRequest(
            Metadata: new WriteDataRequest.RequestMetadata(),
            Tuples: [
                new Tuple(
                    Entity: new Entity("document", "1"),
                    Relation: "owner",
                    Subject: new Entity("user", "1")
                )
            ],
            Attributes: []
        ), cancellationToken);

        // Act
        var response = await dataService.WriteDataAsync(new WriteDataRequest(
            Metadata: new WriteDataRequest.RequestMetadata(),
            Tuples: [],
            Attributes: [
                new AttributeEntity(
                    Entity: new Entity("document", "1"),
                    Attribute: "is_private",
                    Value: true
                )
            ]
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.SnapToken).IsNotNull();
    }

    [Test]
    [DependsOn<GrpcSchemaServiceTests>(nameof(SchemaServiceTestsBase.Schema_Service_Can_Write))]
    public async Task Data_Service_Can_Write_Attributes(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/organization-hierarchy.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        await schemaService.WriteSchemaAsync(new WriteSchemaRequest(schema), cancellationToken);
        await dataService.WriteDataAsync(new WriteDataRequest(
            Metadata: new WriteDataRequest.RequestMetadata(),
            Tuples: [
                new Tuple(
                    Entity: new Entity("document", "1"),
                    Relation: "owner",
                    Subject: new Entity("user", "1")
                )
            ],
            Attributes: []
        ), cancellationToken);

        // Act
        var response = await dataService.WriteDataAsync(new WriteDataRequest(
            Metadata: new WriteDataRequest.RequestMetadata(),
            Tuples: [],
            Attributes: [
                new AttributeEntity(
                    Entity: new Entity("document", "1"),
                    Attribute: "is_private",
                    Value: true
                ),
                new AttributeEntity(
                    Entity: new Entity("user", "1"),
                    Attribute: "is_real",
                    Value: true
                )
            ]
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.SnapToken).IsNotNull();
    }

    [Test]
    [DependsOn(nameof(Data_Service_Can_Write_Tuple))]
    [DependsOn(nameof(Data_Service_Can_Write_Attribute))]
    [DependsOn<GrpcSchemaServiceTests>(nameof(SchemaServiceTestsBase.Schema_Service_Can_Write))]
    public async Task Data_Service_Can_Write_Combination(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/organization-hierarchy.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        await schemaService.WriteSchemaAsync(new WriteSchemaRequest(schema), cancellationToken);

        // Act
        var response = await dataService.WriteDataAsync(new WriteDataRequest(
            Metadata: new WriteDataRequest.RequestMetadata(),
            Tuples: [
                new Tuple(
                    Entity: new Entity("document", "1"),
                    Relation: "owner",
                    Subject: new Entity("user", "1")
                ),
            ],
            Attributes: [
                new AttributeEntity(
                    Entity: new Entity("document", "1"),
                    Attribute: "is_private",
                    Value: true
                ),
            ]
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.SnapToken).IsNotNull();
    }
}