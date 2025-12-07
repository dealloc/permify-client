This project is a client library for [Permify](https://docs.permify.co/)
It's written in C# and supports `net10.0;net9.0;net8.0;netstandard2.0`
Library is AOT and trimming compatible and treats warnings as errors.
Source code is formatted using `dotnet format`
Uses `just` as a command runner (`see justfile`)
Has top-level `Directory.build.props` and `Directory.build.targets` files
It adheres to the following project structure:
- `src/` -> all library code
- `test/` -> unit and integration tests (XUnit + Aspire)
- `samples/` -> sample application code
- `docs/` -> docfx markdown documentation + theme
- `benchmarks/` -> benchmarking code

Currently contains 3 libraries:
- `Permify.Client` -> core library with models + contracts
- `Permify.Client.Grpc` -> gRPC API client using client generated from proto files (under `proto/` folder)
- `Permify.Client.Http` -> HTTP API client using client generated from Kiota OpenAPI spec (under `/vendor` folder)

Mapping from `Permify.Client` models to `Permify.Client.*` generated code is done with Mapperly.

Coding conventions for AI tools to follow:
- `dotnet format` after making edits
- `dotnet build` to validate edits compile
- `just test` to run unit tests if relevant
- Copy and/or refer to relevant documentation on https://docs.permify.co/ in `Permify.Client` models / interfaces
- Minimize the amount of custom code in mapper functions (prefer having Mapperly generate code).
- documentation should match the wording of the Permify docs as closely as possible.

implementing a new feature / service should follow these steps loosely:
- Generate models (based on OpenAPI / gRPC, optimise for common denominator and don't leak implementation details)
- Write interface (base on Schema service for coding and naming conventions)
- implement gRPC service including mappings
- implement http service including mappings
- write *meaningful* integration tests

integration tests are written using Aspire, write a base test that contains the actual test code
and then provide subclasses for each implementation to ensure both implementations use the same test logic.
