var builder = DistributedApplication.CreateBuilder(args);

builder.AddContainer("permify", "permify/permify")
    .WithImageTag("latest")
    .WithImageRegistry("ghcr.io")
    .WithHttpEndpoint(targetPort: 3476, name: "http")
    .WithHttpEndpoint(targetPort: 3478, name: "grpc")
    .WithHttpHealthCheck("/healthz");

builder.Build().Run();