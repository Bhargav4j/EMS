using EMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Employee
/// </summary>
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Number);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Gender)
            .HasMaxLength(10);

        builder.Property(e => e.Phone)
            .HasMaxLength(50);

        builder.Property(e => e.JobTitle)
            .HasMaxLength(100);

        builder.Property(e => e.Salary)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.Commission)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasDefaultValue("System");

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100);

        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentNo)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Manager)
            .WithMany()
            .HasForeignKey(e => e.ReportingTo)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.Email);
        builder.HasIndex(e => e.DepartmentNo);
    }
}
