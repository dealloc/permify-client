using Permify.Client.Mappers;
using Permify.Client.Models.V1;
using Permify.Client.Models.V1.Watch;

using Riok.Mapperly.Abstractions;

namespace Permify.Client.Grpc.Mappers.V1;

/// <summary>
/// Maps gRPC Schema responses to domain models.
/// </summary>
[Mapper]
[UseStaticMapper(typeof(DateTimeOffsetMapper))]
public static partial class WatchServiceMapper
{
    internal static partial WatchResponse MapToWatchResponse(Base.V1.WatchResponse response);

    // Map DataChanges - manually implemented to use explicit DataChange mapper
    internal static WatchResponse.WatchDataChanges MapToWatchDataChanges(Base.V1.DataChanges changes)
        => new(
            changes.SnapToken,
            changes.DataChanges_.Select(MapToWatchDataChange).ToList()
        );

    // Map DataChange (uses custom mappings below for Tuple/Attribute)
    [MapperIgnoreSource(nameof(Base.V1.DataChange.TypeCase))]
    internal static partial WatchResponse.WatchDataChange MapToWatchDataChange(Base.V1.DataChange change);

    // Map gRPC Tuple to domain Tuple (Subject conversion handled by SubjectToEntity)
    internal static partial Models.V1.Tuple MapToTuple(Base.V1.Tuple tuple);

    // Map gRPC Attribute to domain AttributeEntity
    [MapProperty(nameof(Base.V1.Attribute.Attribute_), nameof(AttributeEntity.Attribute))]
    internal static partial AttributeEntity MapToAttributeEntity(Base.V1.Attribute attribute);

    // Custom mapper: gRPC Entity → domain Entity (direct mapping)
    internal static partial Entity MapToEntity(Base.V1.Entity entity);
}