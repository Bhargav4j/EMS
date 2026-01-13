using EMS.Application.DTOs;

namespace EMS.Application.Interfaces;

/// <summary>
/// Service interface for Department operations
/// </summary>
public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DepartmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
