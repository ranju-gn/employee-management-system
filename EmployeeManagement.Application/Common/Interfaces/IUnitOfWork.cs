using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepository Employees { get; }
        //IRepository<Employee> Employees { get; }
        IRepository<Department> Departments { get; }
        IRepository<Designation> Designations { get; }
        IRepository<Salary> Salaries { get; }
        IRepository<User> Users { get; }

        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    }
}
