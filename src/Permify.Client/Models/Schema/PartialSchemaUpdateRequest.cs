using Permify.Client.Contracts;

namespace Permify.Client.Models.Schema;

/// <summary>
/// Request for <see cref="ISchemaService.PartialUpdateSchemaAsync" />
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
    /// <param name="Write">Conditions to be added. If a relation or permission/action already exists, it should return an error.</param>
    /// <param name="Delete">
    /// Names (permissions/actions) to be deleted.
    /// If the relation/permission/action name does not exist, it should return an error.
    /// Note: specifying the name is enough as relation/permission/action names should be unique.
    /// </param>
    /// <param name="Update">Conditions to be updated.</param>
    public sealed record SchemaPartials(
        List<string> Write,
        List<string> Delete,
        List<string> Update
    );
};