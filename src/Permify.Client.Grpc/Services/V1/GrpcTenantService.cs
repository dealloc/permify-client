using Base.V1;

using Grpc.Core;

using Permify.Client.Contracts.V1;
using Permify.Client.Grpc.Exceptions;
using Permify.Client.Models.V1.Tenancy;

using TenancyServiceMapper = Permify.Client.Grpc.Mappers.V1.TenancyServiceMapper;

namespace Permify.Client.Grpc.Services.V1;

/// <summary>
/// Implements <see cref="ISchemaService"/> using gRPC.
/// </summary>
internal sealed class GrpcTenantService(
    Tenancy.TenancyClient client
) : ITenancyService
{
    /// <inheritdoc />
    public async Task<CreateTenantResponse> CreateTenantAsync(CreateTenantRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await client.CreateAsync(
                new TenantCreateRequest { Id = request.Id, Name = request.Name },
                cancellationToken: cancellationToken
            ).ResponseAsync;

            if (response is null)
                throw new NullReferenceException("Response cannot be null");

            return TenancyServiceMapper.MapCreateTenantResponse(response);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ListTenantResponse> ListTenantsAsync(
        ListTenantRequest request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await client.ListAsync(
                new TenantListRequest
                {
                    PageSize = request.PageSize,
                    ContinuousToken = request.ContinuousToken ?? string.Empty,
                },
                cancellationToken: cancellationToken
            ).ResponseAsync;

            if (response is null)
                throw new NullReferenceException("Response cannot be null");

            return TenancyServiceMapper.MapListTenantResponse(response);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<DeleteTenantResponse> DeleteTenantAsync(
        DeleteTenantRequest request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await client.DeleteAsync(
                new TenantDeleteRequest { Id = request.Id },
                cancellationToken: cancellationToken
            ).ResponseAsync;

            if (response is null)
                throw new NullReferenceException("Response cannot be null");

            return TenancyServiceMapper.MapDeleteTenantResponse(response);
        }
        catch (RpcException exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }
}