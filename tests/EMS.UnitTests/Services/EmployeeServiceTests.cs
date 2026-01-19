using EMS.Application.Services;
using EMS.Domain.Entities;
using EMS.Domain.Interfaces.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace EMS.UnitTests.Services;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly Mock<ILogger<EmployeeService>> _loggerMock;
    private readonly EmployeeService _employeeService;

    public EmployeeServiceTests()
    {
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _loggerMock = new Mock<ILogger<EmployeeService>>();
        _employeeService = new EmployeeService(_employeeRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEmployees()
    {
        var employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "John Doe", Email = "john@example.com" },
            new Employee { Id = 2, Name = "Jane Smith", Email = "jane@example.com" }
        };

        _employeeRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(employees);

        var result = await _employeeService.GetAllAsync();

        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(employees);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEmployee_WhenEmployeeExists()
    {
        var employee = new Employee { Id = 1, Name = "John Doe", Email = "john@example.com" };

        _employeeRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);

        var result = await _employeeService.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(employee);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateEmployee()
    {
        var employee = new Employee { Name = "John Doe", Email = "john@example.com" };

        _employeeRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);

        var result = await _employeeService.CreateAsync(employee);

        result.Should().NotBeNull();
        result.CreatedDate.Should().NotBe(default(DateTime));
        result.IsActive.Should().BeTrue();
    }
}
