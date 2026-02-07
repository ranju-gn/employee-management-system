using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IEmployeeRepository? _employees;
        private IRepository<Department>? _departments;
        private IRepository<Designation>? _designations;
        private IRepository<Salary>? _salaries;
        private IRepository<User>? _users;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEmployeeRepository Employees => _employees ??= new EmployeeRepository(_context);
        public IRepository<Department> Departments => _departments ??= new Repository<Department>(_context);
        public IRepository<Designation> Designations => _designations ??= new Repository<Designation>(_context);
        public IRepository<Salary> Salaries => _salaries ??= new Repository<Salary>(_context);
        public IRepository<User> Users => _users ??= new Repository<User>(_context);

        public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
