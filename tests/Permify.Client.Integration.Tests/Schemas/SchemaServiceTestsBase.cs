using System.Text.RegularExpressions;

using Permify.Client.Contracts;
using Permify.Client.Exceptions;
using Permify.Client.Integration.Tests.TestSchemas;
using Permify.Client.Models.Schema;

namespace Permify.Client.Integration.Tests.Schemas;

/// <summary>
/// Abstract base class for testing ISchemaService implementations.
/// Contains all test logic that applies to both HTTP and gRPC implementations.
/// </summary>
public abstract class SchemaServiceTestsBase(string endpointName) : ServiceTestsBase(endpointName)
{
    [Theory, MemberData(nameof(SchemaLoader.GetAllValid), MemberType = typeof(SchemaLoader))]
    public async Task Schema_Service_Can_Write(string filename, string schema)
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var (app, serviceProvider) = await SetupTestEnvironmentAsync(cancellationToken);
        await using (app)
        {
            // Act
            var schemaService = serviceProvider.GetRequiredService<ISchemaService>();
            var response = await schemaService.WriteSchemaAsync(
                new WriteSchemaRequest(
                    Schema: schema
                ),
                cancellationToken
            );

            // Assert
            Assert.NotNull(filename); // to avoid unused parameter warning
        }
    }

    [Theory, MemberData(nameof(SchemaLoader.GetAllInvalid), MemberType = typeof(SchemaLoader))]
    public async Task Schema_Service_Cannot_Write_Invalid_Schema(string filename, string schema)
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var (app, serviceProvider) = await SetupTestEnvironmentAsync(cancellationToken);
        await using (app)
        {
            // Act
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

            // Assert
            Assert.NotNull(filename); // to avoid unused parameter warning
        }
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
        await using (app)
        {
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
    }

    [Fact]
    public async Task Schema_Service_Cannot_List_Before_Write()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var (app, serviceProvider) = await SetupTestEnvironmentAsync(cancellationToken);
        await using (app)
        {
            // Act
            var schemaService = serviceProvider.GetRequiredService<ISchemaService>();

            // Assert
            await Assert.ThrowsAsync<PermifyNotFoundException>(async () =>
                await schemaService.ListSchemaAsync(new(), cancellationToken));
        }
    }
}