using AutoMapper;
using Moq;
using WMS.Application.DTOs;
using WMS.Application.Services.Implementations;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using Xunit;

namespace WMS.Tests
{
    public class LeaveServiceTests
    {
        private readonly Mock<ILeaveRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly LeaveService _service;

        public LeaveServiceTests()
        {
            _mockRepo = new Mock<ILeaveRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new LeaveService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task ApplyLeave_ReturnsLeaveDTO()
        {
            var applyDTO = new ApplyLeaveDTO
            {
                EmpId = 1,
                LeaveType = "Sick",
                Reason = "Fever",
                FromDate = DateTime.Now,
                ToDate = DateTime.Now.AddDays(1)
            };
            var leave = new LeaveRequest { LeaveId = 1, EmpId = 1, LeaveType = "Sick" };
            var leaveDTO = new LeaveDTO { LeaveId = 1, EmpId = 1, LeaveType = "Sick", Status = "Pending" };

            _mockMapper.Setup(m => m.Map<LeaveRequest>(applyDTO)).Returns(leave);
            _mockRepo.Setup(r => r.AddAsync(leave)).ReturnsAsync(leave);
            _mockMapper.Setup(m => m.Map<LeaveDTO>(leave)).Returns(leaveDTO);

            var result = await _service.ApplyLeaveAsync(applyDTO);

            Assert.NotNull(result);
            Assert.Equal("Sick", result.LeaveType);
        }

        [Fact]
        public async Task GetPendingLeaves_ReturnsPendingLeaves()
        {
            var leaves = new List<LeaveRequest>
            {
                new LeaveRequest { LeaveId = 1, Status = "Pending" },
                new LeaveRequest { LeaveId = 2, Status = "Pending" }
            };
            var leaveDTOs = new List<LeaveDTO>
            {
                new LeaveDTO { LeaveId = 1, Status = "Pending" },
                new LeaveDTO { LeaveId = 2, Status = "Pending" }
            };

            _mockRepo.Setup(r => r.GetPendingLeavesAsync()).ReturnsAsync(leaves);
            _mockMapper.Setup(m => m.Map<IEnumerable<LeaveDTO>>(leaves)).Returns(leaveDTOs);

            var result = await _service.GetPendingLeavesAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CancelLeave_ReturnsTrue_WhenExists()
        {
            var leave = new LeaveRequest { LeaveId = 1, Status = "Pending" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(leave);
            _mockRepo.Setup(r => r.UpdateAsync(leave)).ReturnsAsync(leave);

            var result = await _service.CancelLeaveAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task CancelLeave_ReturnsFalse_WhenNotExists()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((LeaveRequest)null);

            var result = await _service.CancelLeaveAsync(999);

            Assert.False(result);
        }
    }
}
