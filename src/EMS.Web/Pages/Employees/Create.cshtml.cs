using System.ComponentModel.DataAnnotations;
using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMS.Web.Pages.Employees;

public class CreateModel : PageModel
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IEmployeeService employeeService, IDepartmentService departmentService, ILogger<CreateModel> logger)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public SelectList Departments { get; set; } = new SelectList(new List<Department>());
    public SelectList Managers { get; set; } = new SelectList(new List<Employee>());

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
                DepartmentId = Input.DepartmentId,
                JobTitle = Input.JobTitle,
                Salary = Input.Salary,
                Commission = Input.Commission,
                DateOfBirth = Input.DateOfBirth,
                DateOfJoining = Input.DateOfJoining,
                Phone = Input.Phone,
                Gender = Input.Gender,
                ReportingTo = Input.ReportingTo,
                CreatedBy = "System"
            };

            await _employeeService.CreateAsync(employee);
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
        var departments = await _departmentService.GetAllAsync();
        Departments = new SelectList(departments, "Id", "Name");

        var employees = await _employeeService.GetAllAsync();
        Managers = new SelectList(employees, "Id", "Name");
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
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [Range(0, 9999999)]
        public decimal Salary { get; set; }

        [Range(0, 9999999)]
        public decimal Commission { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Date of Joining")]
        public DateTime DateOfJoining { get; set; } = DateTime.Today;

        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        [Display(Name = "Manager")]
        public int? ReportingTo { get; set; }
    }
}
