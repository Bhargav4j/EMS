using EMS.Domain.Entities;

namespace EMS.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Department business operations
/// </summary>
public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default);
    Task<Department?> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Department> CreateDepartmentAsync(Department department, CancellationToken cancellationToken = default);
    Task<Department> UpdateDepartmentAsync(Department department, CancellationToken cancellationToken = default);
    Task<bool> DeleteDepartmentAsync(int id, CancellationToken cancellationToken = default);
}
