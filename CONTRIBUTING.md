# Contributing

## Commit Format

Use [Conventional Commits](https://www.conventionalcommits.org/):

```
<type>: <description>

[optional body]
```

Types: `feat`, `fix`, `docs`, `chore`, `refactor`, `test`, `perf`

## Project Structure

- `/src` - Source code
- `/tests` - Test projects
- `/examples` - Example applications

## Requirements

- All code must be AOT compatible
- No reflection or dynamic code generation
- Source generators for serialization where needed
- Trim-safe APIs

## Development

```bash
dotnet build
dotnet test
dotnet publish -c Release # Validates AOT compatibility
```

## Updating Kioto generated HTTP client

We use [`just`](https://just.systems/) as a task runner, regenerating the Kiota HTTP client looks like this:

```bash
just kiota
```

If you don't want to install `just`, you can open the `justfile` and execute the command manually.
This currently requires having `podman` (or `docker`) installed since the Kiota CLI does not support
.NET 10 yet, but the generated code is compatible with it.

## Pull Requests

- Target `master` branch
- Include tests for new features
- Ensure AOT compatibility with `PublishAot` builds
- Update examples if adding public APIs
- no merge commits in your fork (rebase if updating from upstream)