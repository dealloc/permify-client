using Base.V1;

using Permify.Client.Mappers;
using Permify.Client.Models.V1.Permission;

using Riok.Mapperly.Abstractions;

namespace Permify.Client.Grpc.Mappers.V1;

/// <summary>
/// Maps gRPC Schema responses to domain models.
/// </summary>
[Mapper]
[UseStaticMapper(typeof(AnyValueMapper))]
[UseStaticMapper(typeof(DateTimeOffsetMapper))]
internal static partial class PermissionServiceMapper
{
    /// <summary>
    /// Maps a gRPC PermissionCheckResponse to domain CheckAccessControlResponse.
    /// </summary>
    public static partial CheckAccessControlResponse MapToCheckAccessControlResponse(PermissionCheckResponse response);
}