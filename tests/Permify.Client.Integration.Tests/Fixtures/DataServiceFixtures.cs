using Permify.Client.Contracts.V1;
using Permify.Client.Models.V1;
using Permify.Client.Models.V1.Data;

using Tuple = Permify.Client.Models.V1.Tuple;

namespace Permify.Client.Integration.Tests.Fixtures;

/// <summary>
/// Contains fixtures for having the <see cref="IDataService" /> set up data.
/// </summary>
public static class DataServiceFixtures
{
    /// <summary>
    /// Generates a set of relationships with attributes for testing.
    /// This requires the `valid/organization-hierarchy.perm` schema.
    /// </summary>
    public static async Task GenerateRelationShipsWithAttributes(IDataService dataService, CancellationToken cancellationToken)
    {
        await dataService.WriteDataAsync(new WriteDataRequest(
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
                    Subject: new Subject("user", "1")
                ),
                new Tuple(
                    Entity: new Entity("organization", "1"),
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
    }
}