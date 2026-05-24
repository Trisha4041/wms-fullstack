using AutoMapper;
using Moq;
using WMS.Application.DTOs;
using WMS.Application.Services.Implementations;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using Xunit;

namespace WMS.Tests
{
    public class DepartmentServiceTests
    {
        private readonly Mock<IDepartmentRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DepartmentService _service;

        public DepartmentServiceTests()
        {
            _mockRepo = new Mock<IDepartmentRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new DepartmentService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllDepartments_ReturnsAllDepartments()
        {
            var departments = new List<Department>
            {
                new Department { DepartmentId = 1, DepartmentName = "HR" },
                new Department { DepartmentId = 2, DepartmentName = "IT" }
            };
            var departmentDTOs = new List<DepartmentDTO>
            {
                new DepartmentDTO { DepartmentId = 1, DepartmentName = "HR" },
                new DepartmentDTO { DepartmentId = 2, DepartmentName = "IT" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(departments);
            _mockMapper.Setup(m => m.Map<IEnumerable<DepartmentDTO>>(departments)).Returns(departmentDTOs);

            var result = await _service.GetAllDepartmentsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateDepartment_ReturnsCreatedDepartment()
        {
            var createDTO = new CreateDepartmentDTO { DepartmentName = "Finance", Description = "Finance Dept" };
            var department = new Department { DepartmentId = 3, DepartmentName = "Finance" };
            var departmentDTO = new DepartmentDTO { DepartmentId = 3, DepartmentName = "Finance" };

            _mockMapper.Setup(m => m.Map<Department>(createDTO)).Returns(department);
            _mockRepo.Setup(r => r.AddAsync(department)).ReturnsAsync(department);
            _mockMapper.Setup(m => m.Map<DepartmentDTO>(department)).Returns(departmentDTO);

            var result = await _service.CreateDepartmentAsync(createDTO);

            Assert.NotNull(result);
            Assert.Equal("Finance", result.DepartmentName);
        }

        [Fact]
        public async Task DeleteDepartment_ReturnsTrue_WhenExists()
        {
            _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _service.DeleteDepartmentAsync(1);

            Assert.True(result);
        }
    }
}
