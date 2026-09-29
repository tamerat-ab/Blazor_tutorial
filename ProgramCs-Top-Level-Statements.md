# Why Program.cs Does Not Show Main

In modern C#, `Program.cs` can use **top-level statements**.

The compiler automatically generates a `Program` class and a `Main` method around the code. This means:

```csharp
var builder = WebApplication.CreateBuilder(args);
```

is conceptually similar to:

```csharp
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
    }
}
```

In an ASP.NET Core application, `Program.cs`:

- Registers services with `builder.Services`.
- Builds the application with `builder.Build()`.
- Configures middleware with `app.Use...`.
- Configures routes and endpoints with `app.Map...`.
- Starts the web server with `app.Run()`.

`app.Run()` starts the host, but the compiler-generated `Main` method is the actual entry point.

`Routes.razor` can use `typeof(Program).Assembly` because the compiler-generated `Program` class still exists.
