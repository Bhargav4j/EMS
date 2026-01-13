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
    public InputModel Input { get; set; } = new();

    public SelectList Departments { get; set; } = new(new List<Department>(), "Id", "Name");
    public SelectList Managers { get; set; } = new(new List<Employee>(), "Id", "Name");

    public class InputModel
    {
        public int Id { get; set; }

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

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Phone = employee.Phone,
                Gender = employee.Gender,
                DateOfBirth = employee.DateOfBirth,
                DateOfJoining = employee.DateOfJoining,
                DepartmentId = employee.DepartmentId,
                JobTitle = employee.JobTitle,
                ReportingTo = employee.ReportingTo,
                Salary = employee.Salary,
                Commission = employee.Commission
            };

            await LoadDropdownsAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading employee for edit with ID {EmployeeId}", id);
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
            var employee = await _employeeService.GetByIdAsync(Input.Id);

            if (employee == null)
            {
                return NotFound();
            }

            employee.Name = Input.Name;
            employee.Email = Input.Email;
            employee.Phone = Input.Phone;
            employee.Gender = Input.Gender;
            employee.DateOfBirth = Input.DateOfBirth;
            employee.DateOfJoining = Input.DateOfJoining;
            employee.DepartmentId = Input.DepartmentId;
            employee.JobTitle = Input.JobTitle;
            employee.ReportingTo = Input.ReportingTo;
            employee.Salary = Input.Salary;
            employee.Commission = Input.Commission;

            await _employeeService.UpdateAsync(employee);

            TempData["SuccessMessage"] = "Employee updated successfully.";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee with ID {EmployeeId}", Input.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the employee.");
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
