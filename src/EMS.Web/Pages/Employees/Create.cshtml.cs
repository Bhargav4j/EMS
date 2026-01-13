using System.ComponentModel.DataAnnotations;
using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMS.Web.Pages.Employees;

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
    public InputModel Input { get; set; } = new();

    public SelectList Departments { get; set; } = new(new List<Department>(), "Id", "Name");
    public SelectList Managers { get; set; } = new(new List<Employee>(), "Id", "Name");

    public class InputModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfJoining { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(100)]
        public string JobTitle { get; set; } = string.Empty;

        public int? ReportingTo { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Commission { get; set; }
    }

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
            var employee = new Employee
            {
                Name = Input.Name,
                Email = Input.Email,
                Phone = Input.Phone,
                Gender = Input.Gender,
                DateOfBirth = Input.DateOfBirth,
                DateOfJoining = Input.DateOfJoining,
                DepartmentId = Input.DepartmentId,
                JobTitle = Input.JobTitle,
                ReportingTo = Input.ReportingTo,
                Salary = Input.Salary,
                Commission = Input.Commission
            };

            await _employeeService.CreateAsync(employee);

            TempData["SuccessMessage"] = "Employee created successfully.";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the employee.");
            await LoadDropdownsAsync();
            return Page();
        }
    }

    private async Task LoadDropdownsAsync()
    {
        var departments = await _departmentService.GetAllAsync();
        var managers = await _employeeService.GetManagersAsync();

        Departments = new SelectList(departments, "Id", "Name");
        Managers = new SelectList(managers, "Id", "Name");
    }
}
