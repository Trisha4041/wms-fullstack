using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs;
using WMS.Application.Services.Interfaces;
using WMS.Domain.Interfaces;

namespace WMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeService employeeService, IEmployeeRepository employeeRepository)
        {
            _employeeService = employeeService;
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound(new { message = "Employee not found" });
            return Ok(employee);
        }

        [HttpGet("department/{departmentId}")]
        public async Task<IActionResult> GetByDepartment(int departmentId)
        {
            var employees = await _employeeService.GetByDepartmentAsync(departmentId);
            return Ok(employees);
        }

        [HttpGet("{id}/projects")]
        public async Task<IActionResult> GetMyProjects(int id)
        {
            var projects = await _employeeRepository.GetEmployeeProjectsAsync(id);
            var result = projects.Select(ep => new {
                projectId = ep.ProjectId,
                projectName = ep.Project.ProjectName,
                status = ep.Project.Status,
                assignedOn = ep.AssignedOn,
                startDate = ep.Project.StartDate,
                endDate = ep.Project.EndDate
            });
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var employee = await _employeeService.CreateEmployeeAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = employee.EmployeeId }, employee);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var employee = await _employeeService.UpdateEmployeeAsync(dto);
                return Ok(employee);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            if (!result) return NotFound(new { message = "Employee not found" });
            return Ok(new { message = "Employee deleted successfully" });
        }
    }
}
