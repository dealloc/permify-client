using Base.V1;

using Grpc.Core;

using Permify.Client.Contracts;
using Permify.Client.Grpc.Exceptions;
using Permify.Client.Grpc.Mappers;
using Permify.Client.Models.Tenancy;

namespace Permify.Client.Grpc.Services;

/// <summary>
/// Implements <see cref="ISchemaService"/> using gRPC.
/// </summary>
public sealed class GrpcTenantService(
    Tenancy.TenancyClient client
) : ITenancyService
{
    /// <inheritdoc />
    public async Task<CreateTenantResponse> CreateTenantAsync(CreateTenantRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await client.CreateAsync(
                new TenantCreateRequest
                {
                    Id = request.Id,
                    Name = request.Name
                },
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
}