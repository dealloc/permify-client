using Base.V1;

using Permify.Client.Mappers;
using Permify.Client.Models.V1;
using Permify.Client.Models.V1.Data;

using Riok.Mapperly.Abstractions;

namespace Permify.Client.Grpc.Mappers.V1;

/// <summary>
/// Maps gRPC Schema responses to domain models.
/// </summary>
[Mapper]
[UseStaticMapper(typeof(AnyValueMapper))]
[UseStaticMapper(typeof(DateTimeOffsetMapper))]
public static partial class DataServiceMapper
{
    /// <summary>
    /// Maps a gRPC WriteDataResponse to domain DataWriteResponsew.
    /// </summary>
    public static partial WriteDataResponse MapToWriteDataResponse(DataWriteResponse response);

    /// <summary>
    /// Maps a gRPC RelationshipReadResponse to domain ReadRelationshipsResponse.
    /// </summary>
    public static partial ReadRelationshipsResponse MapToReadRelationshipsResponse(RelationshipReadResponse response);

    /// <summary>
    /// Maps a gRPC AttributeReadResponse to domain ReadAttributesResponse.
    /// </summary>
    public static partial ReadAttributesResponse MapToReadAttributesResponse(AttributeReadResponse response);

    /// <summary>
    /// Maps a gRPC BundleRunResponse to domain RunBundleResponse.
    /// </summary>
    public static partial RunBundleResponse MapToRunBundleResponse(BundleRunResponse response);

    /// <summary>
    /// Maps a gRPC Attribute to domain AttributeEntity.
    /// </summary>
    [MapProperty(nameof(Base.V1.Attribute.Attribute_), nameof(AttributeEntity.Attribute))]
    internal static partial AttributeEntity MapToAttributeEntity(Base.V1.Attribute? attribute);
}