using AutoMapper;
using DotnetTask.Model.DTOs;
using TaskManagement.Repository.Entities;
using TaskManagement.Repository.Interfaces;
using TaskManagement.Service.Interfaces;

namespace TaskManagement.Service
{

    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        public TaskService(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TaskDTO>> GetAllAsync()
        {
            try
            {
                var tasks = await _taskRepository.GetAllTasksAsync();
                return _mapper.Map<IEnumerable<TaskDTO>>(tasks);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<TaskDTO>> GetAllTasks(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var tasks = await _taskRepository.GetPagedTasksAsync(pageNumber, pageSize);

                if (tasks == null || !tasks.Any())
                {
                    return null;
                }
                return _mapper.Map<IEnumerable<TaskDTO>>(tasks);
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TaskDTO> GetTaskByIdAsync(int id)
        {
            try
            {
                var task = await _taskRepository.GetTaskByIdAsync(id);
                return _mapper.Map<TaskDTO>(task);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<TaskDTO>> GetTaskByStatusAsync(int status)
        {
            try
            {
                var task = await _taskRepository.GetTaskByStatusAsync(status);
                return _mapper.Map<IEnumerable<TaskDTO>>(task);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void AddTask(TaskEntity tasks)
        {
            try
            {
                _taskRepository.AddTask(tasks);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void DeleteTask(int id)
        {
            try
            {
                _taskRepository.DeleteTask(id);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void UpdateTask(int id, TaskEntity taskd) {
            try
            {
                _taskRepository.UpdateTask(id, taskd);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task<int> GetTotalTaskCount()
        {
            return await _taskRepository.GetTotalTaskCountAsync();
        }

    }
}
