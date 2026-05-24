using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs;
using WMS.Application.Services.Interfaces;

namespace WMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInDTO dto)
        {
            try
            {
                var result = await _attendanceService.CheckInAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut([FromBody] CheckOutDTO dto)
        {
            try
            {
                var result = await _attendanceService.CheckOutAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("employee/{empId}")]
        public async Task<IActionResult> GetByEmployee(int empId)
        {
            var records = await _attendanceService.GetByEmployeeIdAsync(empId);
            return Ok(records);
        }

        [HttpGet("today/{empId}")]
        public async Task<IActionResult> GetToday(int empId)
        {
            var record = await _attendanceService.GetTodayAttendanceAsync(empId);
            return Ok(record);
        }
    }
}
