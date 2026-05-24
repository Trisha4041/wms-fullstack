using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.Services.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IGenericRepository<EmployeeProject> _employeeProjectRepository;
        private readonly IMapper _mapper;

        public ProjectService(
            IProjectRepository projectRepository,
            IGenericRepository<EmployeeProject> employeeProjectRepository,
            IMapper mapper)
        {
            _projectRepository = projectRepository;
            _employeeProjectRepository = employeeProjectRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProjectDTO>>(projects);
        }

        public async Task<ProjectDTO> GetProjectByIdAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            return _mapper.Map<ProjectDTO>(project);
        }

        public async Task<ProjectDTO> CreateProjectAsync(CreateProjectDTO dto)
        {
            var project = _mapper.Map<Project>(dto);
            project.Status = "Active";
            var created = await _projectRepository.AddAsync(project);
            return _mapper.Map<ProjectDTO>(created);
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            return await _projectRepository.DeleteAsync(id);
        }

        public async Task<bool> AssignEmployeeAsync(AssignEmployeeDTO dto)
        {
            var allocation = new EmployeeProject
            {
                EmpId = dto.EmpId,
                ProjectId = dto.ProjectId,
                AssignedOn = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedBy = dto.AssignedBy,
                Status = true
            };
            await _employeeProjectRepository.AddAsync(allocation);
            return true;
        }

        public async Task<IEnumerable<AssignedEmployeeDTO>> GetAssignedEmployeesAsync(int projectId)
        {
            var assignments = await _projectRepository.GetAssignedEmployeesAsync(projectId);
            return assignments.Select(ep => new AssignedEmployeeDTO
            {
                AllocationId = ep.AllocationId,
                EmpId = ep.EmpId,
                EmployeeName = ep.Employee.FirstName + " " + ep.Employee.LastName,
                AssignedOn = ep.AssignedOn,
                AssignedBy = ep.CreatedBy
            });
        }
    }
}
