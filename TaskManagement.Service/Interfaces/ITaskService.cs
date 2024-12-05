using DotnetTask.Model.DTOs;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Repository.Entities;

namespace TaskManagement.Service.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDTO>> GetAllAsync();
        Task<IEnumerable<TaskDTO>> GetAllTasks(int pageNumber, int pageSize);

        Task<TaskDTO> GetTaskByIdAsync(int id);

        Task<IEnumerable<TaskDTO>> GetTaskByStatusAsync(int status);

        void AddTask(TaskEntity task);

        void DeleteTask(int id);

        void UpdateTask(int id, TaskEntity taskd);

        Task<int> GetTotalTaskCount();
    }
}
