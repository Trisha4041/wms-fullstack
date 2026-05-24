using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories
{
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(WMSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Attendance>> GetByEmployeeIdAsync(int empId)
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.EmpId == empId)
                .ToListAsync();
        }

        public async Task<Attendance> GetTodayAttendanceAsync(int empId)
        {
            return await _context.Attendances
                .FirstOrDefaultAsync(a => a.EmpId == empId
                    && a.AttendanceDate.Date == DateTime.Today);
        }
    }
}
