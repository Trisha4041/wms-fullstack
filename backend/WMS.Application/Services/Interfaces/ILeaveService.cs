using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Application.DTOs;

namespace WMS.Application.Services.Interfaces
{
    public interface ILeaveService
    {
        Task<LeaveDTO> ApplyLeaveAsync(ApplyLeaveDTO dto);
        Task<LeaveDTO> ApproveRejectLeaveAsync(ApproveLeaveDTO dto);
        Task<bool> CancelLeaveAsync(int leaveId);
        Task<IEnumerable<LeaveDTO>> GetByEmployeeIdAsync(int empId);
        Task<IEnumerable<LeaveDTO>> GetPendingLeavesAsync();
    }
}
