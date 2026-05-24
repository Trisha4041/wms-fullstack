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
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;

        public AttendanceService(IAttendanceRepository attendanceRepository, IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
        }

        public async Task<AttendanceDTO> CheckInAsync(CheckInDTO dto)
        {
            var existing = await _attendanceRepository.GetTodayAttendanceAsync(dto.EmpId);
            if (existing != null)
                throw new Exception("Already checked in today");

            var attendance = new Attendance
            {
                EmpId = dto.EmpId,
                CheckIn = DateTime.Now,
                WorkMode = dto.WorkMode,
                AttendanceDate = DateTime.Today
            };

            var created = await _attendanceRepository.AddAsync(attendance);
            return _mapper.Map<AttendanceDTO>(created);
        }

        public async Task<AttendanceDTO> CheckOutAsync(CheckOutDTO dto)
        {
            var attendance = await _attendanceRepository.GetTodayAttendanceAsync(dto.EmpId);
            if (attendance == null)
                throw new Exception("No check-in found for today");

            attendance.CheckOut = DateTime.Now;
            attendance.TotalHours = (attendance.CheckOut.Value - attendance.CheckIn).TotalHours;

            var updated = await _attendanceRepository.UpdateAsync(attendance);
            return _mapper.Map<AttendanceDTO>(updated);
        }

        public async Task<IEnumerable<AttendanceDTO>> GetByEmployeeIdAsync(int empId)
        {
            var records = await _attendanceRepository.GetByEmployeeIdAsync(empId);
            return _mapper.Map<IEnumerable<AttendanceDTO>>(records);
        }

        public async Task<AttendanceDTO> GetTodayAttendanceAsync(int empId)
        {
            var record = await _attendanceRepository.GetTodayAttendanceAsync(empId);
            return _mapper.Map<AttendanceDTO>(record);
        }
    }
}
