# https://just.systems

default:

build:
    dotnet build

format:
    dotnet format

benchmark:
    dotnet run -c Release --project ./benchmarks/PermifyClient.Benchmarks

[working-directory: 'docs']
docs:
    dotnet docfx --serve

kiota:
    podman run -v ./src/Permify.Client.Http/Generated:/app/output mcr.microsoft.com/openapi/kiota generate --language csharp --exclude-backward-compatible -n Permify.Client.Http.Generated --type-access-modifier Internal --openapi https://raw.githubusercontent.com/Permify/permify/887dabd61bbaf1951b93faa563db1e4b26c7caa7/docs/api-reference/openapiv2/apidocs.swagger.json
