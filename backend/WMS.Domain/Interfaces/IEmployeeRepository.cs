using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<Employee> GetByEmailAsync(string email);
        Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId);
        Task<IEnumerable<Employee>> GetByRoleAsync(int roleId);
        Task<IEnumerable<EmployeeProject>> GetEmployeeProjectsAsync(int empId);
    }
}
