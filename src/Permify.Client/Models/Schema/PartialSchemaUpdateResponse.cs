namespace Permify.Client.Models.Schema;

/// <summary>
/// Response from a <see cref="PartialSchemaUpdateRequest" />.
/// </summary>
/// <param name="SchemaVersion">The new schema version after the update.</param>
public sealed record PartialSchemaUpdateResponse(
    string SchemaVersion
);