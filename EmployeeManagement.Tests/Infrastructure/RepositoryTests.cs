using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using EmployeeManagement.Infrastructure.Repositories;
using EmployeeManagement.Tests.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Tests.Infrastructure
{
    public class RepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly Repository<Employee> _repository;

        public RepositoryTests()
        {
            _context = MockDbContextFactory.Create();
            _repository = new Repository<Employee>(_context);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenEntityExists()
        {
            // Arrange
            var employee = TestDataBuilder.CreateTestEmployee();
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(employee.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(employee.Id);
            result.FirstName.Should().Be(employee.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenEntityDoesNotExist()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            var employees = new List<Employee>
        {
            TestDataBuilder.CreateTestEmployee(1),
            TestDataBuilder.CreateTestEmployee(2),
            TestDataBuilder.CreateTestEmployee(3)
        };
            await _context.Employees.AddRangeAsync(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            var employee = TestDataBuilder.CreateTestEmployee();

            // Act
            await _repository.AddAsync(employee);
            await _context.SaveChangesAsync();

            // Assert
            var savedEmployee = await _context.Employees.FindAsync(employee.Id);
            savedEmployee.Should().NotBeNull();
            savedEmployee!.Email.Should().Be(employee.Email);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            // Arrange
            var employee = TestDataBuilder.CreateTestEmployee();
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            // Act
            employee.FirstName = "Jane";
            await _repository.UpdateAsync(employee);
            await _context.SaveChangesAsync();

            // Assert
            var updatedEmployee = await _context.Employees.FindAsync(employee.Id);
            updatedEmployee!.FirstName.Should().Be("Jane");
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            // Arrange
            var employee = TestDataBuilder.CreateTestEmployee();
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(employee);
            await _context.SaveChangesAsync();

            // Assert
            var deletedEmployee = await _context.Employees.FindAsync(employee.Id);
            deletedEmployee.Should().BeNull();
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenEntityExists()
        {
            // Arrange
            var employee = TestDataBuilder.CreateTestEmployee();
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync(e => e.Email == employee.Email);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_WhenEntityDoesNotExist()
        {
            // Act
            var result = await _repository.ExistsAsync(e => e.Email == "nonexistent@test.com");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CountAsync_ShouldReturnCorrectCount()
        {
            // Arrange
            var employees = new List<Employee>
        {
            TestDataBuilder.CreateTestEmployee(1),
            TestDataBuilder.CreateTestEmployee(2),
            TestDataBuilder.CreateTestEmployee(3)
        };
            await _context.Employees.AddRangeAsync(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.CountAsync();

            // Assert
            result.Should().Be(3);
        }

        [Fact]
        public async Task FindAsync_ShouldReturnMatchingEntities()
        {
            // Arrange
            var employees = new List<Employee>
        {
            TestDataBuilder.CreateTestEmployee(1),
            TestDataBuilder.CreateTestEmployee(2)
        };
            employees[0].FirstName = "John";
            employees[1].FirstName = "Jane";
            await _context.Employees.AddRangeAsync(employees);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.FindAsync(e => e.FirstName == "John");

            // Assert
            result.Should().HaveCount(1);
            result.First().FirstName.Should().Be("John");
        }

        public void Dispose()
        {
            MockDbContextFactory.Destroy(_context);
        }
    }
}
