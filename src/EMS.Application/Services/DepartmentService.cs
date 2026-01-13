using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Repositories;
using EMS.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EMS.Application.Services;

/// <summary>
/// Service implementation for Department business logic
/// </summary>
public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(
        IDepartmentRepository departmentRepository,
        ILogger<DepartmentService> logger)
    {
        _departmentRepository = departmentRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all departments");
            return await _departmentRepository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all departments");
            throw;
        }
    }

    public async Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving department with ID {DepartmentId}", id);
            return await _departmentRepository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving department with ID {DepartmentId}", id);
            throw;
        }
    }

    public async Task<Department> CreateAsync(Department department, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new department {DepartmentName}", department.Name);

            department.CreatedDate = DateTime.UtcNow;
            department.IsActive = true;
            department.CreatedBy = "System";

            var result = await _departmentRepository.AddAsync(department, cancellationToken);
            _logger.LogInformation("Department created successfully with ID {DepartmentId}", result.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating department {DepartmentName}", department.Name);
            throw;
        }
    }

    public async Task UpdateAsync(Department department, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating department with ID {DepartmentId}", department.Id);

            department.ModifiedDate = DateTime.UtcNow;
            department.ModifiedBy = "System";

            await _departmentRepository.UpdateAsync(department, cancellationToken);
            _logger.LogInformation("Department updated successfully with ID {DepartmentId}", department.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating department with ID {DepartmentId}", department.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting department with ID {DepartmentId}", id);
            await _departmentRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Department deleted successfully with ID {DepartmentId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting department with ID {DepartmentId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Department>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching departments with term {SearchTerm}", searchTerm);
            return await _departmentRepository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching departments with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}
