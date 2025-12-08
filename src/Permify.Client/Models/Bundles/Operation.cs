namespace Permify.Client.Models.Bundles;

/// <summary>
/// Represents an operation that will execute when the containing bundle runs.
/// </summary>
/// <param name="AttributesWrite">A list of attributes to write.</param>
/// <param name="AttributesDelete">A list of attributes to delete.</param>
/// <param name="RelationshipsWrite">A list of relationships to write.</param>
/// <param name="RelationshipsDelete">A list of relationships to delete.</param>
/// <example>
/// Relationships follow the following format (example from Permify docs):
/// <code>
/// "organization:{{.organizationID}}#admin@user:{{.creatorID}}"
/// </code>
///
/// Attributes follow the following format (example from Permify docs):
/// <code>
/// "organization:{{.organizationID}}$public|boolean:false"
/// </code>
///
/// For the full bundle example code, refer to <a href="https://docs.permify.co/api-reference/bundle/write-bundle#how-bundles-works">the docs</a>.
/// </example>
public sealed record Operation(
    IReadOnlyList<string> AttributesWrite,
    IReadOnlyList<string> AttributesDelete,
    IReadOnlyList<string> RelationshipsWrite,
    IReadOnlyList<string> RelationshipsDelete
);