namespace EMS.Domain.Entities;

/// <summary>
/// Represents an employee entity in the system
/// </summary>
public class Employee
{
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int DepartmentNo { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime DateOfJoining { get; set; }
    public int? ReportingTo { get; set; }
    public string Phone { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public decimal Commission { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    public Department? Department { get; set; }
    public Employee? Manager { get; set; }
}
