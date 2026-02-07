using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Designation> Designations => Set<Designation>();
        public DbSet<Salary> Salaries => Set<Salary>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EmployeeCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.EmployeeCode).IsUnique();

                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Designation)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DesignationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ReportingManager)
                    .WithMany(e => e.Subordinates)
                    .HasForeignKey(e => e.ReportingManagerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Code).IsRequired().HasMaxLength(20);
                entity.HasIndex(d => d.Code).IsUnique();

                entity.HasOne(d => d.Manager)
                    .WithMany()
                    .HasForeignKey(d => d.ManagerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(d => !d.IsDeleted);
            });

            modelBuilder.Entity<Designation>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Title).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Code).IsRequired().HasMaxLength(20);
                entity.HasIndex(d => d.Code).IsUnique();

                entity.HasQueryFilter(d => !d.IsDeleted);
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.BasicSalary).HasColumnType("decimal(18,2)");
                entity.Property(s => s.HouseRentAllowance).HasColumnType("decimal(18,2)");
                entity.Property(s => s.TransportAllowance).HasColumnType("decimal(18,2)");
                entity.Property(s => s.MedicalAllowance).HasColumnType("decimal(18,2)");
                entity.Property(s => s.OtherAllowances).HasColumnType("decimal(18,2)");
                entity.Property(s => s.GrossSalary).HasColumnType("decimal(18,2)");
                entity.Property(s => s.TaxDeduction).HasColumnType("decimal(18,2)");
                entity.Property(s => s.OtherDeductions).HasColumnType("decimal(18,2)");
                entity.Property(s => s.NetSalary).HasColumnType("decimal(18,2)");

                entity.HasOne(s => s.Employee)
                    .WithMany(e => e.Salaries)
                    .HasForeignKey(s => s.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasQueryFilter(s => !s.IsDeleted);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();

                entity.HasOne(u => u.Employee)
                    .WithMany()
                    .HasForeignKey(u => u.EmployeeId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasQueryFilter(u => !u.IsDeleted);
            });

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Information Technology", Code = "IT", Description = "IT Department", CreatedBy = "System", CreatedAt = seedDate },
                new Department { Id = 2, Name = "Human Resources", Code = "HR", Description = "HR Department", CreatedBy = "System", CreatedAt = seedDate },
                new Department { Id = 3, Name = "Finance", Code = "FIN", Description = "Finance Department", CreatedBy = "System", CreatedAt = seedDate }
            );

            modelBuilder.Entity<Designation>().HasData(
                new Designation { Id = 1, Title = "Software Engineer", Code = "SE", Level = 1, CreatedBy = "System", CreatedAt = seedDate },
                new Designation { Id = 2, Title = "Senior Software Engineer", Code = "SSE", Level = 2, CreatedBy = "System", CreatedAt = seedDate },
                new Designation { Id = 3, Title = "Tech Lead", Code = "TL", Level = 3, CreatedBy = "System", CreatedAt = seedDate },
                new Designation { Id = 4, Title = "HR Manager", Code = "HRM", Level = 3, CreatedBy = "System", CreatedAt = seedDate }
            );

            var adminPasswordHash = "$2a$11$DakaZoeWx6L06arVW4hl9.8ppGxt2WmzvTRTV6V6ZhbNMlapZOA2e";

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@company.com",
                    PasswordHash = adminPasswordHash,
                    Role = "Admin",
                    CreatedBy = "System",
                    CreatedAt = seedDate
                }
            );
        }

    }
}
