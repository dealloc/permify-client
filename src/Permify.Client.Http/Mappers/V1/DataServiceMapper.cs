using Permify.Client.Http.Generated.Models;
using Permify.Client.Mappers;
using Permify.Client.Models.V1.Data;

using Riok.Mapperly.Abstractions;

namespace Permify.Client.Http.Mappers.V1;

/// <summary>
/// Maps HTTP Schema responses to domain models.
/// </summary>
[UseStaticMapper(typeof(DateTimeOffsetMapper))]
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
internal static partial class DataServiceMapper
{
    public static partial WriteDataResponse MapToWriteDataResponse(DataWriteResponse response);

    public static partial ReadRelationshipsResponse MapToReadRelationshipsResponse(RelationshipReadResponse response);
}