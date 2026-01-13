using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EMS.Web.Pages.Departments;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(
        IDepartmentService departmentService,
        ILogger<IndexModel> logger)
    {
        _departmentService = departmentService;
        _logger = logger;
    }

    public IEnumerable<Department> Departments { get; set; } = new List<Department>();

    public async Task OnGetAsync()
    {
        try
        {
            Departments = await _departmentService.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving departments");
            Departments = new List<Department>();
        }
    }
}
