using EMS.Application.DTOs;
using EMS.Application.Interfaces;
using EMS.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMS.Web.Pages.Employee;

[Authorize]
public class CreateModel : PageModel
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(
        IEmployeeService employeeService,
        IDepartmentService departmentService,
        ILogger<CreateModel> logger)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
        _logger = logger;
    }

    [BindProperty]
    public EmployeeViewModel Employee { get; set; } = new();

    public List<SelectListItem> DepartmentList { get; set; } = new();
    public List<SelectListItem> ManagerList { get; set; } = new();

    public async Task OnGetAsync()
    {
        await LoadDropdownsAsync();
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
            var createDto = new EmployeeCreateDto
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

            await _employeeService.CreateAsync(createDto);
            _logger.LogInformation("Employee created successfully");
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the employee");
            await LoadDropdownsAsync();
            return Page();
        }
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
        ManagerList = employees.Select(e => new SelectListItem
        {
            Value = e.Number.ToString(),
            Text = e.Name
        }).ToList();
    }
}
