using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Data;

/// <summary>
/// Request for <see cref="IDataService.WriteDataAsync" />
/// </summary>
/// <param name="Metadata">Optional metadata to pass along the request.</param>
/// <param name="Tuples">A list of <see cref="Tuple" /> to write.</param>
/// <param name="Attributes">A list of <see cref="AttributeEntity" /> to write.</param>
public sealed record WriteDataRequest(
    WriteDataRequest.RequestMetadata Metadata,
    List<Tuple> Tuples,
    List<AttributeEntity> Attributes
)
{
    /// <summary>
    /// Allows passing in additional request metadata to the <see cref="WriteDataRequest" />.
    /// </summary>
    /// <param name="SchemaVersion">An optional string indicating for which schema this data is valid.</param>
    public sealed record RequestMetadata(string? SchemaVersion = null);
};