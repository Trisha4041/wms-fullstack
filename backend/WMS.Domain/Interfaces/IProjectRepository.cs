using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<IEnumerable<Project>> GetActiveProjectsAsync();
        Task<IEnumerable<EmployeeProject>> GetAssignedEmployeesAsync(int projectId);
    }
}
