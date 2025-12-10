using Permify.Client.Contracts.V1;
using Permify.Client.Exceptions;
using Permify.Client.Integration.Tests.Helpers;
using Permify.Client.Models.V1.Schema;

namespace Permify.Client.Integration.Tests.V1.SchemaService;

/// <summary>
/// Abstract base class for testing <see cref="ISchemaService" /> implementations.
/// Contains all test logic that applies to both HTTP and gRPC implementations.
/// </summary>
[Retry(3)]
[Category("V1")]
[Category("Integration")]
[Category("ISchemaService")]
[Timeout(1 * 60 * 10000)]
public abstract class SchemaServiceTestsBase
{
    [ClassDataSource<PermifyContainer>(Shared = SharedType.None)]
    public required PermifyContainer PermifyContainer { get; init; }

    protected abstract IServiceProvider Services { get; set; }

    [Test, MethodDataSource(typeof(SchemaHelper), nameof(SchemaHelper.GetAllValidSchemas))]
    public async Task Schema_Service_Can_Write(string schemaFile, CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync(schemaFile, cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();

        // Act
        var response = await schemaService.WriteSchemaAsync(
            new WriteSchemaRequest(
                Schema: schema
            ),
            cancellationToken
        );

        // Assert
        await Assert.That(response).IsNotNull();
    }

    [Test, MethodDataSource(typeof(SchemaHelper), nameof(SchemaHelper.GetAllInvalidSchemas))]
    public async Task Schema_Service_Cannot_Write_Invalid_Schema(string schemaFile, CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync(schemaFile, cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();

        // Act

        // Assert
        var exception = await Assert.ThrowsAsync<PermifyInternalException>(async () =>
        {
            await schemaService.WriteSchemaAsync(
                new WriteSchemaRequest(
                    Schema: schema
                ),
                cancellationToken
            );
        });

        await Assert.That(exception?.Message).Matches("^[0-9]+:[0-9]+");
    }

    [Test, DependsOn(nameof(Schema_Service_Can_Write))]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task Schema_Service_Can_List_After_Write(int amountOfWrites, CancellationToken cancellationToken)
    {
        // Arrange
        var schemaService = Services.GetRequiredService<ISchemaService>();
        for (int i = 0; i < amountOfWrites; i++)
        {
            await schemaService.WriteSchemaAsync(
                new WriteSchemaRequest(
                    Schema: "entity user {}"
                ),
                cancellationToken
            );
        }

        // Act
        var response = await schemaService.ListSchemaAsync(new(), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.Schemas.Count).IsEqualTo(amountOfWrites);
    }

    [Test]
    public async Task Schema_Service_Cannot_List_Before_Write(CancellationToken cancellationToken)
    {
        // Arrange
        var schemaService = Services.GetRequiredService<ISchemaService>();

        // Act

        // Assert
        await Assert.ThrowsAsync<PermifyNotFoundException>(async () =>
            await schemaService.ListSchemaAsync(new(), cancellationToken));
    }

    [Test, DependsOn(nameof(Schema_Service_Can_Write))]
    public async Task Schema_Service_Can_Partial_Write(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/organization-hierarchy.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        await schemaService.WriteSchemaAsync(
            new WriteSchemaRequest(
                Schema: schema
            ),
            cancellationToken
        );

        // Act
        var response = await schemaService.PartialUpdateSchemaAsync(new PartialSchemaUpdateRequest(
            null,
            new()
            {
                ["organization"] = new([
                    "relation new_field @user"
                ], [], [])
            }
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.SchemaVersion).IsNotEmpty();
    }

    [Test, DependsOn(nameof(Schema_Service_Can_Write))]
    public async Task Schema_Service_Can_Partial_Delete(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/organization-hierarchy.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        await schemaService.WriteSchemaAsync(
            new WriteSchemaRequest(
                Schema: schema
            ),
            cancellationToken
        );

        // Act
        var response = await schemaService.PartialUpdateSchemaAsync(new PartialSchemaUpdateRequest(
            null,
            new()
            {
                ["organization"] = new([], [
                    "owner"
                ], [])
            }
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.SchemaVersion).IsNotEmpty();
    }

    [Test, DependsOn(nameof(Schema_Service_Can_Write))]
    public async Task Schema_Service_Can_Partial_Update(CancellationToken cancellationToken)
    {
        // Arrange
        var schema = await SchemaHelper.LoadSchemaAsync("valid/organization-hierarchy.perm", cancellationToken);
        var schemaService = Services.GetRequiredService<ISchemaService>();
        await schemaService.WriteSchemaAsync(
            new WriteSchemaRequest(
                Schema: schema
            ),
            cancellationToken
        );

        // Act
        var response = await schemaService.PartialUpdateSchemaAsync(new PartialSchemaUpdateRequest(
            null,
            new()
            {
                ["organization"] = new([], [], [
                    "action manage = admin or member"
                ])
            }
        ), cancellationToken);

        // Assert
        await Assert.That(response).IsNotNull();
        await Assert.That(response.SchemaVersion).IsNotEmpty();
    }
}