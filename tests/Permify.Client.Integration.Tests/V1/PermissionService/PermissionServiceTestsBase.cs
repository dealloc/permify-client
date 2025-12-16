using Permify.Client.Contracts.V1;
using Permify.Client.Exceptions;
using Permify.Client.Integration.Tests.Base;
using Permify.Client.Integration.Tests.Fixtures;
using Permify.Client.Integration.Tests.Helpers;
using Permify.Client.Models.V1;
using Permify.Client.Models.V1.Permission;
using Permify.Client.Models.V1.Schema;

namespace Permify.Client.Integration.Tests.V1.PermissionService;

/// <summary>
/// Abstract base class for testing <see cref="IPermissionService" /> implementations.
/// Contains all test logic that applies to both HTTP and gRPC implementations.
/// </summary>
[Retry(3)]
[Category("V1")]
[Category("Integration")]
[Category("IPermissionService")]
[Timeout(1 * 60 * 10000)]
public abstract class PermissionServiceTestsBase : SharedPermifyContainerTest
{
    [Test]
    public async Task Permission_Service_Can_Check_Valid(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        var permissionService = Services.GetRequiredService<IPermissionService>();
        await schemaService.WriteSchemaAsync(
            new WriteSchemaRequest(
                Schema: schema
            ),
            cancellationToken
        );
        await DataServiceFixtures.GenerateRelationShipsWithAttributes(dataService, cancellationToken);

        // Act
        var response = await permissionService.CheckAccessControlAsync(new(
            Metadata: new(),
            Entity: new Entity(
                Type: "document",
                Id: "1"
            ),
            Permission: "edit",
            Subject: new Subject(
                Type: "user",
                Id: "1",
                Relation: null
            )
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.Can).IsEqualTo(CheckAccessControlResponse.CheckResult.CHECK_RESULT_ALLOWED);
    }

    [Test]
    public async Task Permission_Service_Can_Check_Denied(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        var permissionService = Services.GetRequiredService<IPermissionService>();
        await schemaService.WriteSchemaAsync(
            new WriteSchemaRequest(
                Schema: schema
            ),
            cancellationToken
        );
        await DataServiceFixtures.GenerateRelationShipsWithAttributes(dataService, cancellationToken);

        // Act
        var response = await permissionService.CheckAccessControlAsync(new(
            Metadata: new(),
            Entity: new Entity(
                Type: "document",
                Id: "1"
            ),
            Permission: "edit",
            Subject: new Subject(
                Type: "user",
                Id: "2",
                Relation: null
            )
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.Can).IsEqualTo(CheckAccessControlResponse.CheckResult.CHECK_RESULT_DENIED);
    }

    [Test]
    public async Task Permission_Service_Can_Check_Invalid(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        var permissionService = Services.GetRequiredService<IPermissionService>();
        await schemaService.WriteSchemaAsync(
            new WriteSchemaRequest(
                Schema: schema
            ),
            cancellationToken
        );
        await DataServiceFixtures.GenerateRelationShipsWithAttributes(dataService, cancellationToken);

        // Act

        // Assert
        await Assert.ThrowsExactlyAsync<PermifyNotFoundException>(async () =>
            await permissionService.CheckAccessControlAsync(new(
                Metadata: new(),
                Entity: new Entity(
                    Type: "user",
                    Id: "1"
                ),
                Permission: "edit",
                Subject: new Subject(
                    Type: "document",
                    Id: "1",
                    Relation: null
                )
            ), cancellationToken)
        );
    }

    [Test]
    public async Task Permission_Service_Cannot_Check_Bulk_Empty(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        var permissionService = Services.GetRequiredService<IPermissionService>();
        await schemaService.WriteSchemaAsync(
            new WriteSchemaRequest(
                Schema: schema
            ),
            cancellationToken
        );
        await DataServiceFixtures.GenerateRelationShipsWithAttributes(dataService, cancellationToken);

        // Act

        // Assert
        await Assert.ThrowsExactlyAsync<PermifyValidationException>(async () =>
            await permissionService.BulkCheckAccessControlAsync(new BulkCheckAccessControlRequest(
                Metadata: new(),
                Arguments: [],
                Items: [],
                Context: null
            ), cancellationToken)
        );
    }

    [Test]
    public async Task Permission_Service_Can_Check_Bulk_All_Valid(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        var permissionService = Services.GetRequiredService<IPermissionService>();
        await schemaService.WriteSchemaAsync(
            new WriteSchemaRequest(
                Schema: schema
            ),
            cancellationToken
        );
        await DataServiceFixtures.GenerateRelationShipsWithAttributes(dataService, cancellationToken);

        // Act
        var response = await permissionService.BulkCheckAccessControlAsync(new BulkCheckAccessControlRequest(
            Metadata: new(),
            Arguments: [],
            Items: [
                new(
                    Entity: new Entity(
                        Type: "document",
                        Id: "1"
                    ),
                    Permission: "edit",
                    Subject: new Subject(
                        Type: "user",
                        Id: "1",
                        Relation: null
                    )
                ),
                new(
                    Entity: new Entity(
                        Type: "document",
                        Id: "2"
                    ),
                    Permission: "view",
                    Subject: new Subject(
                        Type: "user",
                        Id: "2",
                        Relation: null
                    )
                )
            ],
            Context: null
        ), cancellationToken);

        // Assert
        await Assert.That(response.Results.Count).IsEqualTo(2);
        await Assert.That(response.Results[0].Can).IsEqualTo(CheckAccessControlResponse.CheckResult.CHECK_RESULT_ALLOWED);
        await Assert.That(response.Results[1].Can).IsEqualTo(CheckAccessControlResponse.CheckResult.CHECK_RESULT_ALLOWED);
    }

    [Test]
    public async Task Permission_Service_Can_Check_Bulk_Some_Valid(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/attributes-and-relations.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        var dataService = Services.GetRequiredService<IDataService>();
        var permissionService = Services.GetRequiredService<IPermissionService>();
        await schemaService.WriteSchemaAsync(
            new WriteSchemaRequest(
                Schema: schema
            ),
            cancellationToken
        );
        await DataServiceFixtures.GenerateRelationShipsWithAttributes(dataService, cancellationToken);

        // Act
        var response = await permissionService.BulkCheckAccessControlAsync(new BulkCheckAccessControlRequest(
            Metadata: new(),
            Arguments: [],
            Items: [
                new(
                    Entity: new Entity(
                        Type: "document",
                        Id: "1"
                    ),
                    Permission: "edit",
                    Subject: new Subject(
                        Type: "user",
                        Id: "1",
                        Relation: null
                    )
                ),
                new(
                    Entity: new Entity(
                        Type: "document",
                        Id: "2"
                    ),
                    Permission: "edit",
                    Subject: new Subject(
                        Type: "user",
                        Id: "2",
                        Relation: null
                    )
                )
            ],
            Context: null
        ), cancellationToken);

        // Assert
        await Assert.That(response.Results.Count).IsEqualTo(2);
        await Assert.That(response.Results[0].Can).IsEqualTo(CheckAccessControlResponse.CheckResult.CHECK_RESULT_ALLOWED);
        await Assert.That(response.Results[1].Can).IsEqualTo(CheckAccessControlResponse.CheckResult.CHECK_RESULT_DENIED);
    }
}