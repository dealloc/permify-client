using Microsoft.Extensions.Options;

using Permify.Client.Contracts.V1;
using Permify.Client.Http.Exceptions;
using Permify.Client.Http.Generated;
using Permify.Client.Http.Generated.Models;
using Permify.Client.Http.Generated.Models.Data;
using Permify.Client.Http.Generated.V1.Tenants.Item.Data;
using Permify.Client.Http.Mappers;
using Permify.Client.Http.Mappers.V1;
using Permify.Client.Models.V1.Data;
using Permify.Client.Options;

namespace Permify.Client.Http.Services.V1;

internal sealed class HttpDataService(
    IOptions<PermifyOptions> options,
    ApiClient api
) : IDataService
{
    /// <summary>Shorthand utility for the <see cref="DataRequestBuilder" />.</summary>
    private DataRequestBuilder Datas => api.V1.Tenants[options.Value.TenantId].Data;

    /// <inheritdoc />
    public async Task<WriteDataResponse> WriteDataAsync(
        WriteDataRequest request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await Datas.Write.PostAsync(new WriteBody
            {
                Metadata = new DataWriteRequestMetadata
                {
                    SchemaVersion = request.Metadata?.SchemaVersion ?? string.Empty
                },
                Tuples = request.Tuples.Select(tuple => new TupleObject
                {
                    Entity = new Entity
                    {
                        Id = tuple.Entity.Id,
                        Type = tuple.Entity.Type
                    },
                    Relation = tuple.Relation,
                    Subject = new Subject
                    {
                        Id = tuple.Subject.Id,
                        Type = tuple.Subject.Type
                    }
                }).ToList(),
                Attributes = request.Attributes.Select(attribute => new AttributeObject
                {
                    Entity = new Entity
                    {
                        Id = attribute.Entity.Id,
                        Type = attribute.Entity.Type
                    },
                    Attribute = attribute.Attribute,
                    Value = AnyValueMapper.MapToAny(attribute.Value)
                }).ToList()
            }, cancellationToken: cancellationToken);

            if (response is null)
                throw new NullReferenceException("Response cannot be null");

            return DataServiceMapper.MapToWriteDataResponse(response);
        }
        catch (Status exception) when (ThrowHelper.ShouldCatchException(exception))
        {
            ThrowHelper.ThrowPermifyClientException(exception);
            throw;
        }
    }
}