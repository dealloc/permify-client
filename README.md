# Permify .NET Client

[![Permify v1.5.4](https://img.shields.io/badge/Permify-v1.5.4-4F14CC)](https://docs.permify.co)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)

Unofficial .NET client for [Permify](https://permify.co) with support for both HTTP and gRPC APIs.
Designed for AOT compatibility with optional Aspire integration.

## Features

- HTTP and gRPC API support
- 100% Native AOT compatible
- Optional Aspire integration
- Modern .NET with maximum compatibility (.NET 10, .NET 9, .NET 8, netstandard2.0)

## Documentation

See [Permify documentation](https://docs.permify.co) for API details.

## License

[MIT](LICENSE.md)

## Roadmap

This is the current roadmap for the unofficial Permify client, in no particular order:

- [ ] Schema Service
    - [ ] Read schema (polymorphic trees are difficult)
- [ ] Permission Service
    - [ ] Expand API
    - [ ] Subject Filtering
    - [ ] Lookup Entity (Data Filtering)
    - [ ] Lookup Entity (Streaming)
    - [ ] Subject Permission List
- [ ] Bundle Service
    - [ ] Delete Bundle
- [ ] OpenTelemetry integration
- [ ] Authn support for Permify servers that require it
