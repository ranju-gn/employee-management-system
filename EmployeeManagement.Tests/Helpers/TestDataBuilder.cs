using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Tests.Helpers
{
    public static class TestDataBuilder
    {
        public static Department CreateTestDepartment(int id = 1)
        {
            return new Department
            {
                Id = id,
                Name = "IT Department",
                Code = "IT",
                Description = "Information Technology",
                CreatedBy = "TestUser",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        public static Designation CreateTestDesignation(int id = 1)
        {
            return new Designation
            {
                Id = id,
                Title = "Software Engineer",
                Code = "SE",
                Level = 1,
                CreatedBy = "TestUser",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        public static Employee CreateTestEmployee(int id = 1, int departmentId = 1, int designationId = 1)
        {
            return new Employee
            {
                Id = id,
                EmployeeCode = $"EMP{id:D6}",
                FirstName = "John",
                LastName = "Doe",
                Email = $"john.doe{id}@company.com",
                PhoneNumber = "9876543210",
                DateOfBirth = new DateTime(1990, 1, 1),
                JoiningDate = new DateTime(2020, 1, 1),
                Gender = "Male",
                Address = "123 Test St",
                City = "Bangalore",
                State = "Karnataka",
                Country = "India",
                PostalCode = "560001",
                DepartmentId = departmentId,
                DesignationId = designationId,
                CreatedBy = "TestUser",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        public static User CreateTestUser(int id = 1)
        {
            return new User
            {
                Id = id,
                Username = "testuser",
                Email = "test@company.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@123"),
                Role = "User",
                CreatedBy = "System",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        public static Salary CreateTestSalary(int id = 1, int employeeId = 1)
        {
            return new Salary
            {
                Id = id,
                EmployeeId = employeeId,
                BasicSalary = 50000,
                HouseRentAllowance = 20000,
                TransportAllowance = 5000,
                MedicalAllowance = 3000,
                GrossSalary = 78000,
                TaxDeduction = 8000,
                NetSalary = 70000,
                EffectiveFrom = DateTime.UtcNow.AddMonths(-6),
                IsCurrent = true,
                CreatedBy = "TestUser",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }
    }
}
