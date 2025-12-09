using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithArgs("-c", "track_commit_timestamp=on")
    .AddDatabase("permify-db");

var permify = builder.AddContainer("permify", "permify/permify")
    .WithImageTag("latest")
    .WithImageRegistry("ghcr.io")
    .WithHttpEndpoint(port: 3476, targetPort: 3476, name: "http")
    .WithHttpEndpoint(port: 3478, targetPort: 3478, name: "grpc")
    .WithHttpHealthCheck("/healthz")
    .WithEnvironment("PERMIFY_DATABASE_ENGINE", "postgres")
    .WithEnvironment("PERMIFY_DATABASE_URI", postgres.Resource.UriExpression)
    .WithEnvironment("PERMIFY_SERVICE_WATCH_ENABLED", "true");

builder.AddProject<Permify_Client_AotExample>("aot-example")
    .WithReference(permify.GetEndpoint("http"))
    .WithReference(permify.GetEndpoint("grpc"))
    .WaitFor(permify);

builder.Build().Run();