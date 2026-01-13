using EMS.Domain.Entities;

namespace EMS.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Employee business logic operations
/// </summary>
public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
    Task<Employee> CreateAsync(Employee employee, CancellationToken cancellationToken = default);
    Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Employee>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<Employee>> GetManagersAsync(CancellationToken cancellationToken = default);
}
