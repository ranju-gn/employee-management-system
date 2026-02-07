using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Common.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
        Task<Employee?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    }
}
