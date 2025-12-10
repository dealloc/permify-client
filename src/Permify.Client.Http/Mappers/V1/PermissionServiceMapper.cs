using Permify.Client.Http.Generated.Models;
using Permify.Client.Mappers;
using Permify.Client.Models.V1.Permission;

using Riok.Mapperly.Abstractions;

namespace Permify.Client.Http.Mappers.V1;

/// <summary>
/// Maps HTTP Schema responses to domain models.
/// </summary>
[UseStaticMapper(typeof(DateTimeOffsetMapper))]
[Mapper(
    RequiredMappingStrategy = RequiredMappingStrategy.Target,
    EnumMappingStrategy = EnumMappingStrategy.ByName,
    EnumMappingIgnoreCase = true
)]
internal static partial class PermissionServiceMapper
{
    public static partial CheckAccessControlResponse MapToCheckAccessControlResponse(PermissionCheckResponse response);

    [MapperIgnoreTargetValue(CheckAccessControlResponse.CheckResult.CHECK_RESULT_UNSPECIFIED)]
    private static partial CheckAccessControlResponse.CheckResult MapToCheckResult(CheckResult source);
}