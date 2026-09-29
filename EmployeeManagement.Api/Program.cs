using EmployeeManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Api;

internal static class ApiProgram
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("WebAssemblyClient", policy =>
                policy.WithOrigins("http://localhost:5015")
                    .AllowAnyHeader()
                    .AllowAnyMethod());

// When the browser calls the API, it includes its origin (http://localhost:5015) 
// in the request. The API’s CORS middleware compares that origin against the allowed 
// origin in Program.cs. If you change the client’s port, update the allowed origin to 
// match. In production, replace it with the deployed client’s URL.

        });
        builder.Services.AddControllers();
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDbConnection")));
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

        var app = builder.Build();

        app.UseCors("WebAssemblyClient");
        app.UseStaticFiles();
        app.MapControllers();

        app.Run();
    }
}