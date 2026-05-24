using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(WMSDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .ToListAsync();
        }

        public override async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task<Employee> GetByEmailAsync(string email)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .Where(e => e.DepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByRoleAsync(int roleId)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .Where(e => e.RoleId == roleId)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeProject>> GetEmployeeProjectsAsync(int empId)
        {
            return await _context.EmployeeProjects
                .Include(ep => ep.Project)
                .Where(ep => ep.EmpId == empId && ep.Status == true)
                .ToListAsync();
        }
    }
}
