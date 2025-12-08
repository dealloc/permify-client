using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Bundles;

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
    IReadOnlyList<Bundle> Bundles
);