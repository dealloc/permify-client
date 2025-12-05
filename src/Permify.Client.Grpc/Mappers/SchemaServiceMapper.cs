using Base.V1;

using Permify.Client.Mappers;

using Riok.Mapperly.Abstractions;

using Permify.Client.Models.Schema;

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
}