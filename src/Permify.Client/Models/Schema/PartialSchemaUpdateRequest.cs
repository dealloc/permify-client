namespace Permify.Client.Models.Schema;

/// <summary>
/// This endpoint allows authorized users to make partial updates to the schema by adding or modifying actions within individual entities.
/// </summary>
/// <param name="Metadata">Optional metadata to pass along the request.</param>
/// <param name="Partials">A list of entities and their operations.</param>
public sealed record PartialSchemaUpdateRequest(
    PartialSchemaUpdateRequest.RequestMetadata? Metadata,
    Dictionary<string, PartialSchemaUpdateRequest.SchemaPartials> Partials
)
{
    /// <summary>
    /// Represents metadata that can be passed into <see cref="PartialSchemaUpdateRequest" />.
    /// </summary>
    /// <param name="SchemaVersion">The string that identifies the version of the schema, an empty string indicates the latest version.</param>
    public sealed record RequestMetadata(string SchemaVersion);

    /// <summary>
    /// Contains partials to write, delete, and update an existing schema.
    /// </summary>
    /// <param name="Write">The new properties to write into the entity.</param>
    /// <param name="Delete">The properties to delete from the entity.</param>
    /// <param name="Update">The properties to update from the entity.</param>
    public sealed record SchemaPartials(
        List<string> Write,
        List<string> Delete,
        List<string> Update
    );
};