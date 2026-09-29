# Employee Management Database Setup

This project uses SQL Server with Entity Framework Core. On macOS, SQL Server runs in Docker because LocalDB is a Windows-only feature.

## 1. Start Docker Desktop

Open Docker Desktop and wait until the Docker engine is running.

## 2. Start SQL Server in Docker

Run this command from Terminal:

```bash
docker run \
  --name sqlserver \
  --platform linux/amd64 \
  -e ACCEPT_EULA=Y \
  -e MSSQL_SA_PASSWORD='YourStrongPassword123!' \
  -p 1433:1433 \
  -v sqlserver_data:/var/opt/mssql \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

Explanation:

- `ACCEPT_EULA=Y` accepts the SQL Server license agreement required by the image.
- `MSSQL_SA_PASSWORD` sets the password for the `sa` administrator account.
- `-p 1433:1433` maps Mac port 1433 to SQL Server port 1433 in the container.
- `sqlserver_data` preserves database data when the container stops.
- `--platform linux/amd64` allows SQL Server to run on Apple Silicon through Docker emulation.

Check the container:

```bash
docker ps
docker logs sqlserver
```

## 3. Configure the Web project

Because the Blazor app runs directly on the Mac, it connects through the published Docker port using `localhost`.

In `EmployeeManagement.Web/appsettings.json`:

```json
"ConnectionStrings": {
  "EmployeeDbConnection": "Server=localhost,1433;Database=EmployeeDb;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True;"
}
```

The password must match the password used when the SQL Server container was created.

Do not commit real production passwords to a repository. Use environment variables or your hosting provider's secret settings for production.

## 4. Create and apply EF Core migrations

The EF CLI tool is installed globally with a version matching the project's EF Core packages:

```bash
dotnet tool install --global dotnet-ef --version 10.0.12
export PATH="$PATH:$HOME/.dotnet/tools"
```

Create the migration:

```bash
dotnet ef migrations add InitialCreate \
  --project EmployeeManagement.Api \
  --startup-project EmployeeManagement.Web \
  --context ApplicationDbContext
```

Apply it to SQL Server:

```bash
dotnet ef database update \
  --project EmployeeManagement.Api \
  --startup-project EmployeeManagement.Web \
  --context ApplicationDbContext
```

This creates the `EmployeeDb` database, tables, and seed data.

## 5. Run the Blazor application

```bash
dotnet run --project EmployeeManagement.Web
```

Open the URL shown by .NET, usually:

```text
http://localhost:5167
```

The browser displays the Blazor application, not SQL Server directly.

## 6. View the database in a browser with Adminer

Start Adminer in Docker:

```bash
docker run \
  --name adminer \
  -p 8080:8080 \
  -d adminer
```

Open:

```text
http://localhost:8080
```

Use these login values:

| Field | Value |
|---|---|
| System | Microsoft SQL Server |
| Server | `host.docker.internal,1433` |
| Username | `sa` |
| Password | The SQL Server password |
| Database | `EmployeeDb` |

`host.docker.internal` means the Mac host from inside a Docker container. Adminer needs it because Adminer itself runs in Docker. Do not enter `http://` in the Server field.

Useful Adminer commands:

```bash
docker stop adminer
docker start adminer
```

## 7. Connection addresses by location

| Application location | Server value |
|---|---|
| Blazor app running on the Mac | `localhost,1433` |
| Adminer running in Docker | `host.docker.internal,1433` |
| Another container on the same Docker network | `sqlserver,1433` |
| Production deployment | The production database hostname and port |

`localhost` is correct for the Blazor app because the app runs on the Mac and Docker publishes SQL Server to the Mac's port 1433.

## 8. Project responsibilities

```text
EmployeeManagement.Web
    Blazor pages, layouts, routing, and Program.cs
        references
EmployeeManagement.Api
    ApplicationDbContext and EF Core database configuration
        references
EmployeeManagement.Models
    Employee, Department, and Gender classes
```

`EmployeeManagement.Api` is currently a backend/data-access library used by the Web project. It is not a separate running HTTP API.

## 9. Fixes applied to the solution

- Added the global `dotnet-ef` tool version `10.0.12`.
- Excluded nested `EmployeeManagement.Api/Models/obj` generated files from the API project. Those files were being compiled twice and caused duplicate assembly-attribute errors.
- Added `Microsoft.EntityFrameworkCore.Design` version `10.0.12` to `EmployeeManagement.Web`, which is required by EF migration tools.
- Created and applied the `InitialCreate` migration successfully.

## 10. Container commands

```bash
docker ps
docker stop sqlserver
docker start sqlserver
docker logs sqlserver
```

Stopping the container does not delete the database because the `sqlserver_data` volume stores its data.
