using Permify.Client.Contracts.V1;

namespace Permify.Client.Models.V1.Schema;

/// <summary>
/// Request for <see cref="ISchemaService.WriteSchemaAsync" />
/// </summary>
/// <param name="Schema">The Permify schema as a string.</param>
public record WriteSchemaRequest(
    string Schema
);