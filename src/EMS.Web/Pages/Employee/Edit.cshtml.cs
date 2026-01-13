using EMS.Application.DTOs;
using EMS.Application.Interfaces;
using EMS.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMS.Web.Pages.Employee;

[Authorize]
public class EditModel : PageModel
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(
        IEmployeeService employeeService,
        IDepartmentService departmentService,
        ILogger<EditModel> logger)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
        _logger = logger;
    }

    [BindProperty]
    public EmployeeViewModel Employee { get; set; } = new();

    public List<SelectListItem> DepartmentList { get; set; } = new();
    public List<SelectListItem> ManagerList { get; set; } = new();

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
            await LoadDropdownsAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading employee for edit");
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync();
            return Page();
        }

        try
        {
            var updateDto = new EmployeeUpdateDto
            {
                Name = Employee.Name,
                Email = Employee.Email,
                DepartmentNo = Employee.DepartmentNo,
                Gender = Employee.Gender,
                DateOfBirth = Employee.DateOfBirth,
                DateOfJoining = Employee.DateOfJoining,
                ReportingTo = Employee.ReportingTo,
                Phone = Employee.Phone,
                Salary = Employee.Salary,
                Commission = Employee.Commission,
                JobTitle = Employee.JobTitle
            };

            await _employeeService.UpdateAsync(Employee.Number, updateDto);
            _logger.LogInformation("Employee updated successfully");
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee");
            ModelState.AddModelError(string.Empty, "An error occurred while updating the employee");
            await LoadDropdownsAsync();
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

    private async Task LoadDropdownsAsync()
    {
        var departments = await _departmentService.GetAllAsync();
        DepartmentList = departments.Select(d => new SelectListItem
        {
            Value = d.DepartmentId.ToString(),
            Text = d.Name
        }).ToList();

        var employees = await _employeeService.GetAllAsync();
        ManagerList = employees
            .Where(e => e.Number != Employee.Number)
            .Select(e => new SelectListItem
            {
                Value = e.Number.ToString(),
                Text = e.Name
            }).ToList();
    }
}
