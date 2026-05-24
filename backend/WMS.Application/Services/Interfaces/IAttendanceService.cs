using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Application.DTOs;

namespace WMS.Application.Services.Interfaces
{
    public interface IAttendanceService
    {
        Task<AttendanceDTO> CheckInAsync(CheckInDTO dto);
        Task<AttendanceDTO> CheckOutAsync(CheckOutDTO dto);
        Task<IEnumerable<AttendanceDTO>> GetByEmployeeIdAsync(int empId);
        Task<AttendanceDTO> GetTodayAttendanceAsync(int empId);
    }
}
