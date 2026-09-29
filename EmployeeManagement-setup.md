# EmployeeManagement Solution Setup

Run these commands from the `NetPrograms` folder:

```bash
cd /Users/tameratmcexternal/NetPrograms
```

## Create the solution

```bash
dotnet new sln -n EmployeeManagement
```

Add the existing web project:

```bash
dotnet sln EmployeeManagement.slnx add EmployeeManagement.Web/EmployeeManagement.Web.csproj
```

## Create additional projects

```bash
dotnet new classlib -n EmployeeManagement.Domain
dotnet new classlib -n EmployeeManagement.Infrastructure
dotnet new xunit -n EmployeeManagement.Tests
```

## Add projects to the solution

```bash
dotnet sln EmployeeManagement.slnx add EmployeeManagement.Domain/EmployeeManagement.Domain.csproj
dotnet sln EmployeeManagement.slnx add EmployeeManagement.Infrastructure/EmployeeManagement.Infrastructure.csproj
dotnet sln EmployeeManagement.slnx add EmployeeManagement.Tests/EmployeeManagement.Tests.csproj
```

## Add project references

```bash
dotnet add EmployeeManagement.Web/EmployeeManagement.Web.csproj reference EmployeeManagement.Domain/EmployeeManagement.Domain.csproj

dotnet add EmployeeManagement.Infrastructure/EmployeeManagement.Infrastructure.csproj reference EmployeeManagement.Domain/EmployeeManagement.Domain.csproj

dotnet add EmployeeManagement.Tests/EmployeeManagement.Tests.csproj reference EmployeeManagement.Domain/EmployeeManagement.Domain.csproj
```

## Build and test

```bash
dotnet build EmployeeManagement.slnx
dotnet test EmployeeManagement.slnx
```

## Expected structure

```text
EmployeeManagement.slnx
EmployeeManagement.Web/
EmployeeManagement.Domain/
EmployeeManagement.Infrastructure/
EmployeeManagement.Tests/
```
