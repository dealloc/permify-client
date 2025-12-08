using Permify.Client.Contracts;
using Permify.Client.Http.Exceptions;
using Permify.Client.Http.Generated;
using Permify.Client.Http.Generated.Models;
using Permify.Client.Http.Mappers;
using Permify.Client.Models.Tenancy;

namespace Permify.Client.Http.Services;

internal sealed class HttpTenancyService(ApiClient api) : ITenancyService
{
    /// <inheritdoc />
    public async Task<CreateTenantResponse> CreateTenantAsync(
        CreateTenantRequest request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await api.V1.Tenants.Create.PostAsync(
                new TenantCreateRequest
                {
                    Id = request.Id,
                    Name = request.Name
                },
                cancellationToken: cancellationToken);

            if (response is null)
                throw new NullReferenceException("Response cannot be null");

            return TenancyServiceMapper.MapCreateTenantResponse(response);
        }
        catch (Status exception) when (ThrowHelper.ShouldCatchException(exception))
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
            var response = await api.V1.Tenants.List.PostAsync(
                new TenantListRequest
                {
                    PageSize = request.PageSize,
                    ContinuousToken = request.ContinuousToken
                },
                cancellationToken: cancellationToken);

            if (response is null)
                throw new NullReferenceException("Response cannot be null");

            return TenancyServiceMapper.MapListTenantResponse(response);
        }
        catch (Status exception) when (ThrowHelper.ShouldCatchException(exception))
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
            var response = await api.V1.Tenants[request.Id].DeleteAsync(cancellationToken: cancellationToken);

            if (response is null)
                throw new NullReferenceException("Response cannot be null");

            return TenancyServiceMapper.MapDeleteTenantResponse(response);
        }
        catch (Status exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }
}