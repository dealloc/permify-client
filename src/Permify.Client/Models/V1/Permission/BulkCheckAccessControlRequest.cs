using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Permission;

/// <summary>
/// Request for <see cref="IPermissionService.BulkCheckAccessControlAsync" />.
/// </summary>
/// <param name="Metadata"></param>
/// <param name="Arguments"></param>
/// <param name="Items"></param>
/// <param name="Context"></param>
public sealed record BulkCheckAccessControlRequest(
    BulkCheckAccessControlRequest.RequestMetadata Metadata,
    IReadOnlyList<string> Arguments,
    IReadOnlyList<BulkCheckAccessControlRequest.BulkCheckAccessControlRequestItem> Items,
    Context? Context = null
)
{
    /// <summary>
    /// Additional metadata to be sent along with the request.
    /// </summary>
    /// <param name="SnapToken">Token associated with the snap.</param>
    /// <param name="SchemaVersion">Version of the schema.</param>
    /// <param name="Depth">The depth of the check must be greater than or equal to 3.</param>
    public sealed record RequestMetadata(
        string? SnapToken = null,
        string? SchemaVersion = null,
        int Depth = 20
    );

    /// <summary>
    /// A single permission check.
    /// </summary>
    /// <param name="Entity">Entity on which the permission needs to be checked.</param>
    /// <param name="Permission">Name of the permission or relation, required, must start with a letter and can include alphanumeric and underscore, max 64 bytes.</param>
    /// <param name="Subject">Subject for which the permission needs to be checked, required.</param>
    public sealed record BulkCheckAccessControlRequestItem(
        Entity Entity,
        string Permission,
        Subject Subject
    );
};