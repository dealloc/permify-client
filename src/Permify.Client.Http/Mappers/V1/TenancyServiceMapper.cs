using Permify.Client.Http.Generated.Models;
using Permify.Client.Mappers;
using Permify.Client.Models.V1.Tenancy;

using Riok.Mapperly.Abstractions;

namespace Permify.Client.Http.Mappers.V1;

/// <summary>
/// Maps HTTP Tenancy responses to domain models.
/// </summary>
[UseStaticMapper(typeof(DateTimeOffsetMapper))]
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
internal static partial class TenancyServiceMapper
{
    public static partial CreateTenantResponse MapCreateTenantResponse(TenantCreateResponse response);

    public static partial ListTenantResponse MapListTenantResponse(TenantListResponse response);

    public static partial DeleteTenantResponse MapDeleteTenantResponse(TenantDeleteResponse response);
}