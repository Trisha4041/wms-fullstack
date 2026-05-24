using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Application.DTOs;

namespace WMS.Application.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync();
        Task<EmployeeDTO> GetEmployeeByIdAsync(int id);
        Task<EmployeeDTO> CreateEmployeeAsync(CreateEmployeeDTO dto);
        Task<EmployeeDTO> UpdateEmployeeAsync(UpdateEmployeeDTO dto);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<IEnumerable<EmployeeDTO>> GetByDepartmentAsync(int departmentId);
    }
}
