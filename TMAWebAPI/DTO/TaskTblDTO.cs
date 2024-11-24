using System.ComponentModel.DataAnnotations;

namespace TMAWebAPI.DTO
{
    public class TaskTblDTO
    {
        [Key]
        
        public int TaskId { get; set; }

        [Required]
        public string TaskName { get; set; } = null!;

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int AssignedToUserId { get; set; } 

        [Required]
        public DateTime TaskStartDate { get; set; }

        [Required]
        public DateTime TaskEndDate { get; set; }

        public string Priority { get; set; } = null!;

        public string Status { get; set; } = null!;
        public string? Description { get; set; }
    }
}
