using Permify.Client.Contracts;

namespace Permify.Client.Models.Schema;

/// <summary>
/// Request for <see cref="ISchemaService.WriteSchemaAsync" />
/// </summary>
/// <param name="Schema">The Permify schema as a string.</param>
public record WriteSchemaRequest(
    string Schema
);