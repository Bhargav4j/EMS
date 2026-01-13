using System.ComponentModel.DataAnnotations;

namespace EMS.Web.ViewModels;

/// <summary>
/// View model for displaying employee information
/// </summary>
public class EmployeeViewModel
{
    public int Number { get; set; }

    [Required]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Department")]
    public int DepartmentNo { get; set; }

    [Display(Name = "Department Name")]
    public string DepartmentName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Gender")]
    public string Gender { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [Display(Name = "Date of Joining")]
    [DataType(DataType.Date)]
    public DateTime DateOfJoining { get; set; }

    [Display(Name = "Reports To")]
    public int? ReportingTo { get; set; }

    [Display(Name = "Manager Name")]
    public string? ManagerName { get; set; }

    [Required]
    [Display(Name = "Phone")]
    public long Phone { get; set; }

    [Required]
    [Display(Name = "Salary")]
    [Range(0, double.MaxValue)]
    public decimal Salary { get; set; }

    [Display(Name = "Commission")]
    [Range(0, double.MaxValue)]
    public decimal Commission { get; set; }

    [Required]
    [Display(Name = "Job Title")]
    public string JobTitle { get; set; } = string.Empty;
}
