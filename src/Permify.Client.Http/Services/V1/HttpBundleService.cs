using Microsoft.Extensions.Options;

using Permify.Client.Contracts.V1;
using Permify.Client.Http.Exceptions;
using Permify.Client.Http.Generated;
using Permify.Client.Http.Generated.Models;
using Permify.Client.Http.Generated.Models.Bundle;
using Permify.Client.Http.Generated.V1.Tenants.Item.Bundle;
using Permify.Client.Models.V1.Bundles;
using Permify.Client.Options;

using BundleServiceMapper = Permify.Client.Http.Mappers.V1.BundleServiceMapper;

namespace Permify.Client.Http.Services.V1;

/// <summary>
/// Implements <see cref="IBundleService"/> using HTTP.
/// </summary>
internal sealed class HttpBundleService(
    IOptions<PermifyOptions> options,
    ApiClient api
) : IBundleService
{
    /// <summary>Shorthand utility for the <see cref="BundleRequestBuilder" />.</summary>
    private BundleRequestBuilder Bundles => api.V1.Tenants[options.Value.TenantId].Bundle;

    /// <inheritdoc />
    public async Task<WriteBundleResponse> WriteBundleAsync(
        WriteBundleRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var response = await Bundles.Write.PostAsync(
                new WriteBody
                {
                    Bundles = request.Bundles.Select(bundle => new DataBundle
                    {
                        Name = bundle.Name,
                        Arguments = bundle.Arguments.ToList(),
                        Operations = bundle.Operations.Select(operation =>
                            new Permify.Client.Http.Generated.Models.V1.Operation
                            {
                                AttributesWrite = operation.AttributesWrite.ToList(),
                                AttributesDelete = operation.AttributesDelete.ToList(),
                                RelationshipsWrite = operation.RelationshipsWrite.ToList(),
                                RelationshipsDelete = operation.RelationshipsDelete.ToList()
                            }).ToList()
                    }).ToList()
                },
                cancellationToken: cancellationToken
            );

            if (response is null)
                throw new NullReferenceException("Response cannot be null");

            return BundleServiceMapper.MapWriteBundleResponse(response);
        }
        catch (Status exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ReadBundleResponse> ReadBundleAsync(
        ReadBundleRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var response = await Bundles.Read.PostAsync(
                new ReadBody
                {
                    Name = request.Name
                },
                cancellationToken: cancellationToken
            );

            if (response is null)
                throw new NullReferenceException("Response cannot be null");

            return BundleServiceMapper.MapReadBundleResponse(response);
        }
        catch (Status exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }
}