using System;
using System.Collections.Generic;

namespace TMAWebAPI.Models;

public partial class TaskTbl
{
    public int TaskId { get; set; }

    public string TaskName { get; set; } = null!;

    public int ProjectId { get; set; }

    public int AssignedToUserId { get; set; }

    public DateTime TaskStartDate { get; set; }

    public DateTime TaskEndDate { get; set; }

    public string Priority { get; set; } = null!;

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public virtual User AssignedToUser { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
