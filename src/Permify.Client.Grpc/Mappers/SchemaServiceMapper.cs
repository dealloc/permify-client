using Base.V1;

using Permify.Client.Mappers;
using Permify.Client.Models.Schema;

using Riok.Mapperly.Abstractions;

namespace Permify.Client.Grpc.Mappers;

/// <summary>
/// Maps gRPC Schema responses to domain models.
/// </summary>
[Mapper]
[UseStaticMapper(typeof(DateTimeOffsetMapper))]
internal static partial class SchemaServiceMapper
{
    /// <summary>
    /// Maps a gRPC SchemaWriteResponse to domain WriteSchemaResponse.
    /// </summary>
    public static partial WriteSchemaResponse MapWriteResponse(SchemaWriteResponse response);

    /// <summary>
    /// Maps a gRPC SchemaListResponse to domain ListSchemaResponse.
    /// </summary>
    public static partial ListSchemaResponse MapListResponse(SchemaListResponse response);

    /// <summary>
    /// Maps a gRPC SchemaPartialWriteResponse to domain PartialSchemaUpdateResponse.
    /// </summary>
    public static partial PartialSchemaUpdateResponse MapPartialUpdateResponse(SchemaPartialWriteResponse response);
}