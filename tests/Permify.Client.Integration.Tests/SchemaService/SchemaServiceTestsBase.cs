using System.Text.RegularExpressions;

using Aspire.Hosting;

using Permify.Client.Contracts;
using Permify.Client.Exceptions;
using Permify.Client.Integration.Tests.Helpers;
using Permify.Client.Models.Schema;

namespace Permify.Client.Integration.Tests.SchemaService;

/// <summary>
/// Abstract base class for testing ISchemaService implementations.
/// Contains all test logic that applies to both HTTP and gRPC implementations.
/// </summary>
public abstract class SchemaServiceTestsBase(string endpointName) : ServiceTestsBase(endpointName)
{
    [Theory, MemberData(nameof(SchemaHelper.GetAllValidSchemas), MemberType = typeof(SchemaHelper))]
    public async Task Schema_Service_Can_Write(string schemaFile)
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var (app, serviceProvider) = await SetupTestEnvironmentAsync(cancellationToken);
        await using DistributedApplication _ = app;

        // Act
        var schema = await SchemaHelper.LoadSchemaAsync(schemaFile, cancellationToken);
        var schemaService = serviceProvider.GetRequiredService<ISchemaService>();
        var response = await schemaService.WriteSchemaAsync(
            new WriteSchemaRequest(
                Schema: schema
            ),
            cancellationToken
        );

        // Assert
        Assert.NotNull(response);
    }

    [Theory, MemberData(nameof(SchemaHelper.GetAllInvalidSchemas), MemberType = typeof(SchemaHelper))]
    public async Task Schema_Service_Cannot_Write_Invalid_Schema(string schemaFile)
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var (app, serviceProvider) = await SetupTestEnvironmentAsync(cancellationToken);
        await using DistributedApplication _ = app;

        // Act
        var schema = await SchemaHelper.LoadSchemaAsync(schemaFile, cancellationToken);
        var schemaService = serviceProvider.GetRequiredService<ISchemaService>();
        await Assert.ThrowsAsync<PermifyInternalException>(async () =>
            {
                await schemaService.WriteSchemaAsync(
                    new WriteSchemaRequest(
                        Schema: schema
                    ),
                    cancellationToken
                );
            },
            exception => Regex.IsMatch(exception.Message, "^[0-9]+:[0-9]+")
                ? null
                : "Thrown exception does not resemble error");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task Schema_Service_Can_List_After_Write(int amountOfWrites)
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var (app, serviceProvider) = await SetupTestEnvironmentAsync(cancellationToken);
        await using DistributedApplication _ = app;

        // Act
        var schemaService = serviceProvider.GetRequiredService<ISchemaService>();
        for (int i = 0; i < amountOfWrites; i++)
        {
            await schemaService.WriteSchemaAsync(
                new WriteSchemaRequest(
                    Schema: "entity user {}"
                ),
                cancellationToken
            );
        }

        var response = await schemaService.ListSchemaAsync(new(), cancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(amountOfWrites, response.Schemas.Count);
    }

    [Fact]
    public async Task Schema_Service_Cannot_List_Before_Write()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var (app, serviceProvider) = await SetupTestEnvironmentAsync(cancellationToken);
        await using DistributedApplication _ = app;

        // Act
        var schemaService = serviceProvider.GetRequiredService<ISchemaService>();

        // Assert
        await Assert.ThrowsAsync<PermifyNotFoundException>(async () =>
            await schemaService.ListSchemaAsync(new(), cancellationToken));
    }
}