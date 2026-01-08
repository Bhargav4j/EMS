using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Repositories;
using EMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMS.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Employee entity
/// </summary>
public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EmployeeRepository> _logger;

    public EmployeeRepository(
        ApplicationDbContext context,
        ILogger<EmployeeRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Manager)
                .Where(e => e.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all employees from database");
            throw;
        }
    }

    public async Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Manager)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Number == id && e.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee {Id} from database", id);
            throw;
        }
    }

    public async Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Employees.AddAsync(employee, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return employee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding employee to database");
            throw;
        }
    }

    public async Task<Employee> UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync(cancellationToken);
            return employee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee {Id} in database", employee.Number);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var employee = await _context.Employees.FindAsync(new object[] { id }, cancellationToken);
            if (employee == null)
            {
                return false;
            }

            employee.IsActive = false;
            employee.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee {Id} from database", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Employees.AnyAsync(e => e.Number == id && e.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if employee {Id} exists", id);
            throw;
        }
    }

    public async Task<IEnumerable<Employee>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync(cancellationToken);
            }

            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Manager)
                .Where(e => e.IsActive &&
                    (e.Name.Contains(searchTerm) ||
                     e.Email.Contains(searchTerm) ||
                     e.JobTitle.Contains(searchTerm)))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
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
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Manager)
                .Where(e => e.DepartmentNo == departmentId && e.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees for department {DepartmentId}", departmentId);
            throw;
        }
    }
}
