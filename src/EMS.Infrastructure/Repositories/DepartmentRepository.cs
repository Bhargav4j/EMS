using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Repositories;
using EMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMS.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Department entity
/// </summary>
public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DepartmentRepository> _logger;

    public DepartmentRepository(
        ApplicationDbContext context,
        ILogger<DepartmentRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Departments
                .Where(d => d.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all departments from database");
            throw;
        }
    }

    public async Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DepartmentId == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving department {Id} from database", id);
            throw;
        }
    }

    public async Task<Department> AddAsync(Department department, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Departments.AddAsync(department, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return department;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding department to database");
            throw;
        }
    }

    public async Task<Department> UpdateAsync(Department department, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync(cancellationToken);
            return department;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating department {Id} in database", department.DepartmentId);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var department = await _context.Departments.FindAsync(new object[] { id }, cancellationToken);
            if (department == null)
            {
                return false;
            }

            department.IsActive = false;
            department.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting department {Id} from database", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Departments.AnyAsync(d => d.DepartmentId == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if department {Id} exists", id);
            throw;
        }
    }
}
