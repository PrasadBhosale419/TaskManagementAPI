using AutoMapper;
using DotnetTask.Model.DTOs;
using Moq;
using TaskManagement.Repository.Entities;
using TaskManagement.Repository.Interfaces;
using TaskManagement.Service;

namespace TaskManagement.UnitTests
{


    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockMapper = new Mock<IMapper>();
            _taskService = new TaskService(_mockTaskRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedTasks()
        {
            // Arrange
            var tasks = new List<TaskEntity>
        {
            new TaskEntity { Description= "Test 1", Id = 1 },
            new TaskEntity { Id = 2, Description = "Task 2" }
        };

            var taskDTOs = new List<TaskDTO>
        {
            new TaskDTO {  Description = "Task 1" },
            new TaskDTO {  Description = "Task 2" }
        };

            _mockTaskRepository.Setup(repo => repo.GetAllTasksAsync()).ReturnsAsync(tasks);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<TaskDTO>>(tasks)).Returns(taskDTOs);

            // Act
            var result = await _taskService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Task 1", result.First().Description);
        }

        [Fact]
        public async Task GetAllTasks_ReturnsPagedTasks()
        {
            // Arrange
            var tasks = new List<TaskEntity>
        {
            new TaskEntity { Id = 1, Description = "Task 1" },
            new TaskEntity { Id = 2, Description = "Task 2" }
        };

            var taskDTOs = new List<TaskDTO>
        {
            new TaskDTO { Description = "Task 1" },
            new TaskDTO { Description = "Task 2" }
        };

            _mockTaskRepository.Setup(repo => repo.GetPagedTasksAsync(1, 10)).ReturnsAsync(tasks);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<TaskDTO>>(tasks)).Returns(taskDTOs);

            // Act
            var result = await _taskService.GetAllTasks(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetTaskByIdAsync_ReturnsTask_WhenFound()
        {
            // Arrange
            var task = new TaskEntity { Id = 1, Description = "Task 1" };
            var taskDTO = new TaskDTO { Description = "Task 1" };

            _mockTaskRepository.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(task);
            _mockMapper.Setup(mapper => mapper.Map<TaskDTO>(task)).Returns(taskDTO);

            // Act
            var result = await _taskService.GetTaskByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Task 1", result.Description);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            _mockTaskRepository.Setup(repo => repo.GetTaskByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskEntity)null);

            // Act
            var result = await _taskService.GetTaskByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void AddTask_CallsRepositoryMethod()
        {
            // Arrange
            var task = new TaskEntity { Id = 1, Description = "Task 1" };

            // Act
            _taskService.AddTask(task);

            // Assert
            _mockTaskRepository.Verify(repo => repo.AddTask(task), Times.Once);
        }

        [Fact]
        public void DeleteTask_CallsRepositoryMethod()
        {
            // Arrange
            var taskId = 1;

            // Act
            _taskService.DeleteTask(taskId);

            // Assert
            _mockTaskRepository.Verify(repo => repo.DeleteTask(taskId), Times.Once);
        }

        [Fact]
        public void UpdateTask_CallsRepositoryMethod()
        {
            // Arrange
            var taskId = 1;
            var task = new TaskEntity { Id = taskId, Description = "Updated Task" };

            // Act
            _taskService.UpdateTask(taskId, task);

            // Assert
            _mockTaskRepository.Verify(repo => repo.UpdateTask(taskId, task), Times.Once);
        }

        [Fact]
        public async Task GetTotalTaskCount_ReturnsCorrectCount()
        {
            // Arrange
            _mockTaskRepository.Setup(repo => repo.GetTotalTaskCountAsync()).ReturnsAsync(5);

            // Act
            var result = await _taskService.GetTotalTaskCount();

            // Assert
            Assert.Equal(5, result);
        }
    }

}
