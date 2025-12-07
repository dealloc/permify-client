using Permify.Client.Contracts;

namespace Permify.Client.Models.Schema;

/// <summary>
/// Response for <see cref="ISchemaService.WriteSchemaAsync" />
/// </summary>
/// <param name="SchemaVersion">The string that identifies the version of the written schema.</param>
public sealed record WriteSchemaResponse(
    string SchemaVersion
);