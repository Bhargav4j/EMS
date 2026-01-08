namespace EMS.Domain.Entities;

/// <summary>
/// Represents a department entity in the system
/// </summary>
public class Department
{
    public int DepartmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
