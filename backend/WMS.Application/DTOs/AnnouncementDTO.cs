using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs
{
    public class AnnouncementDTO
    {
        public int AnnouncementId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateAnnouncementDTO
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        public int CreatedBy { get; set; }
    }
}
