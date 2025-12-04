namespace Permify.Client.Integration.Tests.TestSchemas;

/// <summary>
/// Loads Permify schema files from the schemas directory at the solution root.
/// </summary>
public static class SchemaLoader
{
    private static string GetSolutionRoot()
    {
        var currentDir = Directory.GetCurrentDirectory();

        // Walk up the directory tree until we find the solution root
        // (indicated by the presence of schemas directory)
        while (currentDir != null)
        {
            var testSchemasDir = Path.Combine(currentDir, "test-schemas");
            if (Directory.Exists(testSchemasDir))
            {
                return currentDir;
            }

            currentDir = Directory.GetParent(currentDir)?.FullName;
        }

        throw new DirectoryNotFoundException(
            "Could not find solution root. Make sure tests are run from the solution directory " +
            "and that the 'schemas' folder exists at the solution root.");
    }

    private static string GetSchemaDirectory()
    {
        return Path.Combine(GetSolutionRoot(), "test-schemas");
    }

    /// <summary>
    /// Loads a valid schema file from schemas/valid/{fileName}.
    /// </summary>
    public static string LoadValid(string fileName)
    {
        var path = Path.Combine(GetSchemaDirectory(), "valid", fileName);
        if (File.Exists(path) is false)
        {
            throw new FileNotFoundException($"Valid schema file not found: {fileName}", path);
        }
        return File.ReadAllText(path);
    }

    /// <summary>
    /// Loads an invalid schema file from schemas/invalid/{fileName}.
    /// </summary>
    public static string LoadInvalid(string fileName)
    {
        var path = Path.Combine(GetSchemaDirectory(), "invalid", fileName);
        if (File.Exists(path) is false)
        {
            throw new FileNotFoundException($"Invalid schema file not found: {fileName}", path);
        }
        return File.ReadAllText(path);
    }

    /// <summary>
    /// Gets all valid schema files as (fileName, content) tuples.
    /// </summary>
    public static IEnumerable<(string FileName, string Content)> GetAllValid()
    {
        var validDir = Path.Combine(GetSchemaDirectory(), "valid");
        if (Directory.Exists(validDir) is false)
        {
            yield break;
        }

        foreach (var file in Directory.GetFiles(validDir, "*.perm"))
        {
            var fileName = Path.GetFileName(file);
            var content = File.ReadAllText(file);
            yield return (fileName, content);
        }
    }

    /// <summary>
    /// Gets all invalid schema files as (fileName, content) tuples.
    /// </summary>
    public static IEnumerable<(string FileName, string Content)> GetAllInvalid()
    {
        var invalidDir = Path.Combine(GetSchemaDirectory(), "invalid");
        if (Directory.Exists(invalidDir) is false)
        {
            yield break;
        }

        foreach (var file in Directory.GetFiles(invalidDir, "*.perm"))
        {
            var fileName = Path.GetFileName(file);
            var content = File.ReadAllText(file);
            yield return (fileName, content);
        }
    }
}