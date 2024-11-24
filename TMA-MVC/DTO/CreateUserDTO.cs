using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMA_MVC.DTO
{
    public class CreateUserDTO
    {
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        public string UserName { get; set; } = null;

        public int RoleId { get; set; }
    }
}