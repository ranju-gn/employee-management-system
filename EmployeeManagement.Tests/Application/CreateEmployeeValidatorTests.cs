using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Validators;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Tests.Application
{
    public class CreateEmployeeValidatorTests
    {
        private readonly CreateEmployeeValidator _validator;

        public CreateEmployeeValidatorTests()
        {
            _validator = new CreateEmployeeValidator();
        }

        [Fact]
        public void Should_HaveError_When_FirstNameIsEmpty()
        {
            // Arrange
            var dto = new CreateEmployeeDto { FirstName = "" };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateEmployeeDto.FirstName));
        }

        [Fact]
        public void Should_HaveError_When_EmailIsInvalid()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "invalid-email"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateEmployeeDto.Email));
        }

        [Fact]
        public void Should_HaveError_When_DateOfBirthIsLessThan18Years()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                DateOfBirth = DateTime.Now.AddYears(-15),
                JoiningDate = DateTime.UtcNow,
                Gender = "Male",
                DepartmentId = 1,
                DesignationId = 1
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateEmployeeDto.DateOfBirth));
        }

        [Fact]
        public void Should_NotHaveError_When_ValidData()
        {
            // Arrange
            var dto = new CreateEmployeeDto
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

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
