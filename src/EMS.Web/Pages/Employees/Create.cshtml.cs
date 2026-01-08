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
    public InputModel Input { get; set; } = new InputModel();

    public SelectList Departments { get; set; } = new SelectList(Enumerable.Empty<Department>());
    public SelectList Managers { get; set; } = new SelectList(Enumerable.Empty<Employee>());

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDropdownsAsync();
        return Page();
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
                DepartmentNo = Input.DepartmentNo,
                JobTitle = Input.JobTitle,
                Salary = Input.Salary,
                Commission = Input.Commission,
                DateOfBirth = Input.DateOfBirth,
                DateOfJoining = Input.DateOfJoining,
                ReportingTo = Input.ReportingTo,
                Phone = Input.Phone,
                Gender = Input.Gender,
                CreatedBy = User.Identity?.Name ?? "System"
            };

            await _employeeService.CreateEmployeeAsync(employee);

            TempData["SuccessMessage"] = "Employee created successfully.";
            return RedirectToPage("./Index");
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
        var departments = await _departmentService.GetAllDepartmentsAsync();
        Departments = new SelectList(departments, nameof(Department.DepartmentId), nameof(Department.Name));

        var employees = await _employeeService.GetAllEmployeesAsync();
        Managers = new SelectList(employees, nameof(Employee.Number), nameof(Employee.Name));
    }

    public class InputModel
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Department")]
        public int DepartmentNo { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [Range(0, 999999999.99)]
        public decimal Salary { get; set; }

        [Range(0, 999999999.99)]
        public decimal Commission { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Date of Joining")]
        [DataType(DataType.Date)]
        public DateTime DateOfJoining { get; set; }

        [Display(Name = "Manager")]
        public int? ReportingTo { get; set; }

        [Required]
        [Phone]
        [StringLength(50)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;
    }
}
