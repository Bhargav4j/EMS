namespace EMS.Domain.Entities;

/// <summary>
/// Employee domain entity
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
    public long Phone { get; set; }
    public decimal Salary { get; set; }
    public decimal Commission { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Department? Department { get; set; }
    public virtual Employee? Manager { get; set; }
}
