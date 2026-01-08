using EMS.Domain.Entities;

namespace EMS.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Employee business operations
/// </summary>
public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken = default);
    Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Employee> CreateEmployeeAsync(Employee employee, CancellationToken cancellationToken = default);
    Task<Employee> UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken = default);
    Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
}
