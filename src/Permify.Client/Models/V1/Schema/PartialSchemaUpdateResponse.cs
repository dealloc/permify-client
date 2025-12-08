using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Schema;

/// <summary>
/// Response for <see cref="ISchemaService.PartialUpdateSchemaAsync" />
/// </summary>
/// <param name="SchemaVersion">The new schema version after the update.</param>
public sealed record PartialSchemaUpdateResponse(
    string SchemaVersion
);