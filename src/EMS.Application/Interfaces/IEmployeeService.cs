using EMS.Application.DTOs;

namespace EMS.Application.Interfaces;

/// <summary>
/// Service interface for Employee operations
/// </summary>
public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<EmployeeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<EmployeeDto> CreateAsync(EmployeeCreateDto dto, CancellationToken cancellationToken = default);
    Task<EmployeeDto> UpdateAsync(int id, EmployeeUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeeDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
}
