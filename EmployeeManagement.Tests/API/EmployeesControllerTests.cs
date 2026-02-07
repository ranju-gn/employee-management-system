using EmployeeManagement.API.Controllers;
using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Tests.API
{
    public class EmployeesControllerTests
    {
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly Mock<ILogger<EmployeesController>> _loggerMock;
        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _loggerMock = new Mock<ILogger<EmployeesController>>();
            _controller = new EmployeesController(_employeeServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetEmployees_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var paginatedResult = new PaginatedList<EmployeeDto>
            {
                Items = new List<EmployeeDto>(),
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 0
            };

            var successResult = Result<PaginatedList<EmployeeDto>>.SuccessResult(paginatedResult);

            _employeeServiceMock
                .Setup(x => x.GetEmployeesAsync(1, 10, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(successResult);

            // Act
            var result = await _controller.GetEmployees(1, 10, null, CancellationToken.None);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(successResult);
        }

        [Fact]
        public async Task GetEmployees_ShouldReturnBadRequest_WhenFailed()
        {
            // Arrange
            var failureResult = Result<PaginatedList<EmployeeDto>>.FailureResult("Error");

            _employeeServiceMock
                .Setup(x => x.GetEmployeesAsync(1, 10, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(failureResult);

            // Act
            var result = await _controller.GetEmployees(1, 10, null, CancellationToken.None);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetEmployee_ShouldReturnOk_WhenEmployeeExists()
        {
            // Arrange
            var employeeDto = new EmployeeDto { Id = 1, FirstName = "John" };
            var successResult = Result<EmployeeDto>.SuccessResult(employeeDto);

            _employeeServiceMock
                .Setup(x => x.GetEmployeeByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(successResult);

            // Act
            var result = await _controller.GetEmployee(1, CancellationToken.None);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetEmployee_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var failureResult = Result<EmployeeDto>.FailureResult("Not found");

            _employeeServiceMock
                .Setup(x => x.GetEmployeeByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync(failureResult);

            // Act
            var result = await _controller.GetEmployee(999, CancellationToken.None);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
