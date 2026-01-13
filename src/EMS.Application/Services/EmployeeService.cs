using AutoMapper;
using EMS.Application.DTOs;
using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Repositories;
using EMS.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace EMS.Application.Services;

/// <summary>
/// Service implementation for Employee operations
/// </summary>
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IMapper mapper,
        ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all employees");
            var employees = await _employeeRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all employees");
            throw;
        }
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting employee with ID: {Id}", id);
            var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            return employee != null ? _mapper.Map<EmployeeDto>(employee) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employee with ID: {Id}", id);
            throw;
        }
    }

    public async Task<EmployeeDto> CreateAsync(EmployeeCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new employee: {Name}", dto.Name);
            var employee = _mapper.Map<Employee>(dto);
            employee.CreatedDate = DateTime.UtcNow;
            employee.IsActive = true;
            employee.CreatedBy = "System";

            var created = await _employeeRepository.AddAsync(employee, cancellationToken);
            _logger.LogInformation("Employee created with ID: {Id}", created.Number);
            return _mapper.Map<EmployeeDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee: {Name}", dto.Name);
            throw;
        }
    }

    public async Task<EmployeeDto> UpdateAsync(int id, EmployeeUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating employee with ID: {Id}", id);
            var existing = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new InvalidOperationException($"Employee with ID {id} not found");
            }

            _mapper.Map(dto, existing);
            existing.ModifiedDate = DateTime.UtcNow;
            existing.ModifiedBy = "System";

            var updated = await _employeeRepository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("Employee updated with ID: {Id}", id);
            return _mapper.Map<EmployeeDto>(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee with ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting employee with ID: {Id}", id);
            var result = await _employeeRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Employee deleted with ID: {Id}", id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<EmployeeDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching employees with term: {SearchTerm}", searchTerm);
            var employees = await _employeeRepository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching employees with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting employees by department: {DepartmentId}", departmentId);
            var employees = await _employeeRepository.GetByDepartmentAsync(departmentId, cancellationToken);
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employees by department: {DepartmentId}", departmentId);
            throw;
        }
    }
}
