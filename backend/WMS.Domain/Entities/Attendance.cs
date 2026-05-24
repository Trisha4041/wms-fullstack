using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS.Domain.Entities
{
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttendanceId { get; set; }

        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Employee Employee { get; set; }

        [Required]
        public DateTime CheckIn { get; set; }

        public DateTime? CheckOut { get; set; }

        public double? TotalHours { get; set; }

        [MaxLength(20)]
        public string? WorkMode { get; set; }

        [Required]
        public DateTime AttendanceDate { get; set; }
    }
}
