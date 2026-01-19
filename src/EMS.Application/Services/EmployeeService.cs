using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Repositories;
using EMS.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EMS.Application.Services;

/// <summary>
/// Service implementation for Employee operations
/// </summary>
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all employees");
            return await _employeeRepository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all employees");
            throw;
        }
    }

    public async Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving employee with ID: {EmployeeId}", id);
            return await _employeeRepository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee with ID: {EmployeeId}", id);
            throw;
        }
    }

    public async Task<Employee> CreateAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new employee: {EmployeeName}", employee.Name);
            employee.CreatedDate = DateTime.UtcNow;
            employee.IsActive = true;
            return await _employeeRepository.AddAsync(employee, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee: {EmployeeName}", employee.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, Employee employee, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating employee with ID: {EmployeeId}", id);
            var existingEmployee = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            if (existingEmployee == null)
            {
                throw new InvalidOperationException($"Employee with ID {id} not found");
            }

            employee.Id = id;
            employee.ModifiedDate = DateTime.UtcNow;
            await _employeeRepository.UpdateAsync(employee, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee with ID: {EmployeeId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting employee with ID: {EmployeeId}", id);
            await _employeeRepository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee with ID: {EmployeeId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Employee>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching employees with term: {SearchTerm}", searchTerm);
            return await _employeeRepository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching employees with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving employees for department: {DepartmentId}", departmentId);
            return await _employeeRepository.GetByDepartmentAsync(departmentId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees for department: {DepartmentId}", departmentId);
            throw;
        }
    }
}
