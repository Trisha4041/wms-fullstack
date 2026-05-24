using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs
{
    public class LeaveDTO
    {
        public int LeaveId { get; set; }
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveType { get; set; }
        public string Reason { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; }
        public DateTime AppliedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
    }

    public class ApplyLeaveDTO
    {
        [Required]
        public int EmpId { get; set; }

        [Required]
        public string LeaveType { get; set; }

        public string Reason { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }
    }

    public class ApproveLeaveDTO
    {
        [Required]
        public int LeaveId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public int ApprovedBy { get; set; }
    }
}
