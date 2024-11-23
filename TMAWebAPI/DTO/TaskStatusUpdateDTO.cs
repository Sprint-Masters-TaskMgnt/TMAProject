using System.ComponentModel.DataAnnotations;

namespace TMAWebAPI.DTO
{
    public class TaskStatusUpdateDTO
    {
        [Required]
        [Key]
        public int TaskId { get; set; }
        public string Status { get; set; }

        //public static implicit operator string(TaskStatusUpdateDTO v)
        //{
        //    throw new NotImplementedException();
        //}
        public string Priority { get; set; }
        public DateTime? TaskEndDate { get; set; }
    }
}
