using EMS.Application.Interfaces;
using EMS.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Application.Extensions;

/// <summary>
/// Extension methods for configuring application layer services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        // Register services
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();

        return services;
    }
}
