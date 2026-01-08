using EMS.Application.Services;
using EMS.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Application.Extensions;

/// <summary>
/// Extension methods for registering application services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();

        return services;
    }
}
