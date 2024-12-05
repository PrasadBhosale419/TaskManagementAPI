using Microsoft.EntityFrameworkCore;
using TaskManagement.Repository.Entities;
using TaskManagement.Repository.Interfaces;
using TaskStatus = TaskManagement.Repository.Entities.CustomTaskStatus;

namespace TaskManagement.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext db;

        public TaskRepository(TaskDbContext db)
        {
            this.db = db;
        }
        public void AddTask(TaskEntity task)
        {
            db.Tasks.Add(task);
            db.SaveChanges();
        }

        public void DeleteTask(int id)
        {
            var taskToBeDeleted = db.Tasks.Where(x => x.Id == id).FirstOrDefault();
            if (taskToBeDeleted != null)
            {
                db.Tasks.Remove(taskToBeDeleted);
                db.SaveChanges();
            }
        }

        public async Task<IEnumerable<TaskEntity>> GetAllTasksAsync()
        {
            var tasks = await db.Tasks.ToListAsync();
            return tasks;
        }

        public async Task<TaskEntity> GetTaskByIdAsync(int id)
        {
            var task = await db.Tasks.Where(x => x.Id == id).FirstOrDefaultAsync();
            return task;
        }

        public async Task<IEnumerable<TaskEntity>> GetTaskByStatusAsync(int status)
        {
            var tasks = await db.Tasks.ToListAsync();
            if (status == 0)
            {
                var pendingTasks = tasks.Where(x => x.Status == CustomTaskStatus.Pending).ToList();
                return pendingTasks;
            }
            else if (status == 1)
            {
                var inProgressTasks = tasks.Where(x => x.Status == CustomTaskStatus.InProgress).ToList();
                return inProgressTasks;
            }
            else if (status == 2)
            {
                var completedTasks = tasks.Where(x => x.Status == CustomTaskStatus.Completed).ToList();
                return completedTasks;
            }
            else if (status == 3)
            {
                var dueTasks = tasks.Where(x => x.DueDate < DateTime.Now && x.Status != CustomTaskStatus.Completed).ToList();
                return dueTasks;
            }
            return tasks;

        }

        public void UpdateTask(int id, TaskEntity taskd)
        {
            var taskToUpdate = db.Tasks.Where(x => x.Id == id).FirstOrDefault();

            taskToUpdate.Title = taskd.Title;
            taskToUpdate.Description = taskd.Description;
            taskToUpdate.Status = taskd.Status;
            taskToUpdate.DueDate = taskd.DueDate;
            db.SaveChanges();
        }

        public async Task<IEnumerable<TaskEntity>> GetPagedTasksAsync(int pageNumber, int pageSize)
        {
            var tasks = await db.Tasks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return tasks;
        }

        public async Task<int> GetTotalTaskCountAsync()
        {
            return await db.Tasks.CountAsync();
        }
    }
}
