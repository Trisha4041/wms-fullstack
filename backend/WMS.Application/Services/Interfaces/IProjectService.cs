using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Application.DTOs;

namespace WMS.Application.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync();
        Task<ProjectDTO> GetProjectByIdAsync(int id);
        Task<ProjectDTO> CreateProjectAsync(CreateProjectDTO dto);
        Task<bool> DeleteProjectAsync(int id);
        Task<bool> AssignEmployeeAsync(AssignEmployeeDTO dto);
        Task<IEnumerable<AssignedEmployeeDTO>> GetAssignedEmployeesAsync(int projectId);
    }
}
