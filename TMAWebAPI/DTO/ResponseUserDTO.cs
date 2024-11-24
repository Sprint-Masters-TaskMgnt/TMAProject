using System.ComponentModel.DataAnnotations;

namespace TMAWebAPI.DTO
{
    public class ResponseUserDTO
    {
        [Key]
     
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
        public string UserName { get; set; } = null!;

        public int RoleId { get; set; }
    }
}
