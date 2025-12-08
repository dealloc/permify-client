using Permify.Client.Http.Generated.Models;
using Permify.Client.Mappers;
using Permify.Client.Models.Schema;

using Riok.Mapperly.Abstractions;

namespace Permify.Client.Http.Mappers;

/// <summary>
/// Maps HTTP Schema responses to domain models.
/// </summary>
[UseStaticMapper(typeof(DateTimeOffsetMapper))]
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
internal static partial class SchemaServiceMapper
{
    /// <summary>
    /// Maps a Kiota SchemaWriteResponse to domain WriteSchemaResponse.
    /// </summary>
    [MapperIgnoreSource(nameof(SchemaWriteResponse.AdditionalData))]
    public static partial WriteSchemaResponse MapWriteResponse(SchemaWriteResponse response);

    /// <summary>
    /// Maps a Kiota SchemaListResponse to domain ListSchemaResponse.
    /// </summary>
    public static partial ListSchemaResponse MapListResponse(SchemaListResponse response);

    /// <summary>
    /// Maps a Kiota SchemaPartialWriteResponse to domain PartialSchemaUpdateResponse.
    /// </summary>
    public static partial PartialSchemaUpdateResponse MapPartialWriteResponse(SchemaPartialWriteResponse response);
}