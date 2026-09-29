# Quick Database Commands

```bash
# Start SQL Server

docker run --name employee-sqlserver --platform linux/amd64 \
  -e ACCEPT_EULA=Y \
  -e MSSQL_SA_PASSWORD='YourStrongPassword123!' \
  -p 1433:1433 \
  -d mcr.microsoft.com/mssql/server:2022-latest

# Build API

dotnet build EmployeeManagement.Api/EmployeeManagement.Api.csproj

# Create migration

dotnet ef migrations add InitialCreate \
  --project EmployeeManagement.Api/EmployeeManagement.Api.csproj \
  --startup-project EmployeeManagement.Web/EmployeeManagement.Web.csproj \
  --context AppDbContext \
  --output-dir Migrations

# Apply migration

dotnet ef database update \
  --project EmployeeManagement.Api/EmployeeManagement.Api.csproj \
  --startup-project EmployeeManagement.Web/EmployeeManagement.Web.csproj \
  --context AppDbContext

# Start CloudBeaver

docker run --name cloudbeaver -p 127.0.0.1:8978:8978 -d dbeaver/cloudbeaver:latest

# Open browser
http://localhost:8978

# Run the web app

dotnet run --project EmployeeManagement.Web/EmployeeManagement.Web.csproj
```

## CloudBeaver connection details

- Host: `host.docker.internal`
- Port: `1433`
- Database: `EmployeeDb`
- Username: `sa`
- Password: `YourStrongPassword123!`

## Useful restarts

```bash
docker restart employee-sqlserver
docker restart cloudbeaver
```

## Useful cleanup

```bash
docker rm -f employee-sqlserver
docker rm -f cloudbeaver
```
