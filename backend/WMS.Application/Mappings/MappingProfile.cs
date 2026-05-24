using AutoMapper;
using WMS.Application.DTOs;
using WMS.Domain.Entities;

namespace WMS.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.DepartmentName))
                .ForMember(dest => dest.RoleName,
                    opt => opt.MapFrom(src => src.Role.RoleName));

            CreateMap<CreateEmployeeDTO, Employee>();
            CreateMap<UpdateEmployeeDTO, Employee>();

            CreateMap<Attendance, AttendanceDTO>()
                .ForMember(dest => dest.EmployeeName,
                    opt => opt.MapFrom(src =>
                        src.Employee.FirstName + " " + src.Employee.LastName));

            CreateMap<LeaveRequest, LeaveDTO>()
                .ForMember(dest => dest.EmployeeName,
                    opt => opt.MapFrom(src =>
                        src.Employee.FirstName + " " + src.Employee.LastName));

            CreateMap<ApplyLeaveDTO, LeaveRequest>();

            CreateMap<Department, DepartmentDTO>();
            CreateMap<CreateDepartmentDTO, Department>();

            CreateMap<Project, ProjectDTO>()
                .ForMember(dest => dest.ClientName,
                    opt => opt.MapFrom(src => src.Client != null ? src.Client.ClientName : ""));

            CreateMap<CreateProjectDTO, Project>();
        }
    }
}
