using Permify.Client.Contracts.V1;
using Permify.Client.Integration.Tests.Base;
using Permify.Client.Integration.Tests.Fixtures;
using Permify.Client.Integration.Tests.Helpers;
using Permify.Client.Integration.Tests.V1.BundleService;
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
[Category("V1")]
[Category("Integration")]
[Category("IDataService")]
[Timeout(1 * 60 * 10000)]
public abstract class DataServiceTestsBase : SharedPermifyContainerTest
{
    [Test]
    [DependsOn<GrpcSchemaServiceTests>(nameof(SchemaServiceTestsBase.Schema_Service_Can_Write))]
    public async Task Data_Service_Can_Write_Empty(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
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
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
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
                    Subject: new Subject("user", "1")
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
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
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
                    Subject: new Subject("user", "1")
                ),
                new Tuple(
                    Entity: new Entity("document", "2"),
                    Relation: "owner",
                    Subject: new Subject("user", "2")
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
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        await schemaService.WriteSchemaAsync(new WriteSchemaRequest(schema), cancellationToken);
        await dataService.WriteDataAsync(new WriteDataRequest(
            Metadata: new WriteDataRequest.RequestMetadata(),
            Tuples: [
                new Tuple(
                    Entity: new Entity("document", "1"),
                    Relation: "owner",
                    Subject: new Subject("user", "1")
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
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        await schemaService.WriteSchemaAsync(new WriteSchemaRequest(schema), cancellationToken);
        await dataService.WriteDataAsync(new WriteDataRequest(
            Metadata: new WriteDataRequest.RequestMetadata(),
            Tuples: [
                new Tuple(
                    Entity: new Entity("document", "1"),
                    Relation: "owner",
                    Subject: new Subject("user", "1")
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
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
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
                    Subject: new Subject("user", "1")
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

    [Test]
    [DependsOn(nameof(Data_Service_Can_Write_Tuple))]
    [DependsOn(nameof(Data_Service_Can_Write_Attribute))]
    [DependsOn<GrpcSchemaServiceTests>(nameof(SchemaServiceTestsBase.Schema_Service_Can_Write))]
    public async Task Data_Service_Can_Read_Relationships_Without_Filter(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        await schemaService.WriteSchemaAsync(new WriteSchemaRequest(schema), cancellationToken);
        await DataServiceFixtures.GenerateRelationShipsWithAttributes(dataService, cancellationToken);

        // Act
        var response = await dataService.ReadRelationshipsAsync(new ReadRelationshipsRequest(
            Metadata: new(),
            Filter: new(
                Entity: null,
                Relation: null,
                Subject: null
            ),
            PageSize: 10,
            ContinuousToken: null
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.Tuples.Count).IsEqualTo(3);
        await Assert.That(response.Tuples[0].Entity.Type).IsEqualTo("document");
    }

    [Test]
    [DependsOn(nameof(Data_Service_Can_Write_Tuple))]
    [DependsOn(nameof(Data_Service_Can_Write_Attribute))]
    [DependsOn<GrpcSchemaServiceTests>(nameof(SchemaServiceTestsBase.Schema_Service_Can_Write))]
    public async Task Data_Service_Can_Read_Relationships(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        await schemaService.WriteSchemaAsync(new WriteSchemaRequest(schema), cancellationToken);
        await DataServiceFixtures.GenerateRelationShipsWithAttributes(dataService, cancellationToken);

        // Act
        var response = await dataService.ReadRelationshipsAsync(new ReadRelationshipsRequest(
            Metadata: new(),
            Filter: new(
                Entity: new EntityFilter(
                    Type: "document",
                    Ids: ["1"]
                ),
                Relation: null,
                Subject: null
            ),
            PageSize: 10,
            ContinuousToken: null
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.Tuples.Count).IsEqualTo(1);
        await Assert.That(response.Tuples[0].Entity.Type).IsEqualTo("document");
        await Assert.That(response.Tuples[0].Entity.Id).IsEqualTo("1");
    }

    [Test]
    [DependsOn(nameof(Data_Service_Can_Write_Tuple))]
    [DependsOn(nameof(Data_Service_Can_Write_Attribute))]
    [DependsOn<GrpcSchemaServiceTests>(nameof(SchemaServiceTestsBase.Schema_Service_Can_Write))]
    public async Task Data_Service_Can_Read_Attributes_Without_Filter(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        await schemaService.WriteSchemaAsync(new WriteSchemaRequest(schema), cancellationToken);
        await DataServiceFixtures.GenerateRelationShipsWithAttributes(dataService, cancellationToken);

        // Act
        var response = await dataService.ReadAttributesAsync(new ReadAttributesRequest(
            Metadata: new(),
            Filter: null,
            PageSize: 10,
            ContinuousToken: null
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.Attributes.Count).IsEqualTo(8);
        await Assert.That(response.Attributes[0].Entity.Type).IsEqualTo("document");
        await Assert.That(response.Attributes[0].Entity.Id).IsEqualTo("1");
        await Assert.That(response.Attributes[0].Attribute).IsEqualTo("name");
        await Assert.That(response.Attributes[0].Value).IsEqualTo("name of document 1");
    }

    [Test]
    [DependsOn(nameof(Data_Service_Can_Write_Tuple))]
    [DependsOn(nameof(Data_Service_Can_Write_Attribute))]
    [DependsOn<GrpcSchemaServiceTests>(nameof(SchemaServiceTestsBase.Schema_Service_Can_Write))]
    public async Task Data_Service_Can_Read_Attributes_Filter_By_Entity(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        await schemaService.WriteSchemaAsync(new WriteSchemaRequest(schema), cancellationToken);
        await DataServiceFixtures.GenerateRelationShipsWithAttributes(dataService, cancellationToken);

        // Act
        var response = await dataService.ReadAttributesAsync(new ReadAttributesRequest(
            Metadata: new(),
            Filter: new(
                Entity: new EntityFilter("document", ["1"]),
                Attributes: []
            ),
            PageSize: 10,
            ContinuousToken: null
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.Attributes.Count).IsEqualTo(8);
        await Assert.That(response.Attributes[0].Entity.Type).IsEqualTo("document");
        await Assert.That(response.Attributes[0].Entity.Id).IsEqualTo("1");
        await Assert.That(response.Attributes[0].Attribute).IsEqualTo("name");
        await Assert.That(response.Attributes[0].Value).IsEqualTo("name of document 1");

        await Assert.That(response.Attributes[1].Entity.Type).IsEqualTo("document");
        await Assert.That(response.Attributes[1].Entity.Id).IsEqualTo("1");
        await Assert.That(response.Attributes[1].Attribute).IsEqualTo("word_count");
        await Assert.That(response.Attributes[1].Value).IsEqualTo(100);

        await Assert.That(response.Attributes[2].Entity.Type).IsEqualTo("document");
        await Assert.That(response.Attributes[2].Entity.Id).IsEqualTo("1");
        await Assert.That(response.Attributes[2].Attribute).IsEqualTo("fault_tolerance");
        await Assert.That(response.Attributes[2].Value).IsEqualTo(0.5d);

        await Assert.That(response.Attributes[3].Entity.Type).IsEqualTo("document");
        await Assert.That(response.Attributes[3].Entity.Id).IsEqualTo("1");
        await Assert.That(response.Attributes[3].Attribute).IsEqualTo("is_private");
        await Assert.That(response.Attributes[3].Value).IsEqualTo(true);

        await Assert.That(response.Attributes[4].Entity.Type).IsEqualTo("document");
        await Assert.That(response.Attributes[4].Entity.Id).IsEqualTo("1");
        await Assert.That(response.Attributes[4].Attribute).IsEqualTo("tags");
        await Assert
            .That(response.Attributes[4].Value)
            .IsTypeOf<string[]>()
            .And
            .Contains("tag-1");

        await Assert.That(response.Attributes[5].Entity.Type).IsEqualTo("document");
        await Assert.That(response.Attributes[5].Entity.Id).IsEqualTo("1");
        await Assert.That(response.Attributes[5].Attribute).IsEqualTo("scores");
        await Assert
            .That(response.Attributes[5].Value)
            .IsTypeOf<int[]>()
            .And
            .Contains(1)
            .And
            .Contains(5);

        await Assert.That(response.Attributes[6].Entity.Type).IsEqualTo("document");
        await Assert.That(response.Attributes[6].Entity.Id).IsEqualTo("1");
        await Assert.That(response.Attributes[6].Attribute).IsEqualTo("fault_tolerances");
        await Assert
            .That(response.Attributes[6].Value)
            .IsTypeOf<double[]>()
            .And
            .Contains(0.1d)
            .And
            .Contains(0.5);

        await Assert.That(response.Attributes[7].Entity.Type).IsEqualTo("document");
        await Assert.That(response.Attributes[7].Entity.Id).IsEqualTo("1");
        await Assert.That(response.Attributes[7].Attribute).IsEqualTo("yes_or_no");
        await Assert
            .That(response.Attributes[7].Value)
            .IsTypeOf<bool[]>()
            .And
            .Contains(true)
            .And
            .Contains(false);
    }

    [Test]
    [DependsOn(nameof(Data_Service_Can_Write_Tuple))]
    [DependsOn(nameof(Data_Service_Can_Write_Attribute))]
    [DependsOn<GrpcSchemaServiceTests>(nameof(SchemaServiceTestsBase.Schema_Service_Can_Write))]
    public async Task Data_Service_Can_Read_Attributes_Filter_By_Attribute(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        await schemaService.WriteSchemaAsync(new WriteSchemaRequest(schema), cancellationToken);
        await DataServiceFixtures.GenerateRelationShipsWithAttributes(dataService, cancellationToken);

        // Act
        var response = await dataService.ReadAttributesAsync(new ReadAttributesRequest(
            Metadata: new(),
            Filter: new(
                Entity: null,
                Attributes: ["word_count"]
            ),
            PageSize: 10,
            ContinuousToken: null
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.Attributes.Count).IsEqualTo(1);
        await Assert.That(response.Attributes[0].Entity.Type).IsEqualTo("document");
        await Assert.That(response.Attributes[0].Entity.Id).IsEqualTo("1");
        await Assert.That(response.Attributes[0].Attribute).IsEqualTo("word_count");
        await Assert.That(response.Attributes[0].Value).IsEqualTo(100);
    }

    [Test]
    [DependsOn(nameof(Data_Service_Can_Read_Relationships))]
    [DependsOn<GrpcSchemaServiceTests>(nameof(SchemaServiceTestsBase.Schema_Service_Can_Write))]
    [DependsOn<GrpcBundleServiceTests>(nameof(BundleServiceTestsBase.Bundle_Service_Can_Write_Complex))]
    public async Task Data_Service_Can_Delete_Empty(CancellationToken cancellationToken)
    {
        // Arrange
        var dataService = Services.GetRequiredService<IDataService>();

        // Act
        var response = await dataService.DeleteDataAsync(new(
            TupleFilter: new(null, null, null),
            AttributeFilter: new(null, null)
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
    }

    [Test]
    [DependsOn(nameof(Data_Service_Can_Write_Tuple))]
    [DependsOn(nameof(Data_Service_Can_Read_Relationships))]
    [DependsOn<GrpcSchemaServiceTests>(nameof(SchemaServiceTestsBase.Schema_Service_Can_Write))]
    public async Task Data_Service_Can_Delete_Filtered_Tuples(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        await schemaService.WriteSchemaAsync(new WriteSchemaRequest(schema), cancellationToken);
        await DataServiceFixtures.GenerateRelationShipsWithAttributes(dataService, cancellationToken);

        // Act
        await dataService.DeleteDataAsync(new(
            TupleFilter: new(new EntityFilter(
                Type: "document",
                Ids: ["2"]
            ), null, null),
            AttributeFilter: new(null, null)
        ), cancellationToken);

        // Assert
        var response = await dataService.ReadRelationshipsAsync(new ReadRelationshipsRequest(
            Metadata: new(),
            Filter: new(
                Entity: null,
                Relation: null,
                Subject: null
            ),
            PageSize: 10,
            ContinuousToken: null
        ), cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Tuples.Count).IsEqualTo(1);
        await Assert.That(response.Tuples[0].Entity.Id).IsEqualTo("1");
    }

    [Test]
    [DependsOn(nameof(Data_Service_Can_Write_Tuple))]
    [DependsOn(nameof(Data_Service_Can_Read_Attributes_Without_Filter))]
    [DependsOn<GrpcSchemaServiceTests>(nameof(SchemaServiceTestsBase.Schema_Service_Can_Write))]
    public async Task Data_Service_Can_Delete_Filtered_Attributes(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        await schemaService.WriteSchemaAsync(new WriteSchemaRequest(schema), cancellationToken);
        await DataServiceFixtures.GenerateRelationShipsWithAttributes(dataService, cancellationToken);

        // Act
        await dataService.DeleteDataAsync(new(
            TupleFilter: new(null, null, null),
            AttributeFilter: new(new EntityFilter(
                Type: "document",
                Ids: ["1"]
            ), ["name", "fault_tolerance"])
        ), cancellationToken);

        // Assert
        var response = await dataService.ReadAttributesAsync(new ReadAttributesRequest(
            Metadata: new(),
            Filter: new(
                Entity: null,
                Attributes: null
            ),
            PageSize: 10,
            ContinuousToken: null
        ), cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Attributes.Count).IsEqualTo(6);
    }
}