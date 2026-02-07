using AutoMapper;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.DTOs.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DepartmentsController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments(CancellationToken cancellationToken)
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetAllAsync(cancellationToken);
                var departmentDtos = _mapper.Map<List<DepartmentDto>>(departments);
                return Ok(Result<List<DepartmentDto>>.SuccessResult(departmentDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving departments");
                return StatusCode(500, Result<List<DepartmentDto>>.FailureResult("Error retrieving departments"));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(int id, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(id, cancellationToken);
                if (department == null)
                {
                    return NotFound(Result<DepartmentDto>.FailureResult("Department not found"));
                }
                var departmentDto = _mapper.Map<DepartmentDto>(department);
                return Ok(Result<DepartmentDto>.SuccessResult(departmentDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving department");
                return StatusCode(500, Result<DepartmentDto>.FailureResult("Error retrieving department"));
            }
        }
    }
}
