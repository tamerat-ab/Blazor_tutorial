
using EmployeeManagement.Models;
using EmployeeManagement.Api.Models;
public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;

    public DepartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Department> GetDepartments()
    {
        return _context.Departments.ToList();
    }

    public Department GetDepartment(int departmentId)
    {
        return _context.Departments.Find(departmentId)
            ?? throw new KeyNotFoundException($"Department with ID {departmentId} was not found.");
    }
}