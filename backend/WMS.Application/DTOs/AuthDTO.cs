using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public int UserId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime Expiry { get; set; }
    }

    public class RegisterDTO
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public int RoleId { get; set; }

        public int? EmployeeId { get; set; }
    }
}
