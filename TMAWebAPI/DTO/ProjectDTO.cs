using System.ComponentModel.DataAnnotations;

namespace TMAWebAPI.DTO
{
    public class ProjectDTO
    {

        [Key]
        [Required]
        public int ProjectId { get; set; }

        [Required]
        public string ProjectName { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string Status { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
