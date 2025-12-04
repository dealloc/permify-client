using Permify.Client.Contracts;
using Permify.Client.Http.Generated;
using Permify.Client.Http.Generated.Models.Schema;
using Permify.Client.Models.Schema;

namespace Permify.Client.Http.Services;

internal sealed class HttpSchemaService(ApiClient api) : ISchemaService
{
    /// <inheritdoc />
    public async Task<WriteSchemaResponse> WriteSchemaAsync(WriteSchemaRequest request,
        CancellationToken cancellationToken)
    {
        var response = await api.V1.Tenants["t1"].Schemas.Write
            .PostAsync(new WriteBody { Schema = request.Schema }, cancellationToken: cancellationToken);

        return new WriteSchemaResponse(response!.SchemaVersion!);
    }
}