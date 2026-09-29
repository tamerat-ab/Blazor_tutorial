using EmployeeManagement.Models;
using EmployeeManagement.Web.Services;
using Microsoft.AspNetCore.Components;

namespace EmployeeManagement.Web.Components.Pages;

public partial class EmployeeDetail : ComponentBase
{
    [Inject]
    public IEmployeeService EmployeeService { get; set; } = default!;

    [Inject]
    public IConfiguration Configuration { get; set; } = default!;

    [Parameter]
    public int Id { get; set; }

    public Employee? Employee { get; set; }
    public bool IsLoading { get; private set; } = true;
    public string? ErrorMessage { get; private set; }

    protected override async Task OnParametersSetAsync()
    {
        IsLoading = true;
        ErrorMessage = null;
        Employee = null;

        try
        {
            Employee = await EmployeeService.GetEmployee(Id);
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Could not reach the API. Confirm it is running at http://localhost:5168.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private string ImageUrl(string photoPath) =>
        $"{Configuration["ApiBaseUrl"]?.TrimEnd('/')}/{photoPath.TrimStart('/')}";
}