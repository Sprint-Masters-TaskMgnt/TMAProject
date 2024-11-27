using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace TaskMgnt_MVC_.DTO
{
    public class RoleUserDTO
    {
        [Required]
        public int Id { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        public string UserName { get; set; }

        public int RoleId { get; set; }

    }
}