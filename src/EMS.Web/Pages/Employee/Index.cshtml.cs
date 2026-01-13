using EMS.Application.DTOs;
using EMS.Application.Interfaces;
using EMS.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EMS.Web.Pages.Employee;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IEmployeeService employeeService, ILogger<IndexModel> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    public IEnumerable<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();

    public async Task OnGetAsync()
    {
        try
        {
            var employeeDtos = await _employeeService.GetAllAsync();
            Employees = MapToViewModels(employeeDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading employees");
        }
    }

    private IEnumerable<EmployeeViewModel> MapToViewModels(IEnumerable<EmployeeDto> dtos)
    {
        return dtos.Select(dto => new EmployeeViewModel
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
        });
    }
}
