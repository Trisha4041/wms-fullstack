using AutoMapper;
using Moq;
using WMS.Application.DTOs;
using WMS.Application.Services.Implementations;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using Xunit;

namespace WMS.Tests
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _mockRepo;
        private readonly Mock<IUserLoginRepository> _mockUserRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _mockRepo = new Mock<IEmployeeRepository>();
            _mockUserRepo = new Mock<IUserLoginRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new EmployeeService(_mockRepo.Object, _mockUserRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllEmployees_ReturnsAllEmployees()
        {
            var employees = new List<Employee>
            {
                new Employee { EmployeeId = 1, FirstName = "Trisha", LastName = "Padgelwar" },
                new Employee { EmployeeId = 2, FirstName = "Saniya", LastName = "Benoy" }
            };
            var employeeDTOs = new List<EmployeeDTO>
            {
                new EmployeeDTO { EmployeeId = 1, FirstName = "Trisha", LastName = "Padgelwar" },
                new EmployeeDTO { EmployeeId = 2, FirstName = "Saniya", LastName = "Benoy" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(employees);
            _mockMapper.Setup(m => m.Map<IEnumerable<EmployeeDTO>>(employees)).Returns(employeeDTOs);

            var result = await _service.GetAllEmployeesAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetEmployeeById_ReturnsCorrectEmployee()
        {
            var employee = new Employee { EmployeeId = 1, FirstName = "Trisha", LastName = "Padgelwar" };
            var employeeDTO = new EmployeeDTO { EmployeeId = 1, FirstName = "Trisha", LastName = "Padgelwar" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);
            _mockMapper.Setup(m => m.Map<EmployeeDTO>(employee)).Returns(employeeDTO);

            var result = await _service.GetEmployeeByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Trisha", result.FirstName);
        }

        [Fact]
        public async Task GetEmployeeById_ReturnsNull_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Employee)null);
            _mockMapper.Setup(m => m.Map<EmployeeDTO>((Employee)null)).Returns((EmployeeDTO)null);

            var result = await _service.GetEmployeeByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateEmployee_ReturnsCreatedEmployee()
        {
            var createDTO = new CreateEmployeeDTO
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                PhoneNumber = "1234567890",
                DepartmentId = 1,
                RoleId = 3
            };
            var employee = new Employee { EmployeeId = 3, FirstName = "John", LastName = "Doe" };
            var employeeDTO = new EmployeeDTO { EmployeeId = 3, FirstName = "John", LastName = "Doe" };

            _mockMapper.Setup(m => m.Map<Employee>(createDTO)).Returns(employee);
            _mockRepo.Setup(r => r.AddAsync(employee)).ReturnsAsync(employee);
            _mockMapper.Setup(m => m.Map<EmployeeDTO>(employee)).Returns(employeeDTO);
            _mockUserRepo.Setup(r => r.GetByUsernameAsync("john.doe")).ReturnsAsync((UserLogin)null);
            _mockUserRepo.Setup(r => r.AddAsync(It.IsAny<UserLogin>())).ReturnsAsync(new UserLogin());

            var result = await _service.CreateEmployeeAsync(createDTO);

            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsTrue_WhenExists()
        {
            _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _service.DeleteEmployeeAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsFalse_WhenNotExists()
        {
            _mockRepo.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

            var result = await _service.DeleteEmployeeAsync(999);

            Assert.False(result);
        }
    }
}
