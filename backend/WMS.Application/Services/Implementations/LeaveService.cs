using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.Services.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services.Implementations
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IMapper _mapper;

        public LeaveService(ILeaveRepository leaveRepository, IMapper mapper)
        {
            _leaveRepository = leaveRepository;
            _mapper = mapper;
        }

        public async Task<LeaveDTO> ApplyLeaveAsync(ApplyLeaveDTO dto)
        {
            var leave = _mapper.Map<LeaveRequest>(dto);
            leave.Status = "Pending";
            leave.AppliedOn = DateTime.Now;
            var created = await _leaveRepository.AddAsync(leave);
            return _mapper.Map<LeaveDTO>(created);
        }

        public async Task<LeaveDTO> ApproveRejectLeaveAsync(ApproveLeaveDTO dto)
        {
            var leave = await _leaveRepository.GetByIdAsync(dto.LeaveId);
            if (leave == null)
                throw new Exception("Leave request not found");

            leave.Status = dto.Status;
            leave.ApprovedBy = dto.ApprovedBy;
            leave.ApprovedOn = DateTime.Now;

            var updated = await _leaveRepository.UpdateAsync(leave);
            return _mapper.Map<LeaveDTO>(updated);
        }

        public async Task<bool> CancelLeaveAsync(int leaveId)
        {
            var leave = await _leaveRepository.GetByIdAsync(leaveId);
            if (leave == null) return false;
            leave.Status = "Cancelled";
            await _leaveRepository.UpdateAsync(leave);
            return true;
        }

        public async Task<IEnumerable<LeaveDTO>> GetByEmployeeIdAsync(int empId)
        {
            var leaves = await _leaveRepository.GetByEmployeeIdAsync(empId);
            return _mapper.Map<IEnumerable<LeaveDTO>>(leaves);
        }

        public async Task<IEnumerable<LeaveDTO>> GetPendingLeavesAsync()
        {
            var leaves = await _leaveRepository.GetPendingLeavesAsync();
            return _mapper.Map<IEnumerable<LeaveDTO>>(leaves);
        }
    }
}
