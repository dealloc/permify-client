namespace Permify.Client.Models.Schema;

/// <summary>
/// Overwrites the Permify schema with a new version.
/// </summary>
/// <param name="Schema">The Permify schema as a string.</param>
public record WriteSchemaRequest(
    string Schema
);