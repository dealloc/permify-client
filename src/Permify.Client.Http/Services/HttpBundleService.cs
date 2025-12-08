using Microsoft.Extensions.Options;

using Permify.Client.Contracts;
using Permify.Client.Http.Exceptions;
using Permify.Client.Http.Generated;
using Permify.Client.Http.Generated.Models;
using Permify.Client.Http.Generated.Models.Bundle;
using Permify.Client.Http.Generated.V1.Tenants.Item.Bundle;
using Permify.Client.Http.Mappers;
using Permify.Client.Models.Bundles;
using Permify.Client.Options;

namespace Permify.Client.Http.Services;

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
}