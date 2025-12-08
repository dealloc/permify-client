using Base.V1;

using Permify.Client.Mappers;
using Permify.Client.Models.Tenancy;

using Riok.Mapperly.Abstractions;

namespace Permify.Client.Grpc.Mappers;

/// <summary>
/// Maps gRPC Tenancy responses to domain models.
/// </summary>
[UseStaticMapper(typeof(DateTimeOffsetMapper))]
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
internal static partial class TenancyServiceMapper
{
    public static partial CreateTenantResponse MapCreateTenantResponse(TenantCreateResponse response);

    public static partial ListTenantResponse MapListTenantResponse(TenantListResponse response);

    public static partial DeleteTenantResponse MapDeleteTenantResponse(TenantDeleteResponse response);
}