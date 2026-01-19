using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EMS.Web.Pages.Employees;

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
            Employee = await _employeeService.GetByIdAsync(id.Value);

            if (Employee == null)
            {
                return NotFound();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading employee for deletion with ID: {EmployeeId}", id);
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Employee?.Id == null)
        {
            return NotFound();
        }

        try
        {
            await _employeeService.DeleteAsync(Employee.Id);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee with ID: {EmployeeId}", Employee.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the employee.");
            return Page();
        }
    }
}
