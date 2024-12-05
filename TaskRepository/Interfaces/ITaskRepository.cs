using TaskManagement.Repository.Entities;

namespace TaskManagement.Repository.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetAllTasksAsync();

        Task<TaskEntity> GetTaskByIdAsync(int id);

        Task<IEnumerable<TaskEntity>> GetTaskByStatusAsync(int status);

        void AddTask(TaskEntity task);

        void UpdateTask(int id, TaskEntity task);

        void DeleteTask(int id);

        Task<IEnumerable<TaskEntity>> GetPagedTasksAsync(int pageNumber, int pageSize);

        Task<int> GetTotalTaskCountAsync();
    }
}
