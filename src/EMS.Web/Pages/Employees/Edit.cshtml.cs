using System.ComponentModel.DataAnnotations;
using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMS.Web.Pages.Employees;

public class EditModel : PageModel
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IEmployeeService employeeService, IDepartmentService departmentService, ILogger<EditModel> logger)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public SelectList Departments { get; set; } = new SelectList(new List<Department>());
    public SelectList Managers { get; set; } = new SelectList(new List<Employee>());

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var employee = await _employeeService.GetByIdAsync(id.Value);

            if (employee == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                JobTitle = employee.JobTitle,
                Salary = employee.Salary,
                Commission = employee.Commission,
                DateOfBirth = employee.DateOfBirth,
                DateOfJoining = employee.DateOfJoining,
                Phone = employee.Phone,
                Gender = employee.Gender,
                ReportingTo = employee.ReportingTo
            };

            await LoadDropdownsAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading employee for edit with ID: {EmployeeId}", id);
            return RedirectToPage("./Index");
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
            var employee = new Employee
            {
                Id = Input.Id,
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
                ModifiedBy = "System"
            };

            await _employeeService.UpdateAsync(Input.Id, employee);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee with ID: {EmployeeId}", Input.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the employee.");
            await LoadDropdownsAsync();
            return Page();
        }
    }

    private async Task LoadDropdownsAsync()
    {
        var departments = await _departmentService.GetAllAsync();
        Departments = new SelectList(departments, "Id", "Name");

        var employees = await _employeeService.GetAllAsync();
        Managers = new SelectList(employees.Where(e => e.Id != Input.Id), "Id", "Name");
    }

    public class InputModel
    {
        public int Id { get; set; }

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
        public DateTime DateOfJoining { get; set; }

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
