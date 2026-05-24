using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs
{
    public class DepartmentDTO
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class CreateDepartmentDTO
    {
        [Required]
        [MaxLength(100)]
        public string DepartmentName { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
    }
}
