using AutoMapper;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.DTOs.Designation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DesignationsController> _logger;

        public DesignationsController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DesignationsController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetDesignations(CancellationToken cancellationToken)
        {
            try
            {
                var designations = await _unitOfWork.Designations.GetAllAsync(cancellationToken);
                var designationDtos = _mapper.Map<List<DesignationDto>>(designations);
                return Ok(Result<List<DesignationDto>>.SuccessResult(designationDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving designations");
                return StatusCode(500, Result<List<DesignationDto>>.FailureResult("Error retrieving designations"));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDesignation(int id, CancellationToken cancellationToken)
        {
            try
            {
                var designation = await _unitOfWork.Designations.GetByIdAsync(id, cancellationToken);
                if (designation == null)
                {
                    return NotFound(Result<DesignationDto>.FailureResult("Designation not found"));
                }
                var designationDto = _mapper.Map<DesignationDto>(designation);
                return Ok(Result<DesignationDto>.SuccessResult(designationDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving designation");
                return StatusCode(500, Result<DesignationDto>.FailureResult("Error retrieving designation"));
            }
        }
    }
}
