using EMS.Application.DTOs;
using EMS.Application.Interfaces;
using EMS.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EMS.Web.Pages.Employee;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IEmployeeService employeeService, ILogger<DeleteModel> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    [BindProperty]
    public EmployeeViewModel Employee { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var dto = await _employeeService.GetByIdAsync(id.Value);
            if (dto == null)
            {
                return NotFound();
            }

            Employee = MapToViewModel(dto);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading employee for deletion");
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _employeeService.DeleteAsync(Employee.Number);
            _logger.LogInformation("Employee deleted successfully");
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee");
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the employee");
            return Page();
        }
    }

    private EmployeeViewModel MapToViewModel(EmployeeDto dto)
    {
        return new EmployeeViewModel
        {
            Number = dto.Number,
            Name = dto.Name,
            Email = dto.Email,
            DepartmentNo = dto.DepartmentNo,
            DepartmentName = dto.DepartmentName,
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            DateOfJoining = dto.DateOfJoining,
            ReportingTo = dto.ReportingTo,
            ManagerName = dto.ManagerName,
            Phone = dto.Phone,
            Salary = dto.Salary,
            Commission = dto.Commission,
            JobTitle = dto.JobTitle
        };
    }
}
