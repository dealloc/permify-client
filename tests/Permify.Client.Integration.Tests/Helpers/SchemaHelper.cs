using System.Runtime.CompilerServices;

namespace Permify.Client.Integration.Tests.Helpers;

/// <summary>
/// Helper methods for working with Permify schemas in unit / integration tests.
/// This class assumes that there's a `schemas` folder under the project root.
/// </summary>
public static class SchemaHelper
{
    /// <summary>
    /// Gets the schemas directory path.
    /// </summary>
    private static string GetSchemasDirectory([CallerFilePath] string sourceFilePath = "")
    {
        var directory = Path.GetDirectoryName(sourceFilePath)!;
        var projectRoot = Directory.GetParent(directory)!.FullName;

        return Path.Combine(projectRoot, "schemas");
    }

    /// <summary>
    /// Resolves a relative schema name (e.g. <c>"filename.perm"</c> or <c>"nested/filename.perm"</c> and returns the
    /// schema's contents.
    /// </summary>
    /// <param name="schema">The (relative) filename, which must be relative to the <c>schemas</c> folder.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to cancel the asynchronous operation.</param>
    /// <returns>The contents of the given schema file</returns>
    /// <exception cref="FileNotFoundException">Thrown if the requested file could not be found.</exception>
    public static async Task<string> LoadSchemaAsync(string schema, CancellationToken cancellationToken = default)
    {
        var schemasDir = GetSchemasDirectory();
        var path = Path.Combine(schemasDir, schema);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Schema file not found: {schema}", path);
        }

        return await File.ReadAllTextAsync(path, cancellationToken);
    }

    /// <summary>
    /// Gets all valid schema filenames for use with xUnit MemberData.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}" /> of object arrays, each containing a single schema filename.</returns>
    public static IEnumerable<object[]> GetAllValidSchemas()
    {
        var validDir = Path.Combine(GetSchemasDirectory(), "valid");
        if (!Directory.Exists(validDir))
        {
            yield break;
        }

        foreach (var file in Directory.GetFiles(validDir, "*.perm"))
        {
            var fileName = Path.GetFileName(file);
            yield return [$"valid/{fileName}"];
        }
    }

    /// <summary>
    /// Gets all invalid schema filenames for use with xUnit MemberData.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}" /> of object arrays, each containing a single schema filename.</returns>
    public static IEnumerable<object[]> GetAllInvalidSchemas()
    {
        var invalidDir = Path.Combine(GetSchemasDirectory(), "invalid");
        if (!Directory.Exists(invalidDir))
        {
            yield break;
        }

        foreach (var file in Directory.GetFiles(invalidDir, "*.perm"))
        {
            var fileName = Path.GetFileName(file);
            yield return [$"invalid/{fileName}"];
        }
    }
}