namespace Permify.Client.Integration.Tests.TestSchemas;

/// <summary>
/// Provides test data for schema-related tests.
/// </summary>
public static class SchemaTestData
{
    /// <summary>
    /// Returns all valid schemas as test data.
    /// Each item is: [description, schemaContent]
    /// </summary>
    public static IEnumerable<object[]> ValidSchemas()
    {
        foreach (var (fileName, content) in SchemaLoader.GetAllValid())
        {
            // Use filename without extension as the test case description
            var description = Path.GetFileNameWithoutExtension(fileName);
            yield return new object[] { description, content };
        }
    }

    /// <summary>
    /// Returns all invalid schemas as test data.
    /// Each item is: [description, schemaContent]
    /// </summary>
    public static IEnumerable<object[]> InvalidSchemas()
    {
        foreach (var (fileName, content) in SchemaLoader.GetAllInvalid())
        {
            var description = Path.GetFileNameWithoutExtension(fileName);
            yield return new object[] { description, content };
        }
    }

    /// <summary>
    /// Returns specific valid schemas by filename.
    /// Each item is: [description, schemaContent]
    /// </summary>
    public static IEnumerable<object[]> SpecificValidSchemas(params string[] fileNames)
    {
        foreach (var fileName in fileNames)
        {
            var description = Path.GetFileNameWithoutExtension(fileName);
            var content = SchemaLoader.LoadValid(fileName);
            yield return new object[] { description, content };
        }
    }

    /// <summary>
    /// Returns specific invalid schemas by filename.
    /// Each item is: [description, schemaContent]
    /// </summary>
    public static IEnumerable<object[]> SpecificInvalidSchemas(params string[] fileNames)
    {
        foreach (var fileName in fileNames)
        {
            var description = Path.GetFileNameWithoutExtension(fileName);
            var content = SchemaLoader.LoadInvalid(fileName);
            yield return new object[] { description, content };
        }
    }
}