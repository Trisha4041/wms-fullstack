using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS.Domain.Entities
{
    public class LeaveRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaveId { get; set; }

        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Employee Employee { get; set; }

        [Required]
        [MaxLength(30)]
        public string LeaveType { get; set; }

        [MaxLength(255)]
        public string? Reason { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime AppliedOn { get; set; } = DateTime.Now;

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedOn { get; set; }
    }
}
