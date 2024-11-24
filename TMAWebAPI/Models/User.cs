using System;
using System.Collections.Generic;

namespace TMAWebAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public int? RoleId { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<TaskTbl> TaskTbls { get; set; } = new List<TaskTbl>();
}
