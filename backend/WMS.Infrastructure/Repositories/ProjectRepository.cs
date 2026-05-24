using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(WMSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Project>> GetActiveProjectsAsync()
        {
            return await _context.Projects
                .Include(p => p.Client)
                .Where(p => p.Status == "Active")
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeProject>> GetAssignedEmployeesAsync(int projectId)
        {
            return await _context.EmployeeProjects
                .Include(ep => ep.Employee)
                .Where(ep => ep.ProjectId == projectId && ep.Status == true)
                .ToListAsync();
        }
    }
}
