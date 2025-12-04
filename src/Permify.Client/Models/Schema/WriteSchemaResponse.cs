namespace Permify.Client.Models.Schema;

/// <summary>
/// Represents the response of a <see cref="WriteSchemaRequest" />.
/// </summary>
/// <param name="SchemaVersion">The string that identifies the version of the written schema.</param>
public sealed record WriteSchemaResponse(
    string SchemaVersion
);