using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var permify = builder.AddContainer("permify", "permify/permify")
    .WithImageTag("latest")
    .WithImageRegistry("ghcr.io")
    .WithHttpEndpoint(targetPort: 3476, name: "http")
    .WithHttpEndpoint(targetPort: 3478, name: "grpc")
    .WithHttpHealthCheck("/healthz");

builder.AddProject<Permify_Client_AotExample>("aot-example")
    .WithReference(permify.GetEndpoint("http"))
    .WithReference(permify.GetEndpoint("grpc"))
    .WaitFor(permify);

builder.Build().Run();