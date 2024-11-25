﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMA_MVC_.DTO
{
    public class CreateUserDTO
    {
        [Required]
        [Key]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        public string UserName { get; set; }

        public int RoleId { get; set; } = 2;
    }
}