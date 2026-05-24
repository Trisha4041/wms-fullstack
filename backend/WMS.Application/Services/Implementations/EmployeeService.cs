using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.Services.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly IMapper _mapper;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IUserLoginRepository userLoginRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _userLoginRepository = userLoginRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
        }

        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return _mapper.Map<EmployeeDTO>(employee);
        }

        public async Task<EmployeeDTO> CreateEmployeeAsync(CreateEmployeeDTO dto)
        {
            var employee = _mapper.Map<Employee>(dto);
            var created = await _employeeRepository.AddAsync(employee);

            var username = (dto.FirstName + "." + dto.LastName).ToLower().Replace(" ", "");
            var existing = await _userLoginRepository.GetByUsernameAsync(username);
            if (existing == null)
            {
                var userLogin = new UserLogin
                {
                    Username = username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("WMS@1234"),
                    RoleId = dto.RoleId,
                    EmployeeId = created.EmployeeId
                };
                await _userLoginRepository.AddAsync(userLogin);
            }

            return _mapper.Map<EmployeeDTO>(created);
        }

        public async Task<EmployeeDTO> UpdateEmployeeAsync(UpdateEmployeeDTO dto)
        {
            var employee = _mapper.Map<Employee>(dto);
            employee.UpdatedOn = DateTime.Now;
            var updated = await _employeeRepository.UpdateAsync(employee);
            return _mapper.Map<EmployeeDTO>(updated);
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            return await _employeeRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<EmployeeDTO>> GetByDepartmentAsync(int departmentId)
        {
            var employees = await _employeeRepository.GetByDepartmentAsync(departmentId);
            return _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
        }
    }
}
