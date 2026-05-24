using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs;
using WMS.Application.Services.Interfaces;

namespace WMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null) return NotFound(new { message = "Project not found" });
            return Ok(project);
        }

        [HttpGet("{id}/employees")]
        public async Task<IActionResult> GetAssignedEmployees(int id)
        {
            var employees = await _projectService.GetAssignedEmployeesAsync(id);
            return Ok(employees);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([FromBody] CreateProjectDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var project = await _projectService.CreateProjectAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = project.ProjectId }, project);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AssignEmployee([FromBody] AssignEmployeeDTO dto)
        {
            try
            {
                var result = await _projectService.AssignEmployeeAsync(dto);
                return Ok(new { message = "Employee assigned successfully" });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _projectService.DeleteProjectAsync(id);
            if (!result) return NotFound(new { message = "Project not found" });
            return Ok(new { message = "Project deleted successfully" });
        }
    }
}
