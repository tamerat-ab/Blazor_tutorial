# Database Setup Runbook

## 1) Start SQL Server in Docker

```bash
docker run --name employee-sqlserver --platform linux/amd64 \
  -e ACCEPT_EULA=Y \
  -e MSSQL_SA_PASSWORD='YourStrongPassword123!' \
  -p 1433:1433 \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

Check status:

```bash
docker ps --filter name=employee-sqlserver
```

If it is already running:

```bash
docker start employee-sqlserver
```

---

## 2) Verify app connection string

In `EmployeeManagement.Web/appsettings.json`:

```json
"ConnectionStrings": {
  "EmployeeDbConnection": "Server=localhost,1433;Database=EmployeeDb;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True;"
}
```

---

## 3) Build the API project

```bash
dotnet build EmployeeManagement.Api/EmployeeManagement.Api.csproj
```

---

## 4) Create EF Core migration

```bash
dotnet ef migrations add InitialCreate \
  --project EmployeeManagement.Api/EmployeeManagement.Api.csproj \
  --startup-project EmployeeManagement.Web/EmployeeManagement.Web.csproj \
  --context AppDbContext \
  --output-dir Migrations
```

---

## 5) Apply the migration to the database

```bash
dotnet ef database update \
  --project EmployeeManagement.Api/EmployeeManagement.Api.csproj \
  --startup-project EmployeeManagement.Web/EmployeeManagement.Web.csproj \
  --context AppDbContext
```

This creates the `EmployeeDb` database and all tables.

---

## 6) Start CloudBeaver

```bash
docker run --name cloudbeaver -p 127.0.0.1:8978:8978 -d dbeaver/cloudbeaver:latest
```

If it is already created but stopped:

```bash
docker start cloudbeaver
```

Check status:

```bash
docker ps --filter name=cloudbeaver
```

Open in browser:

```text
http://localhost:8978
```

---

## 7) Connect CloudBeaver to SQL Server

Use:

- Host: `host.docker.internal`
- Port: `1433`
- Database: `EmployeeDb`
- Username: `sa`
- Password: `YourStrongPassword123!`

---

## 8) Run the web app

```bash
dotnet run --project EmployeeManagement.Web/EmployeeManagement.Web.csproj
```

---

## Useful restart commands

```bash
docker restart employee-sqlserver
docker restart cloudbeaver
```

---

## Useful cleanup commands

```bash
docker rm -f employee-sqlserver
docker rm -f cloudbeaver
```
