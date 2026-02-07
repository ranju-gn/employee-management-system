using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.DTOs.Salary
{
    public class SalaryDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public decimal BasicSalary { get; set; }
        public decimal? HouseRentAllowance { get; set; }
        public decimal? TransportAllowance { get; set; }
        public decimal? MedicalAllowance { get; set; }
        public decimal? OtherAllowances { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal? TaxDeduction { get; set; }
        public decimal? OtherDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool IsCurrent { get; set; }
    }
}
