using Base.V1;

using Permify.Client.Mappers;
using Permify.Client.Models.V1.Bundles;

using Riok.Mapperly.Abstractions;

namespace Permify.Client.Grpc.Mappers.V1;

/// <summary>
/// Maps gRPC Schema responses to domain models.
/// </summary>
[Mapper]
[UseStaticMapper(typeof(DateTimeOffsetMapper))]
internal static partial class BundleServiceMapper
{
    /// <summary>
    /// Maps a gRPC BundleWriteResponse to domain WriteBundleResponse.
    /// </summary>
    public static partial WriteBundleResponse MapToWriteBundleResponse(BundleWriteResponse response);

    /// <summary>
    /// Maps a gRPC ReadBundleResponse to domain BundleReadResponse.
    /// </summary>
    public static partial ReadBundleResponse MapToReadBundleResponse(BundleReadResponse response);
}