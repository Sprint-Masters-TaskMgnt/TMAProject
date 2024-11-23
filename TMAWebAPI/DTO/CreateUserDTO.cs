using System.ComponentModel.DataAnnotations;

namespace TMAWebAPI.DTO
{
    public class CreateUserDTO
    {
        [Required]
        [Key]
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        public string? PasswordHash { get; set; }
        public string UserName { get; set; } = null!;

        public int RoleId { get; set; }
    }
}
