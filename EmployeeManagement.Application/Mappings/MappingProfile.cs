using AutoMapper;
using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Application.DTOs.Designation;
using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.DTOs.Salary;
using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(s => s.Department.Name))
                .ForMember(d => d.DesignationTitle, opt => opt.MapFrom(s => s.Designation.Title))
                .ForMember(d => d.ReportingManagerName, opt => opt.MapFrom(s =>
                    s.ReportingManager != null ? $"{s.ReportingManager.FirstName} {s.ReportingManager.LastName}" : null))
                .ForMember(d => d.CurrentSalary, opt => opt.MapFrom(s =>
                    s.Salaries.FirstOrDefault(sal => sal.IsCurrent) != null
                        ? s.Salaries.FirstOrDefault(sal => sal.IsCurrent)!.NetSalary
                        : (decimal?)null));

            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();

            CreateMap<Department, DepartmentDto>()
                .ForMember(d => d.ManagerName, opt => opt.MapFrom(s =>
                    s.Manager != null ? $"{s.Manager.FirstName} {s.Manager.LastName}" : null))
                .ForMember(d => d.EmployeeCount, opt => opt.MapFrom(s => s.Employees.Count));

            CreateMap<CreateDepartmentDto, Department>();

            CreateMap<Designation, DesignationDto>()
                .ForMember(d => d.EmployeeCount, opt => opt.MapFrom(s => s.Employees.Count));

            CreateMap<Salary, SalaryDto>()
                .ForMember(d => d.EmployeeName, opt => opt.MapFrom(s =>
                    $"{s.Employee.FirstName} {s.Employee.LastName}"));
        }
    }
}
