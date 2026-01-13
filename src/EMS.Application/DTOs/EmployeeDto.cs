namespace EMS.Application.DTOs;

/// <summary>
/// Employee data transfer object
/// </summary>
public class EmployeeDto
{
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int DepartmentNo { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime DateOfJoining { get; set; }
    public int? ReportingTo { get; set; }
    public string? ManagerName { get; set; }
    public long Phone { get; set; }
    public decimal Salary { get; set; }
    public decimal Commission { get; set; }
    public string JobTitle { get; set; } = string.Empty;
}

/// <summary>
/// DTO for creating a new employee
/// </summary>
public class EmployeeCreateDto
{
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
}

/// <summary>
/// DTO for updating an employee
/// </summary>
public class EmployeeUpdateDto
{
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
}
