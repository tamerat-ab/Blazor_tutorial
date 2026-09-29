
using EmployeeManagement.Models;
public interface IDepartmentRepository
{
    IEnumerable<Department> GetDepartments();
    Department GetDepartment (int departmentId);
}