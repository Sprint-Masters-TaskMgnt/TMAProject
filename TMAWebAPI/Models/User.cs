using System.ComponentModel.DataAnnotations;

namespace TMAWebAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        public string? PasswordHash { get; set; }
        public string UserName { get; set; } = null!;

        public string RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<TaskTbl> TaskTbl { get; set; }
    }
}
