using EmployeeManagement.Web.Components;
using EmployeeManagement.Api.Models;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Web.Services;  

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
    builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("EmployeeDbConnection")));
    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();


    builder.Services.AddHttpClient<IEmployeeService, EmployeeService>(client =>
    {
        var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
            ?? throw new InvalidOperationException("Configuration value 'ApiBaseUrl' is required.");
        client.BaseAddress = new Uri(apiBaseUrl);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
