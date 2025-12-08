using Microsoft.Extensions.Options;

using Permify.Client.Contracts.V1;
using Permify.Client.Http.Exceptions;
using Permify.Client.Http.Generated;
using Permify.Client.Http.Generated.Models;
using Permify.Client.Http.Generated.Models.Schema;
using Permify.Client.Http.Generated.V1.Tenants.Item.Schemas;
using Permify.Client.Models.V1.Schema;
using Permify.Client.Options;

using SchemaServiceMapper = Permify.Client.Http.Mappers.V1.SchemaServiceMapper;

namespace Permify.Client.Http.Services.V1;

/// <summary>
/// Implements <see cref="ISchemaService"/> using HTTP.
/// </summary>
internal sealed class HttpSchemaService(
    IOptions<PermifyOptions> options,
    ApiClient api
) : ISchemaService
{
    /// <summary>Shorthand utility for the <see cref="SchemasRequestBuilder" />.</summary>
    private SchemasRequestBuilder Schemas => api.V1.Tenants[options.Value.TenantId].Schemas;

    /// <inheritdoc />
    public async Task<WriteSchemaResponse> WriteSchemaAsync(
        WriteSchemaRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var response = await Schemas.Write.PostAsync(new WriteBody { Schema = request.Schema },
                cancellationToken: cancellationToken);

            if (response is null)
                throw new NullReferenceException("Response cannot be null");

            return SchemaServiceMapper.MapWriteResponse(response);
        }
        catch (Status exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ListSchemaResponse> ListSchemaAsync(
        ListSchemaRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var response = await Schemas.List
                .PostAsync(new ListBody { PageSize = request.PageSize, ContinuousToken = request.ContinuousToken },
                    cancellationToken: cancellationToken);

            if (response is null)
                throw new NullReferenceException("Response cannot be null");

            return SchemaServiceMapper.MapListResponse(response);
        }
        catch (Permify.Client.Http.Generated.Models.Status exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PartialSchemaUpdateResponse> PartialUpdateSchemaAsync(
        PartialSchemaUpdateRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var response = await Schemas.PartialWrite.PatchAsync(new PartialWriteBody
            {
                Metadata = new SchemaPartialWriteRequestMetadata
                {
                    SchemaVersion = request.Metadata?.SchemaVersion ?? string.Empty,
                },
                Partials = new PartialWriteBody_partials
                {
                    AdditionalData = request.Partials.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value as object
                    )
                }
            }, cancellationToken: cancellationToken);

            if (response is null)
                throw new NullReferenceException("Response cannot be null");

            return SchemaServiceMapper.MapPartialWriteResponse(response);
        }
        catch (Status exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }
}