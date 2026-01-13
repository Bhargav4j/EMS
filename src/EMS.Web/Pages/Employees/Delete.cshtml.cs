using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EMS.Web.Pages.Employees;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(
        IEmployeeService employeeService,
        ILogger<DeleteModel> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    public Employee? Employee { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Employee = await _employeeService.GetByIdAsync(id);

            if (Employee == null)
            {
                return NotFound();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading employee for delete with ID {EmployeeId}", id);
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            await _employeeService.DeleteAsync(id);

            TempData["SuccessMessage"] = "Employee deleted successfully.";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee with ID {EmployeeId}", id);
            TempData["ErrorMessage"] = "An error occurred while deleting the employee.";
            return RedirectToPage("Index");
        }
    }
}
