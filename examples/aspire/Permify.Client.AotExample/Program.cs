using Microsoft.AspNetCore.Mvc;

using Permify.Client;
using Permify.Client.Contracts;
using Permify.Client.Models.Schema;

var builder = WebApplication.CreateSlimBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddPermifyCore(builder.Configuration.GetSection("Permify"));
// builder.Services.AddPermifyHttpClients();
builder.Services.AddPermifyGrpcClients();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, PermifyJsonSerializerContext.Default);
});


var app = builder.Build();
app.MapDefaultEndpoints();

app.MapGet("/",
    async static ([FromServices] ISchemaService svc) =>
    await svc.WriteSchemaAsync(new WriteSchemaRequest("entity user {}"), CancellationToken.None));


app.Run();