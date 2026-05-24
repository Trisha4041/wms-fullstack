using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class LeaveRepository : GenericRepository<LeaveRequest>, ILeaveRepository
    {
        public LeaveRepository(WMSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int empId)
        {
            return await _context.LeaveRequests
                .Include(l => l.Employee)
                .Where(l => l.EmpId == empId)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetPendingLeavesAsync()
        {
            return await _context.LeaveRequests
                .Include(l => l.Employee)
                .Where(l => l.Status == "Pending")
                .ToListAsync();
        }
    }
}
