using AutoMapper;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Mappings;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Tests.Application
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<EmployeeService>> _loggerMock;
        private readonly EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }, NullLoggerFactory.Instance);

            _mapper = mapperConfig.CreateMapper();

            _loggerMock = new Mock<ILogger<EmployeeService>>();

            _employeeService = new EmployeeService(
                _unitOfWorkMock.Object,
                _mapper,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetEmployeesAsync_ShouldReturnPaginatedList()
        {
            // Arrange
            var employees = new List<Employee>
        {
            TestDataBuilder.CreateTestEmployee(1),
            TestDataBuilder.CreateTestEmployee(2),
            TestDataBuilder.CreateTestEmployee(3)
        };

            _unitOfWorkMock
                .Setup(x => x.Employees.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(employees);

            // Act
            var result = await _employeeService.GetEmployeesAsync(1, 10, null, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Items.Should().HaveCount(3);
            result.Data.TotalCount.Should().Be(3);
        }

        [Fact]
        public async Task GetEmployeesAsync_WithSearchTerm_ShouldReturnFilteredResults()
        {
            // Arrange
            var employees = new List<Employee>
    {
        TestDataBuilder.CreateTestEmployee(1),
        TestDataBuilder.CreateTestEmployee(2)
    };
            // ✅ Update both FirstName and Email to avoid conflicts
            employees[0].FirstName = "John";
            employees[0].Email = "john.smith@company.com";  // Contains "john"

            employees[1].FirstName = "Jane";
            employees[1].Email = "jane.wilson@company.com";  // Does NOT contain "john"

            _unitOfWorkMock
                .Setup(x => x.Employees.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(employees);

            // Act
            var result = await _employeeService.GetEmployeesAsync(1, 10, "John", CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Data!.Items.Should().HaveCount(1);
            result.Data.Items[0].FirstName.Should().Be("John");
        }


        [Fact]
        public async Task GetEmployeeByIdAsync_ShouldReturnEmployee_WhenExists()
        {
            // Arrange
            var employee = TestDataBuilder.CreateTestEmployee();

            _unitOfWorkMock
                .Setup(x => x.Employees.GetByIdAsync(employee.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(employee.Id, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Id.Should().Be(employee.Id);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ShouldReturnFailure_WhenNotExists()
        {
            // Arrange
            _unitOfWorkMock
                .Setup(x => x.Employees.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Employee?)null);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(999, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Employee not found");
        }

        [Fact]
        public async Task CreateEmployeeAsync_ShouldReturnSuccess_WhenValidData()
        {
            // Arrange
            var createDto = new CreateEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@test.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                JoiningDate = DateTime.UtcNow,
                Gender = "Male",
                DepartmentId = 1,
                DesignationId = 1
            };

            _unitOfWorkMock
                .Setup(x => x.Employees.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Employee, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _unitOfWorkMock
                .Setup(x => x.Employees.CountAsync(null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(5);

            _unitOfWorkMock
                .Setup(x => x.Employees.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Employee e, CancellationToken ct) => e);

            _unitOfWorkMock
                .Setup(x => x.CompleteAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _employeeService.CreateEmployeeAsync(createDto, "TestUser", CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Email.Should().Be(createDto.Email);
            result.Message.Should().Be("Employee created successfully");
        }

        [Fact]
        public async Task CreateEmployeeAsync_ShouldReturnFailure_WhenEmailExists()
        {
            // Arrange
            var createDto = new CreateEmployeeDto
            {
                Email = "existing@test.com",
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                JoiningDate = DateTime.UtcNow,
                Gender = "Male",
                DepartmentId = 1,
                DesignationId = 1
            };

            _unitOfWorkMock
                .Setup(x => x.Employees.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Employee, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _employeeService.CreateEmployeeAsync(createDto, "TestUser", CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Email already exists");
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldReturnSuccess_WhenValidData()
        {
            // Arrange
            var employee = TestDataBuilder.CreateTestEmployee();
            var updateDto = new UpdateEmployeeDto
            {
                Id = employee.Id,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@test.com",
                DateOfBirth = employee.DateOfBirth,
                Gender = "Female",
                DepartmentId = 1,
                DesignationId = 1,
                IsActive = true
            };

            _unitOfWorkMock
                .Setup(x => x.Employees.GetByIdAsync(employee.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee);

            _unitOfWorkMock
                .Setup(x => x.Employees.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Employee, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _unitOfWorkMock
                .Setup(x => x.CompleteAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _employeeService.UpdateEmployeeAsync(updateDto, "TestUser", CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Employee updated successfully");
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ShouldReturnSuccess_WhenEmployeeExists()
        {
            // Arrange
            var employee = TestDataBuilder.CreateTestEmployee();

            _unitOfWorkMock
                .Setup(x => x.Employees.GetByIdAsync(employee.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee);

            _unitOfWorkMock
                .Setup(x => x.CompleteAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _employeeService.DeleteEmployeeAsync(employee.Id, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().BeTrue();
            result.Message.Should().Be("Employee deleted successfully");
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ShouldReturnFailure_WhenEmployeeNotFound()
        {
            // Arrange
            _unitOfWorkMock
                .Setup(x => x.Employees.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Employee?)null);

            // Act
            var result = await _employeeService.DeleteEmployeeAsync(999, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Employee not found");
        }
    }
}
