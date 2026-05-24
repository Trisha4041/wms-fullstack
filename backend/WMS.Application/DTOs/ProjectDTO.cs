using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
    }

    public class CreateProjectDTO
    {
        [Required]
        [MaxLength(100)]
        public string ProjectName { get; set; }
        public int? ClientId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class AssignEmployeeDTO
    {
        [Required]
        public int EmpId { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public string AssignedBy { get; set; }
    }

    public class AssignedEmployeeDTO
    {
        public int AllocationId { get; set; }
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime AssignedOn { get; set; }
        public string AssignedBy { get; set; }
    }
}
