# https://just.systems

default:

pre-commit: build format test

ci:
    dotnet clean
    dotnet build ./src/Permify.Client/Permify.Client.csproj --no-restore --configuration Release
    dotnet build ./src/Permify.Client.Grpc/Permify.Client.Grpc.csproj --no-restore --configuration Release
    dotnet build ./src/Permify.Client.Http/Permify.Client.Http.csproj --no-restore --configuration Release
    dotnet test --maximum-parallel-tests 10
    dotnet format --verify-no-changes --exclude examples

build:
    dotnet build

format:
    dotnet format

benchmark:
    dotnet run -c Release -f net10.0 --project ./benchmarks/PermifyClient.Benchmarks -- --join --allStats true 

test:
    dotnet test -f net10.0 --report-trx --github-reporter-style full
    dotnet test -f net9.0 --report-trx --github-reporter-style full
    dotnet test -f net8.0 --report-trx --github-reporter-style full

[working-directory: 'docs']
docs:
    dotnet docfx --serve

kiota:
    podman run -v ./src/Permify.Client.Http/Generated:/app/output mcr.microsoft.com/openapi/kiota generate --language csharp --exclude-backward-compatible -n Permify.Client.Http.Generated --type-access-modifier Internal --openapi https://raw.githubusercontent.com/Permify/permify/27e684d1d36cee2715157ae61e7b835f8610a70b/docs/api-reference/openapiv2/apidocs.swagger.json
