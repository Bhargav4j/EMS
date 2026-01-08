using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Repositories;
using EMS.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EMS.Application.Services;

/// <summary>
/// Service implementation for Department business operations
/// </summary>
public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(
        IDepartmentRepository departmentRepository,
        ILogger<DepartmentService> logger)
    {
        _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Department>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all departments");
            var departments = await _departmentRepository.GetAllAsync(cancellationToken);
            _logger.LogInformation("Retrieved {Count} departments", departments.Count());
            return departments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all departments");
            throw;
        }
    }

    public async Task<Department?> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving department with ID: {Id}", id);
            var department = await _departmentRepository.GetByIdAsync(id, cancellationToken);

            if (department == null)
            {
                _logger.LogWarning("Department with ID {Id} not found", id);
            }

            return department;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving department with ID: {Id}", id);
            throw;
        }
    }

    public async Task<Department> CreateDepartmentAsync(Department department, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new department: {Name}", department.Name);

            department.CreatedDate = DateTime.UtcNow;
            department.IsActive = true;

            var createdDepartment = await _departmentRepository.AddAsync(department, cancellationToken);
            _logger.LogInformation("Successfully created department with ID: {Id}", createdDepartment.DepartmentId);

            return createdDepartment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating department: {Name}", department.Name);
            throw;
        }
    }

    public async Task<Department> UpdateDepartmentAsync(Department department, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating department with ID: {Id}", department.DepartmentId);

            var existingDepartment = await _departmentRepository.GetByIdAsync(department.DepartmentId, cancellationToken);
            if (existingDepartment == null)
            {
                _logger.LogWarning("Department with ID {Id} not found for update", department.DepartmentId);
                throw new InvalidOperationException($"Department with ID {department.DepartmentId} not found");
            }

            department.ModifiedDate = DateTime.UtcNow;

            var updatedDepartment = await _departmentRepository.UpdateAsync(department, cancellationToken);
            _logger.LogInformation("Successfully updated department with ID: {Id}", department.DepartmentId);

            return updatedDepartment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating department with ID: {Id}", department.DepartmentId);
            throw;
        }
    }

    public async Task<bool> DeleteDepartmentAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting department with ID: {Id}", id);

            var exists = await _departmentRepository.ExistsAsync(id, cancellationToken);
            if (!exists)
            {
                _logger.LogWarning("Department with ID {Id} not found for deletion", id);
                return false;
            }

            var result = await _departmentRepository.DeleteAsync(id, cancellationToken);

            if (result)
            {
                _logger.LogInformation("Successfully deleted department with ID: {Id}", id);
            }
            else
            {
                _logger.LogWarning("Failed to delete department with ID: {Id}", id);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting department with ID: {Id}", id);
            throw;
        }
    }
}
