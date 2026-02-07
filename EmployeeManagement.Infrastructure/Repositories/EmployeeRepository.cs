using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Infrastructure.Repositories
{

    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Employee>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Include(e => e.ReportingManager)
                .Include(e => e.Salaries.Where(s => s.IsCurrent))
                .Where(e => !e.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<Employee?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Include(e => e.ReportingManager)
                .Include(e => e.Salaries.Where(s => s.IsCurrent))
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
    }
}
