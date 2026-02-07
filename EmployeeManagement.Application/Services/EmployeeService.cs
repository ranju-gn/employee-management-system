using AutoMapper;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Services
{
    public interface IEmployeeService
    {
        Task<Result<PaginatedList<EmployeeDto>>> GetEmployeesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
        Task<Result<EmployeeDto>> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);
        Task<Result<EmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto dto, string createdBy, CancellationToken cancellationToken);
        Task<Result<EmployeeDto>> UpdateEmployeeAsync(UpdateEmployeeDto dto, string updatedBy, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteEmployeeAsync(int id, CancellationToken cancellationToken);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EmployeeService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PaginatedList<EmployeeDto>>> GetEmployeesAsync(
            int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            try
            {
                var employees = await _unitOfWork.Employees.GetAllWithDetailsAsync(cancellationToken);

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    employees = employees.Where(e =>
                        e.FirstName.ToLower().Contains(searchTerm) ||
                        e.LastName.ToLower().Contains(searchTerm) ||
                        e.Email.ToLower().Contains(searchTerm) ||
                        e.EmployeeCode.ToLower().Contains(searchTerm));
                }

                var totalCount = employees.Count();
                var items = employees
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var employeeDtos = _mapper.Map<List<EmployeeDto>>(items);

                var result = new PaginatedList<EmployeeDto>
                {
                    Items = employeeDtos,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount
                };

                _logger.LogInformation("Retrieved {Count} employees (Page {Page} of {TotalPages})",
                    employeeDtos.Count, pageNumber, result.TotalPages);

                return Result<PaginatedList<EmployeeDto>>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employees");
                return Result<PaginatedList<EmployeeDto>>.FailureResult("Error retrieving employees");
            }
        }

        public async Task<Result<EmployeeDto>> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdWithDetailsAsync(id, cancellationToken);

                if (employee == null)
                {
                    _logger.LogWarning("Employee with ID {Id} not found", id);
                    return Result<EmployeeDto>.FailureResult("Employee not found");
                }

                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                return Result<EmployeeDto>.SuccessResult(employeeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee with ID {Id}", id);
                return Result<EmployeeDto>.FailureResult("Error retrieving employee");
            }
        }

        public async Task<Result<EmployeeDto>> CreateEmployeeAsync(
            CreateEmployeeDto dto, string createdBy, CancellationToken cancellationToken)
        {
            try
            {
                var emailExists = await _unitOfWork.Employees.ExistsAsync(e => e.Email == dto.Email, cancellationToken);
                if (emailExists)
                {
                    return Result<EmployeeDto>.FailureResult("Email already exists");
                }

                var employee = _mapper.Map<Employee>(dto);
                employee.EmployeeCode = await GenerateEmployeeCodeAsync(cancellationToken);
                employee.CreatedBy = createdBy;

                await _unitOfWork.Employees.AddAsync(employee, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                var employeeDto = _mapper.Map<EmployeeDto>(employee);

                _logger.LogInformation("Employee {EmployeeCode} created successfully by {CreatedBy}",
                    employee.EmployeeCode, createdBy);

                return Result<EmployeeDto>.SuccessResult(employeeDto, "Employee created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                return Result<EmployeeDto>.FailureResult("Error creating employee");
            }
        }

        public async Task<Result<EmployeeDto>> UpdateEmployeeAsync(
            UpdateEmployeeDto dto, string updatedBy, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(dto.Id, cancellationToken);

                if (employee == null)
                {
                    return Result<EmployeeDto>.FailureResult("Employee not found");
                }

                var emailExists = await _unitOfWork.Employees.ExistsAsync(
                    e => e.Email == dto.Email && e.Id != dto.Id, cancellationToken);

                if (emailExists)
                {
                    return Result<EmployeeDto>.FailureResult("Email already exists");
                }

                _mapper.Map(dto, employee);
                employee.UpdatedBy = updatedBy;
                employee.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Employees.UpdateAsync(employee, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                var employeeDto = _mapper.Map<EmployeeDto>(employee);

                _logger.LogInformation("Employee {Id} updated successfully by {UpdatedBy}", dto.Id, updatedBy);

                return Result<EmployeeDto>.SuccessResult(employeeDto, "Employee updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee with ID {Id}", dto.Id);
                return Result<EmployeeDto>.FailureResult("Error updating employee");
            }
        }

        public async Task<Result<bool>> DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(id, cancellationToken);

                if (employee == null)
                {
                    return Result<bool>.FailureResult("Employee not found");
                }

                employee.IsDeleted = true;
                employee.IsActive = false;
                employee.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Employees.UpdateAsync(employee, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                _logger.LogInformation("Employee {Id} deleted successfully", id);

                return Result<bool>.SuccessResult(true, "Employee deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee with ID {Id}", id);
                return Result<bool>.FailureResult("Error deleting employee");
            }
        }

        private async Task<string> GenerateEmployeeCodeAsync(CancellationToken cancellationToken)
        {
            var count = await _unitOfWork.Employees.CountAsync(cancellationToken: cancellationToken);
            return $"EMP{(count + 1):D6}";
        }
    }
}
