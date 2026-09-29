using EmployeeManagement.Models;
using EmployeeManagement.Web.Services;
using Microsoft.AspNetCore.Components;

namespace EmployeeManagement.Web.Components.Pages;

public partial class EmployeeList : ComponentBase
{
    [Inject]
    public IEmployeeService EmployeeService { get; set; } = default!;

    [Inject]
    public IConfiguration Configuration { get; set; } = default!;

    public List<Employee>? Employees { get; set; }
    public bool IsLoading { get; private set; } = true;
    public string? ErrorMessage { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Employees = (await EmployeeService.GetEmployees()).ToList();
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

    private async Task DeleteEmployee(Employee employee)
    {
        try
        {
            await EmployeeService.DeleteEmployee(employee.EmployeeId);
            Employees?.Remove(employee);
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "The employee could not be deleted.";
        }
    }
}