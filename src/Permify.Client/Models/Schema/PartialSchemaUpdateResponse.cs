using Permify.Client.Contracts;

namespace Permify.Client.Models.Schema;

/// <summary>
/// Response for <see cref="ISchemaService.PartialUpdateSchemaAsync" />
/// </summary>
/// <param name="SchemaVersion">The new schema version after the update.</param>
public sealed record PartialSchemaUpdateResponse(
    string SchemaVersion
);