using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Mvc;

using Permify.Client;
using Permify.Client.Contracts.V1;
using Permify.Client.Models.V1.Schema;
using Permify.Client.Models.V1.Watch;

var builder = WebApplication.CreateSlimBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddPermifyCore(builder.Configuration.GetSection("Permify"));
// builder.Services.AddPermifyHttpClients("http://permify");
builder.Services.AddPermifyGrpcClients(options => options.Address = new Uri("http://_grpc.permify"));

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, PermifyJsonSerializerContext.Default);
});


var app = builder.Build();
app.MapDefaultEndpoints();

app.MapGet("/", async static ([FromServices] ISchemaService svc) =>
    await svc.WriteSchemaAsync(new WriteSchemaRequest("entity user {}"), CancellationToken.None));

app.MapGet("/watch", static ([FromServices] IWatchService svc, CancellationToken cancellationToken) =>
{
    async IAsyncEnumerable<WatchResponse> GetWatchResponses(
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        await foreach (var response in svc.WatchAsync(new WatchRequest(), cancellationToken))
        {
            yield return response;
        }
    }

    return TypedResults.ServerSentEvents(
        GetWatchResponses(cancellationToken),
        eventType: "WatchResponse"
    );
});


app.Run();