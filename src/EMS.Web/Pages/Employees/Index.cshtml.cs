using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EMS.Web.Pages.Employees;

public class IndexModel : PageModel
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IEmployeeService employeeService, ILogger<IndexModel> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();

    public async Task OnGetAsync()
    {
        try
        {
            Employees = await _employeeService.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading employees");
            Employees = new List<Employee>();
        }
    }
}
