using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Repository.Entities
{
    public class TaskEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the Title")]
        public string Title { get; set; }

        public string? Description { get; set; }

        public CustomTaskStatus Status { get; set; }

        public DateTime DueDate { get; set; }
    }

    public enum CustomTaskStatus
    {
        Pending,
        InProgress,
        Completed
    }
}
