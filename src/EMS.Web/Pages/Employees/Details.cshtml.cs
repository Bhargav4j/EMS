using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EMS.Web.Pages.Employees;

public class DetailsModel : PageModel
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IEmployeeService employeeService, ILogger<DetailsModel> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

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
            _logger.LogError(ex, "Error loading employee details for ID: {EmployeeId}", id);
            return RedirectToPage("./Index");
        }
    }
}
