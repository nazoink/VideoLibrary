# .NET net8.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET net8.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET net8.0 upgrade.
3. Upgrade VideoLibrary\VideoLibrary.csproj


## Settings

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name | Description |
|:-----------------------------------------------|:---------------------------:|


### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name | Current Version | New Version | Description |
|:------------------------------------|:---------------:|:-----------:|:----------------------------------------------|
| Microsoft.Data.SqlClient |3.0.0 |6.1.2 | Security vulnerability and deprecated version (CVE-2024-0056). Recommended update to6.1.2. |
| Microsoft.EntityFrameworkCore |3.1.15 |8.0.21 | Upgrade EF Core to8.0 to match target framework. |
| Microsoft.EntityFrameworkCore.Abstractions |3.1.15 |8.0.21 | Upgrade EF Core packages to8.0.21. |
| Microsoft.EntityFrameworkCore.SqlServer |3.1.15 |8.0.21 | Upgrade EF Core SQL Server provider to8.0.21. |
| Microsoft.Extensions.Configuration |3.1.15 |8.0.0 | Upgrade to latest configuration package compatible with .NET8. |
| Microsoft.Extensions.Http |3.1.15 |8.0.1 | Upgrade HTTP client extensions for .NET8 compatibility. |
| Microsoft.NET.Sdk.Functions |3.0.12 | | In-proc Functions SDK3.x is not compatible with .NET8. Recommendation: migrate to isolated worker model and add packages `Microsoft.Azure.Functions.Worker`1.24.0, `Microsoft.Azure.Functions.Worker.Sdk`1.18.1 and `Microsoft.Azure.Functions.Worker.Extensions.Http`3.3.0. Alternatively, if you must keep in-proc, update to `Microsoft.NET.Sdk.Functions`4.6.0 and keep in-proc model (additional migration work may still be required). |
| System.Data.SqlClient |4.8.2 |4.9.0 | Security/stability update. Recommended update to4.9.0. |


### Project upgrade details

#### VideoLibrary\\VideoLibrary.csproj modifications

Project properties changes:
 - Target framework should be changed from `netcoreapp3.1` to `net8.0`.

NuGet packages changes:
 - `Microsoft.Data.SqlClient` should be updated from `3.0.0` to `6.1.2` (*security vulnerability*).
 - `Microsoft.EntityFrameworkCore` should be updated from `3.1.15` to `8.0.21`.
 - `Microsoft.EntityFrameworkCore.Abstractions` should be updated from `3.1.15` to `8.0.21`.
 - `Microsoft.EntityFrameworkCore.SqlServer` should be updated from `3.1.15` to `8.0.21`.
 - `Microsoft.Extensions.Configuration` should be updated from `3.1.15` to `8.0.0`.
 - `Microsoft.Extensions.Http` should be updated from `3.1.15` to `8.0.1`.
 - `System.Data.SqlClient` should be updated from `4.8.2` to `4.9.0`.
 - `Microsoft.NET.Sdk.Functions` should be replaced: recommended migration to isolated worker packages `Microsoft.Azure.Functions.Worker`1.24.0, `Microsoft.Azure.Functions.Worker.Sdk`1.18.1 and `Microsoft.Azure.Functions.Worker.Extensions.Http`3.3.0. If you prefer to remain in-proc, update `Microsoft.NET.Sdk.Functions` to `4.6.0` and follow in-proc compatibility guidance.

Feature upgrades:
 - Azure Functions: This project targets Azure Functions v3 in-process model. .NET8 requires migrating to the isolated worker model (recommended) or upgrading to Functions v4+ in-proc with careful compatibility checks. Migration tasks include:
 - Replace `FunctionsStartup` usage and `Microsoft.Azure.Functions.Extensions.DependencyInjection` patterns with isolated worker host startup (create `Program.cs` with `Host.CreateDefaultBuilder().ConfigureFunctionsWorkerDefaults()` and register services via dependency injection there).
 - Replace package `Microsoft.NET.Sdk.Functions` usage with `Microsoft.Azure.Functions.Worker.*` packages and update project SDK declarations as required.
 - Update function classes (HttpTrigger bindings attributes remain but some binding types and extension registrations differ).
 - Move configuration initialization to host builder (use `builder.Configuration` or `HostBuilder` configuration APIs) instead of manual `ConfigurationBuilder` in `Startup`.
 - Update any code that depends on in-proc-only APIs to their isolated equivalents.

Other changes:
 - Check `LangVersion` in project file. For .NET8 you can rely on default latest language, but consider setting `<LangVersion>latest</LangVersion>` if necessary.
 - Update any runtime-specific APIs that are removed or changed between .NET Core3.1 and .NET8 (analyzers will catch many issues).

