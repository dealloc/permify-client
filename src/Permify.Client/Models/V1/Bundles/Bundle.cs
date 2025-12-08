namespace Permify.Client.Models.V1.Bundles;

/// <summary>
/// Represents a single <a href="https://docs.permify.co/api-reference/bundle">bundle</a> to write to Permify.
/// </summary>
/// <param name="Name">The name / identifier of the bundle to write.</param>
/// <param name="Arguments">A list of arguments that can be used in the <see cref="Operations" />.</param>
/// <param name="Operations">A list of write/update/deletes that should occur when this bundle executes.</param>
public sealed record Bundle(
    string Name,
    IReadOnlyList<string> Arguments,
    IReadOnlyList<Operation> Operations
);