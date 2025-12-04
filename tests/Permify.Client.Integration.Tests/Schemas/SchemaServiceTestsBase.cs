using Permify.Client.Contracts;
using Permify.Client.Models.Schema;

namespace Permify.Client.Integration.Tests.Schemas;

/// <summary>
/// Abstract base class for testing ISchemaService implementations.
/// Contains all test logic that applies to both HTTP and gRPC implementations.
/// </summary>
public abstract class SchemaServiceTestsBase(string endpointName) : ServiceTestsBase(endpointName)
{
    [Fact]
    public async Task Schema_Service_Can_Write()
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
                    Schema: """
                            entity user {}
                            entity document {
                                relation owner @user
                                action view = owner
                            }
                            """
                ),
                cancellationToken
            );

            // Assert
            Assert.NotNull(response);
        }
    }

    [Fact]
    public async Task Schema_Service_Can_List_After_Write()
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
                    Schema: "entity user {}"
                ),
                cancellationToken
            );

            await schemaService.ListSchemaAsync(new(), cancellationToken);

            // Assert
            Assert.NotNull(response);
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
            await schemaService.ListSchemaAsync(new(), cancellationToken);

            // Assert
        }
    }
}
