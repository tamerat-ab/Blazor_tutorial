
using EmployeeManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Json;
using EmployeeManagement.Web.Services;
namespace EmployeeManagement.Web.Services

{
   public class EmployeeService : IEmployeeService
{
    private readonly HttpClient httpClient;

    public EmployeeService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Employee> GetEmployee(int id)
    {
        return await httpClient.GetFromJsonAsync<Employee>($"api/employees/{id}");
    }

    public async Task<IEnumerable<Employee>> GetEmployees()
    {
        return await httpClient.GetFromJsonAsync<Employee[]>("api/employees");
    }

    public async Task DeleteEmployee(int id)
    {
        using var response = await httpClient.DeleteAsync($"api/employees/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<Employee> UpdateEmployee(Employee employee)
    {
        using var response = await httpClient.PutAsJsonAsync(
            $"api/employees/{employee.EmployeeId}", employee);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Employee>()
            ?? throw new InvalidOperationException("The API returned no employee.");
    }
}

}