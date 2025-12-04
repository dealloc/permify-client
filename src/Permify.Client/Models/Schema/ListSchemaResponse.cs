namespace Permify.Client.Models.Schema;

/// <summary>
/// Represents the response of a <see cref="ListSchemaRequest" />.
/// </summary>
/// <param name="Head">The latest version available for the tenant.</param>
/// <param name="Schemas">A list of schema versions with their corresponding creation timestamps.</param>
/// <param name="ContinuousToken">String that can be used to paginate and retrieve the next set of results.</param>
public sealed record ListSchemaResponse(
    string Head,
    List<ListSchemaResponse.SchemaItem> Schemas,
    string ContinuousToken
)
{
    /// <summary>
    /// Represents a schema version with it's creation timestamp.
    /// </summary>
    public sealed record SchemaItem(
        string Version,
        DateTime CreatedAt
    );
};