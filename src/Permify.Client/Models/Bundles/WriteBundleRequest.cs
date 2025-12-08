using Permify.Client.Contracts;

namespace Permify.Client.Models.Bundles;

/// <summary>
/// Request for <see cref="IBundleService.WriteBundleAsync" />
/// </summary>
/// <param name="Bundles">The <see cref="Bundle" />s to write.</param>
/// <example>
/// The <a href="">official Permify docs</a> contain a full example of a bundle to write:
/// <code>
/// {
///   "tenant_id": "t1",
///   "bundles": [
///     {
///       "name": "organization_created",
///       "arguments": [
///         "creatorID",
///         "organizationID"
///       ],
///       "operations": [
///         {
///           "relationships_write": [
///             "organization:{{.organizationID}}#admin@user:{{.creatorID}}",
///             "organization:{{.organizationID}}#manager@user:{{.creatorID}}"
///           ],
///           "attributes_write": [
///             "organization:{{.organizationID}}$public|boolean:false"
///           ]
///         }
///       ]
///     }
///   ]
/// }
/// </code>
///
/// Here's how to express the same bundle as a <see cref="WriteBundleRequest" />:
/// <code>
/// await bundleService.WriteBundleAsync(new([
///     new("bundle 1", [
///         "creatorID",
///         "organizationID"
///     ], [
///         new(
///             AttributesWrite: [
///                 "organization:{{.organizationID}}$public|boolean:false"
///             ],
///             AttributesDelete: [],
///             RelationshipsWrite: [
///                 "organization:{{.organizationID}}#admin@user:{{.creatorID}}",
///                 "organization:{{.organizationID}}#manager@user:{{.creatorID}}"
///             ],
///             RelationshipsDelete: []
///         )
///     ])
/// ]), cancellationToken);
/// </code>
/// </example>
public sealed record WriteBundleRequest(
    IReadOnlyList<WriteBundleRequest.Bundle> Bundles
)
{
    /// <summary>
    /// Represents a single <a href="https://docs.permify.co/api-reference/bundle">bundle</a> to write to Permify.
    /// </summary>
    /// <param name="Name">The name / identifier of the bundle to write.</param>
    /// <param name="Arguments">A list of arguments that can be used in the <see cref="Operations" />.</param>
    /// <param name="Operations">A list of write/update/deletes that should occur when this bundle executes.</param>
    public sealed record Bundle(
        string Name,
        IReadOnlyList<string> Arguments,
        IReadOnlyList<Operation> Operations
    );

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
};