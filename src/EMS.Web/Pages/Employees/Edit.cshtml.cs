using System.ComponentModel.DataAnnotations;
using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMS.Web.Pages.Employees;

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
    public InputModel Input { get; set; } = new InputModel();

    public SelectList Departments { get; set; } = new SelectList(Enumerable.Empty<Department>());
    public SelectList Managers { get; set; } = new SelectList(Enumerable.Empty<Employee>());

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);

            if (employee == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                Number = employee.Number,
                Name = employee.Name,
                Email = employee.Email,
                DepartmentNo = employee.DepartmentNo,
                JobTitle = employee.JobTitle,
                Salary = employee.Salary,
                Commission = employee.Commission,
                DateOfBirth = employee.DateOfBirth,
                DateOfJoining = employee.DateOfJoining,
                ReportingTo = employee.ReportingTo,
                Phone = employee.Phone,
                Gender = employee.Gender
            };

            await LoadDropdownsAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading employee for edit, ID: {Id}", id);
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
            var employee = await _employeeService.GetEmployeeByIdAsync(Input.Number);

            if (employee == null)
            {
                return NotFound();
            }

            employee.Name = Input.Name;
            employee.Email = Input.Email;
            employee.DepartmentNo = Input.DepartmentNo;
            employee.JobTitle = Input.JobTitle;
            employee.Salary = Input.Salary;
            employee.Commission = Input.Commission;
            employee.DateOfBirth = Input.DateOfBirth;
            employee.DateOfJoining = Input.DateOfJoining;
            employee.ReportingTo = Input.ReportingTo;
            employee.Phone = Input.Phone;
            employee.Gender = Input.Gender;
            employee.ModifiedBy = User.Identity?.Name ?? "System";

            await _employeeService.UpdateEmployeeAsync(employee);

            TempData["SuccessMessage"] = "Employee updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee");
            ModelState.AddModelError(string.Empty, "An error occurred while updating the employee.");
            await LoadDropdownsAsync();
            return Page();
        }
    }

    private async Task LoadDropdownsAsync()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();
        Departments = new SelectList(departments, nameof(Department.DepartmentId), nameof(Department.Name));

        var employees = await _employeeService.GetAllEmployeesAsync();
        Managers = new SelectList(employees.Where(e => e.Number != Input.Number), nameof(Employee.Number), nameof(Employee.Name));
    }

    public class InputModel
    {
        public int Number { get; set; }

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
