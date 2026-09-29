using EmployeeManagement.Models;


public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetEmployees();
    Task<Employee> GetEmployee(int id);
    Task DeleteEmployee(int id);
    Task<Employee> UpdateEmployee(Employee employee);
}