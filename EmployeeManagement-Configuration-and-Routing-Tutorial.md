# Employee Management Blazor Tutorials

This lesson covers `_Imports.razor`, `Routes.razor`, the `Properties` folder, application settings, `IOptions`, and registering a new setting.

## 1. `_Imports.razor`

`_Imports.razor` is a shared Razor configuration file. It lets components use namespaces without repeating `@using` directives in every `.razor` file.

The current file is:

```razor
@using System.Net.Http
@using System.Net.Http.Json
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using static Microsoft.AspNetCore.Components.Web.RenderMode
@using Microsoft.AspNetCore.Components.Web.Virtualization
@using Microsoft.JSInterop
@using EmployeeManagement.Web
@using EmployeeManagement.Web.Components
@using EmployeeManagement.Web.Components.Layout
```

### Scope

An `_Imports.razor` file applies to its folder and all child folders. For example, `Components/_Imports.razor` affects components under:

```text
Components/
Components/Pages/
Components/Layout/
```

You can create additional `_Imports.razor` files in child folders for more specific imports. A child file inherits parent imports and can add its own imports.

### What it does not do

`_Imports.razor` does not register services, configure routes, execute code, or import JavaScript files. It mainly helps the Razor compiler resolve namespaces and directives.

For example, `NavMenu` can be used by its short name in `MainLayout.razor` because `EmployeeManagement.Web.Components.Layout` is imported.

## 2. `Routes.razor`

`Routes.razor` connects browser URLs to routable Razor components:

```razor
<Router AppAssembly="typeof(Program).Assembly" NotFoundPage="typeof(Pages.NotFound)">
    <Found Context="routeData">
        <RouteView RouteData="routeData" DefaultLayout="typeof(Layout.MainLayout)" />
        <FocusOnNavigate RouteData="routeData" Selector="h1" />
    </Found>
</Router>
```

### `Router`

`Router` examines the current URL and searches the application assembly for a component containing a matching `@page` directive.

For example:

```razor
@page "/"
```

matches the root URL. A component with:

```razor
@page "/employees"
```

matches `/employees`.

### `AppAssembly`

```razor
AppAssembly="typeof(Program).Assembly"
```

This tells Blazor which compiled project assembly to search for routable components. Because `Program.cs` uses top-level statements, the compiler still generates a `Program` type that can be used here.

### `Found` and `routeData`

The `Found` section runs when a route matches. `routeData` contains information about the selected component and route parameters.

`Context="routeData"` gives the found-route value its local name. The name could be changed, but the references below would need to change too.

### `RouteView`

```razor
<RouteView RouteData="routeData" DefaultLayout="typeof(Layout.MainLayout)" />
```

`RouteView` displays the matched component. `DefaultLayout` places it inside `MainLayout` unless the page specifies another layout.

The flow is:

```text
Browser URL
    -> Router finds a component with a matching @page
    -> RouteView displays that component
    -> MainLayout surrounds it
    -> @Body becomes the selected component
```

### `NotFoundPage`

```razor
NotFoundPage="typeof(Pages.NotFound)"
```

This selects the component displayed when no route matches the requested URL.

### `FocusOnNavigate`

```razor
<FocusOnNavigate RouteData="routeData" Selector="h1" />
```

After navigation, Blazor moves focus to the first `h1`. This helps keyboard and screen-reader users recognize that the page changed.

## 3. The `Properties` Folder

Your web project contains:

```text
EmployeeManagement.Web/
└── Properties/
    └── launchSettings.json
```

The `Properties` folder commonly contains project-level development and hosting settings. It does not contain application features.

### `launchSettings.json`

Your file defines two development launch profiles:

```json
"profiles": {
  "http": {
    "commandName": "Project",
    "dotnetRunMessages": true,
    "launchBrowser": true,
    "applicationUrl": "http://localhost:5167",
    "environmentVariables": {
      "ASPNETCORE_ENVIRONMENT": "Development"
    }
  },
  "https": {
    "commandName": "Project",
    "dotnetRunMessages": true,
    "launchBrowser": true,
    "applicationUrl": "https://localhost:7253;http://localhost:5167",
    "environmentVariables": {
      "ASPNETCORE_ENVIRONMENT": "Development"
    }
  }
}
```

A launch profile is a named way to start the application.

- `commandName: Project` runs the project directly.
- `dotnetRunMessages: true` prints startup URLs in the terminal.
- `launchBrowser: true` opens a browser when supported by the launch tool.
- `applicationUrl` tells Kestrel which addresses and ports to use.
- `ASPNETCORE_ENVIRONMENT` selects the ASP.NET Core environment.

The semicolon in the HTTPS profile separates multiple listening addresses:

```text
https://localhost:7253
http://localhost:5167
```

Run a specific profile with:

```bash
dotnet run --project EmployeeManagement.Web --launch-profile http
```

or:

```bash
dotnet run --project EmployeeManagement.Web --launch-profile https
```

### Server address versus route

`applicationUrl` controls the server address. The `@page` directive controls the Blazor route.

For example:

```text
https://localhost:7253/employees
```

contains:

```text
https://localhost:7253  -> server address
/employees              -> Blazor route
```

