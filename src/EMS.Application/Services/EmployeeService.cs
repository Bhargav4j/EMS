using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Repositories;
using EMS.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EMS.Application.Services;

/// <summary>
/// Service implementation for Employee business operations
/// </summary>
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all employees");
            var employees = await _employeeRepository.GetAllAsync(cancellationToken);
            _logger.LogInformation("Retrieved {Count} employees", employees.Count());
            return employees;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all employees");
            throw;
        }
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving employee with ID: {Id}", id);
            var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);

            if (employee == null)
            {
                _logger.LogWarning("Employee with ID {Id} not found", id);
            }

            return employee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee with ID: {Id}", id);
            throw;
        }
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new employee: {Name}", employee.Name);

            employee.CreatedDate = DateTime.UtcNow;
            employee.IsActive = true;

            var createdEmployee = await _employeeRepository.AddAsync(employee, cancellationToken);
            _logger.LogInformation("Successfully created employee with ID: {Id}", createdEmployee.Number);

            return createdEmployee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee: {Name}", employee.Name);
            throw;
        }
    }

    public async Task<Employee> UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating employee with ID: {Id}", employee.Number);

            var existingEmployee = await _employeeRepository.GetByIdAsync(employee.Number, cancellationToken);
            if (existingEmployee == null)
            {
                _logger.LogWarning("Employee with ID {Id} not found for update", employee.Number);
                throw new InvalidOperationException($"Employee with ID {employee.Number} not found");
            }

            employee.ModifiedDate = DateTime.UtcNow;

            var updatedEmployee = await _employeeRepository.UpdateAsync(employee, cancellationToken);
            _logger.LogInformation("Successfully updated employee with ID: {Id}", employee.Number);

            return updatedEmployee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee with ID: {Id}", employee.Number);
            throw;
        }
    }

    public async Task<bool> DeleteEmployeeAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting employee with ID: {Id}", id);

            var exists = await _employeeRepository.ExistsAsync(id, cancellationToken);
            if (!exists)
            {
                _logger.LogWarning("Employee with ID {Id} not found for deletion", id);
                return false;
            }

            var result = await _employeeRepository.DeleteAsync(id, cancellationToken);

            if (result)
            {
                _logger.LogInformation("Successfully deleted employee with ID: {Id}", id);
            }
            else
            {
                _logger.LogWarning("Failed to delete employee with ID: {Id}", id);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching employees with term: {SearchTerm}", searchTerm);
            var employees = await _employeeRepository.SearchAsync(searchTerm, cancellationToken);
            _logger.LogInformation("Found {Count} employees matching search term", employees.Count());
            return employees;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching employees with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving employees for department ID: {DepartmentId}", departmentId);
            var employees = await _employeeRepository.GetByDepartmentAsync(departmentId, cancellationToken);
            _logger.LogInformation("Retrieved {Count} employees for department", employees.Count());
            return employees;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees for department ID: {DepartmentId}", departmentId);
            throw;
        }
    }
}
