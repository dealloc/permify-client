using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Schema;

/// <summary>
/// Response for <see cref="ISchemaService.WriteSchemaAsync" />
/// </summary>
/// <param name="SchemaVersion">The string that identifies the version of the written schema.</param>
public sealed record WriteSchemaResponse(
    string SchemaVersion
);