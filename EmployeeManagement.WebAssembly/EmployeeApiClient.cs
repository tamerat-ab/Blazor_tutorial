using System.Net.Http.Json;
using EmployeeManagement.Models;

public sealed class EmployeeApiClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<Employee>> GetEmployeesAsync()
    {
        return await httpClient.GetFromJsonAsync<List<Employee>>("api/employees")
            ?? [];
    }

    public async Task<Employee?> GetEmployeeAsync(int id)
    {
        using var response = await httpClient.GetAsync($"api/employees/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Employee>();
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        using var response = await httpClient.DeleteAsync($"api/employees/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<Employee> UpdateEmployeeAsync(Employee employee)
    {
        using var response = await httpClient.PutAsJsonAsync(
            $"api/employees/{employee.EmployeeId}", employee);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Employee>()
            ?? throw new InvalidOperationException("The API returned no employee.");
    }
}