`launchSettings.json` is mainly for local development. It does not define components, services, Blazor routes, middleware, or production hosting settings.

## 4. `appsettings.json`

`appsettings.json` stores general application configuration and shared defaults.

The current file contains:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Logging

```json
"Default": "Information"
```

logs informational messages and more serious messages for categories without a more specific setting.

```json
"Microsoft.AspNetCore": "Warning"
```

logs ASP.NET Core framework messages only at warning level or higher.

Log levels, from least to most serious, are:

```text
Trace, Debug, Information, Warning, Error, Critical, None
```

### Allowed hosts

```json
"AllowedHosts": "*"
```

allows all host names when host filtering is enabled. In production, this can be restricted to known host names:

```json
"AllowedHosts": "employees.example.com;www.employees.example.com"
```

`AllowedHosts` is not a Blazor route, application URL, or CORS setting.

## 5. `appsettings.Development.json`

The file name follows this pattern:

```text
appsettings.{EnvironmentName}.json
```

Because `launchSettings.json` sets:

```json
"ASPNETCORE_ENVIRONMENT": "Development"
```

ASP.NET Core loads both:

```text
appsettings.json
appsettings.Development.json
```

The development file contains settings that apply only in the Development environment. It overrides matching values from `appsettings.json` while leaving other base values available.

For example, the base file might contain:

```json
{
  "EmployeeSettings": {
    "PageSize": 20,
    "CompanyName": "Contoso"
  }
}
```

The development file could contain:

```json
{
  "EmployeeSettings": {
    "PageSize": 5
  }
}
```

The effective Development configuration would be:

```text
PageSize: 5
CompanyName: Contoso
```

### Configuration priority

`WebApplication.CreateBuilder(args)` automatically loads configuration. In simplified order, later sources override earlier sources:

```text
appsettings.json
    -> appsettings.Development.json
    -> User Secrets
    -> environment variables
    -> command-line arguments
```

Other environment files can be used:

```text
appsettings.Staging.json
appsettings.Production.json
```

### What belongs in appsettings files?

Good examples include:

- Logging levels
- Database connection strings
- API URLs
- Feature flags
- Application options

Do not commit real production passwords, API keys, or other secrets. Use User Secrets during development and environment variables or a secret-management service in production.

## 6. Registering a New Setting

Use three steps.

### Step 1: Add a JSON section

Add this to `appsettings.json`:

```json
"EmployeeSettings": {
  "PageSize": 20,
  "CompanyName": "Contoso"
}
```

For development-only values, add an override to `appsettings.Development.json`:

```json
"EmployeeSettings": {
  "PageSize": 5,
  "CompanyName": "Development Company"
}
```

### Step 2: Create a settings class

Create `EmployeeSettings.cs`:

```csharp
namespace EmployeeManagement.Web;

public class EmployeeSettings
{
    public int PageSize { get; set; }
    public string CompanyName { get; set; } = string.Empty;
}
```

### Step 3: Register the section in `Program.cs`

```csharp
using EmployeeManagement.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EmployeeSettings>(
    builder.Configuration.GetSection("EmployeeSettings"));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
```

`GetSection("EmployeeSettings")` selects the JSON section. `Configure<EmployeeSettings>` binds its values to the C# class and registers the options with dependency injection.

## 7. `IOptions<T>`

`IOptions<T>` is not a C# keyword. It is a .NET interface from:

```csharp
Microsoft.Extensions.Options
```

It provides strongly typed access to registered configuration.

Use it in a Razor component:

```razor
@using Microsoft.Extensions.Options
@inject IOptions<EmployeeSettings> EmployeeOptions

<h1>@EmployeeOptions.Value.CompanyName</h1>
<p>Page size: @EmployeeOptions.Value.PageSize</p>
```

The important parts are:

- `IOptions<EmployeeSettings>` is the framework interface.
- `EmployeeSettings` is your configuration class.
- `EmployeeOptions.Value` contains the configured values.

The overall relationship is:

```text
JSON section
    -> EmployeeSettings class
    -> Configure<EmployeeSettings>(...)
    -> IOptions<EmployeeSettings>
    -> EmployeeOptions.Value
```

You can also read an individual value directly:

```csharp
var pageSize = builder.Configuration.GetValue<int>(
    "EmployeeSettings:PageSize");
```

However, strongly typed options are usually easier to maintain because property names are checked by the compiler and settings have a clear structure.

## 8. Configuration in One Startup Flow

For this application, the startup flow is:

```text
launchSettings.json
    -> sets ASPNETCORE_ENVIRONMENT=Development

WebApplication.CreateBuilder(args)
    -> loads appsettings.json
    -> loads appsettings.Development.json
    -> applies higher-priority configuration sources

Program.cs
    -> reads or binds configuration
    -> registers services

Routes.razor
    -> matches the browser URL to a Razor component

RouteView
    -> displays the component inside MainLayout
```

### Main lesson

- `_Imports.razor` makes namespaces available to Razor components.
- `Routes.razor` maps URLs to components.
- `Properties/launchSettings.json` controls local application startup.
- `appsettings.json` contains shared configuration defaults.
- `appsettings.Development.json` contains Development-specific overrides.
- `IOptions<T>` provides strongly typed access to registered settings.
