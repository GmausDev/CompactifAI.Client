# Versioning Strategy

This document describes the versioning strategy for the CompactifAI.Client NuGet package.

## Version Alignment with .NET

The package version major number corresponds to the .NET version it targets:

| .NET Version | Package Version | Branch |
|--------------|-----------------|--------|
| .NET 8.0 (LTS) | 8.x.x | `release/net8` |
| .NET 9.0 (STS) | 9.x.x | `release/net9` |
| .NET 10.0 (LTS) | 10.x.x | `main` |

### Semantic Versioning

Within each major version, we follow semantic versioning:

- `X.0.0` - Initial release for .NET X
- `X.Y.0` - New features (minor version bump)
- `X.Y.Z` - Bug fixes (patch version bump)

## Branch Strategy

```
main                  ← Latest LTS (.NET 10)
├── release/net8      ← .NET 8 LTS maintenance
└── release/net9      ← .NET 9 STS maintenance
```

### Branch Lifecycle

1. **main** always contains the latest LTS .NET version
2. When a new .NET LTS releases:
   - Create `release/netX` branch from current `main`
   - Update `main` to target new .NET version
3. Feature branches merge to `main` first, then backported to release branches as needed

## Choosing the Right Version

| Your .NET Version | Recommended Package | Install Command |
|-------------------|---------------------|-----------------|
| .NET 8.0 | 8.x.x | `dotnet add package CompactifAI.Client --version 8.*` |
| .NET 9.0 | 9.x.x | `dotnet add package CompactifAI.Client --version 9.*` |
| .NET 10.0 | 10.x.x | `dotnet add package CompactifAI.Client --version 10.*` |

## Support Policy

| Version | .NET Version | Support Status | End of Support |
|---------|--------------|----------------|----------------|
| 10.x.x | .NET 10 LTS | Active Development | TBD |
| 9.x.x | .NET 9 STS | Maintenance | May 2026 |
| 8.x.x | .NET 8 LTS | Maintenance | November 2026 |

- **Active Development**: New features and bug fixes
- **Maintenance**: Security updates and critical bug fixes only

## Release Process

1. Create a GitHub Release with tag `vX.Y.Z` (e.g., `v8.0.1`, `v10.1.0`)
2. The release workflow automatically:
   - Detects .NET version from tag prefix
   - Builds and tests with the correct .NET SDK
   - Publishes to NuGet.org
   - Attaches package to GitHub Release

## Migration Guide

### From 1.x.x to 8.x.x/9.x.x/10.x.x

The 1.x.x versions have been deprecated. To migrate:

1. Choose the package version matching your .NET version
2. Update your package reference:
   ```xml
   <!-- Before -->
   <PackageReference Include="CompactifAI.Client" Version="1.1.0" />

   <!-- After (for .NET 8) -->
   <PackageReference Include="CompactifAI.Client" Version="8.0.0" />
   ```
3. No API changes required - the migration is seamless

## Contributing

When contributing:

1. Target the `main` branch for new features
2. For bug fixes affecting older versions, create PRs against the appropriate release branch
3. Ensure CI passes for all affected branches
