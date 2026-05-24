using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs
{
    public class AttendanceDTO
    {
        public int AttendanceId { get; set; }
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public double? TotalHours { get; set; }
        public string WorkMode { get; set; }
        public DateTime AttendanceDate { get; set; }
    }

    public class CheckInDTO
    {
        [Required]
        public int EmpId { get; set; }

        [Required]
        public string WorkMode { get; set; }
    }

    public class CheckOutDTO
    {
        [Required]
        public int EmpId { get; set; }
    }
}
