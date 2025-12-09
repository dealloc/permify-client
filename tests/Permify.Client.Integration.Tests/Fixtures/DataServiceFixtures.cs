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
    ///
    /// This generates 2 documents (1 & 2) and a user that is reviewer of 1.
    /// Document 1 has all possible attributes set.
    /// </summary>
    /// <remarks>
    /// This requires the `valid/organization-hierarchy.perm` schema.
    /// </remarks>
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
                    Entity: new Entity("document", "2"),
                    Relation: "reviewer",
                    Subject: new Subject("user", "2")
                ),
            ],
            Attributes: [
                new AttributeEntity(
                    Entity: new Entity("document", "1"),
                    Attribute: "name",
                    Value: "name of document 1"
                ),
                new AttributeEntity(
                    Entity: new Entity("document", "1"),
                    Attribute: "word_count",
                    Value: 100
                ),
                new AttributeEntity(
                    Entity: new Entity("document", "1"),
                    Attribute: "fault_tolerance",
                    Value: 0.5d
                ),
                new AttributeEntity(
                    Entity: new Entity("document", "1"),
                    Attribute: "is_private",
                    Value: true
                ),
                new AttributeEntity(
                    Entity: new Entity("document", "1"),
                    Attribute: "tags",
                    Value: new[] { "tag-1", "tag-2" }
                ),
                new AttributeEntity(
                    Entity: new Entity("document", "1"),
                    Attribute: "scores",
                    Value: new[] { 0, 1, 2, 3, 4, 5 }
                ),
                new AttributeEntity(
                    Entity: new Entity("document", "1"),
                    Attribute: "fault_tolerances",
                    Value: new[] { 0.1, 0.2, 0.3, 0.4, 0.5 }
                ),
                new AttributeEntity(
                    Entity: new Entity("document", "1"),
                    Attribute: "yes_or_no",
                    Value: new[] { true, false, true, false }
                ),
            ]
        ), cancellationToken);
    }
}