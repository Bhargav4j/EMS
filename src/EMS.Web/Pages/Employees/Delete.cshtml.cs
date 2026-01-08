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

    public DeleteModel(IEmployeeService employeeService, ILogger<DeleteModel> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    [BindProperty]
    public Employee? Employee { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            Employee = await _employeeService.GetEmployeeByIdAsync(id.Value);

            if (Employee == null)
            {
                return NotFound();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading employee for delete, ID: {Id}", id);
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Employee == null || Employee.Number == 0)
        {
            return NotFound();
        }

        try
        {
            await _employeeService.DeleteEmployeeAsync(Employee.Number);
            TempData["SuccessMessage"] = "Employee deleted successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee");
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the employee.");
            return Page();
        }
    }
}
