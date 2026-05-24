using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Application.DTOs;

namespace WMS.Application.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync();
        Task<DepartmentDTO> GetDepartmentByIdAsync(int id);
        Task<DepartmentDTO> CreateDepartmentAsync(CreateDepartmentDTO dto);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
