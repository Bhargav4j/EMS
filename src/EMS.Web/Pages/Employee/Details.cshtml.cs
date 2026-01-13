using EMS.Application.DTOs;
using EMS.Application.Interfaces;
using EMS.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EMS.Web.Pages.Employee;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IEmployeeService employeeService, ILogger<DetailsModel> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    public EmployeeViewModel? Employee { get; set; }

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
            _logger.LogError(ex, "Error loading employee details");
            return RedirectToPage("Index");
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
