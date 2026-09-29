
using EmployeeManagement.Models;
using System.Collections.Generic;
public interface IEmployeeRepository
    {
        Task<Employee> AddEmployee(Employee employee);
        Task<Employee> GetEmployee(int employeeId);
        Task<IEnumerable<Employee>> GetEmployees();
        Task<Employee> UpdateEmployee(Employee employee);
        Task<Employee> DeleteEmployee(int employeeId);
        Task<Employee> GetEmployeeByEmail(string email);
    }