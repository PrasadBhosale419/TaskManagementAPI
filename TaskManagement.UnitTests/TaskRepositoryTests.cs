using Microsoft.EntityFrameworkCore;
using TaskManagement.Repository.Entities;
using TaskManagement.Repository;

namespace TaskManagement.UnitTests
{
    public class TaskRepositoryTests
    {
        private readonly TaskRepository _taskRepository;
        private readonly TaskDbContext _dbContext;

        public TaskRepositoryTests()
        {
            // Configure the in-memory database for testing
            var options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new TaskDbContext(options);
            _taskRepository = new TaskRepository(_dbContext);
        }

        [Fact]
        public async Task AddTask_ShouldAddTaskToDatabase()
        {
            // Arrange
            var task = new TaskEntity { Id = 1, Title = "Test Task", Status = CustomTaskStatus.Pending };

            // Act
            _taskRepository.AddTask(task);
            await _dbContext.SaveChangesAsync();

            // Assert
            var result = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == 1);
            Assert.NotNull(result);
            Assert.Equal("Test Task", result.Title);
        }

        [Fact]
        public async Task GetAllTasksAsync_ShouldReturnAllTasks()
        {
            // Arrange
            _dbContext.Tasks.AddRange(
                new TaskEntity { Id = 1, Title = "Task 1" },
                new TaskEntity { Id = 2, Title = "Task 2" });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _taskRepository.GetAllTasksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExists()
        {
            // Arrange
            _dbContext.Tasks.Add(new TaskEntity { Id = 1, Title = "Task 1" });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _taskRepository.GetTaskByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Task 1", result.Title);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            // Act
            var result = await _taskRepository.GetTaskByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteTask_ShouldRemoveTask_WhenTaskExists()
        {
            // Arrange
            _dbContext.Tasks.Add(new TaskEntity { Id = 1, Title = "Task 1" });
            await _dbContext.SaveChangesAsync();

            // Act
            _taskRepository.DeleteTask(1);

            // Assert
            var result = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == 1);
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateTask_ShouldModifyExistingTask()
        {
            // Arrange
            _dbContext.Tasks.Add(new TaskEntity { Id = 1, Title = "Task 1", Status = CustomTaskStatus.Pending });
            await _dbContext.SaveChangesAsync();
            var updatedTask = new TaskEntity { Title = "Updated Task", Status = CustomTaskStatus.Completed };

            // Act
            _taskRepository.UpdateTask(1, updatedTask);

            // Assert
            var result = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == 1);
            Assert.NotNull(result);
            Assert.Equal("Updated Task", result.Title);
            Assert.Equal(CustomTaskStatus.Completed, result.Status);
        }

        [Fact]
        public async Task GetPagedTasksAsync_ShouldReturnCorrectPage()
        {
            // Arrange
            for (int i = 1; i <= 20; i++)
            {
                _dbContext.Tasks.Add(new TaskEntity { Id = i, Title = $"Task {i}" });
            }
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _taskRepository.GetPagedTasksAsync(2, 5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
            Assert.Equal("Task 6", result.First().Title); // Page 2 starts with Task 6
        }

        [Fact]
        public async Task GetTotalTaskCountAsync_ShouldReturnCorrectCount()
        {
            // Arrange
            _dbContext.Tasks.AddRange(
                new TaskEntity { Id = 1, Title = "Task 1" },
                new TaskEntity { Id = 2, Title = "Task 2" });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _taskRepository.GetTotalTaskCountAsync();

            // Assert
            Assert.Equal(2, result);
        }
    }
}
