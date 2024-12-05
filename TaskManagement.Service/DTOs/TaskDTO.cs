/*DTOs help in only displaying the required data*/
namespace DotnetTask.Model.DTOs
{
    public class TaskDTO
    {
        public string Title { get; set; }

        public string? Description { get; set; }

        public TaskStatus Status { get; set; }

        public DateTime DueDate { get; set; }
    }
}
