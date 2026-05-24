using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface IAttendanceRepository : IGenericRepository<Attendance>
    {
        Task<IEnumerable<Attendance>> GetByEmployeeIdAsync(int empId);
        Task<Attendance> GetTodayAttendanceAsync(int empId);
    }
}
