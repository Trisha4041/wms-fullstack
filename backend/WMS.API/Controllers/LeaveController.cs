using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs;
using WMS.Application.Services.Interfaces;

namespace WMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;

        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> Apply([FromBody] ApplyLeaveDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _leaveService.ApplyLeaveAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("approve")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> ApproveReject([FromBody] ApproveLeaveDTO dto)
        {
            try
            {
                var result = await _leaveService.ApproveRejectLeaveAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("cancel/{leaveId}")]
        public async Task<IActionResult> Cancel(int leaveId)
        {
            var result = await _leaveService.CancelLeaveAsync(leaveId);
            if (!result)
                return NotFound(new { message = "Leave request not found" });
            return Ok(new { message = "Leave cancelled successfully" });
        }

        [HttpGet("employee/{empId}")]
        public async Task<IActionResult> GetByEmployee(int empId)
        {
            var leaves = await _leaveService.GetByEmployeeIdAsync(empId);
            return Ok(leaves);
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetPending()
        {
            var leaves = await _leaveService.GetPendingLeavesAsync();
            return Ok(leaves);
        }
    }
}
