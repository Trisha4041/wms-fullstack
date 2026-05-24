using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface ILeaveRepository : IGenericRepository<LeaveRequest>
    {
        Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int empId);
        Task<IEnumerable<LeaveRequest>> GetPendingLeavesAsync();
    }
}
