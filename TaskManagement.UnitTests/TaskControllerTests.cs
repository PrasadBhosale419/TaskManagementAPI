using DotnetTask.Model.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagement.Repository.Entities;
using TaskManagement.Service.Interfaces;
using TaskManagement.Web.Controllers;

namespace TaskManagement.UnitTests
{
    public class TaskControllerTests
    {
        private readonly Mock<ITaskService> _mockTaskService;
        private readonly TaskController _taskController;

        public TaskControllerTests()
        {
            _mockTaskService = new Mock<ITaskService>();
            _taskController = new TaskController(_mockTaskService.Object);
        }

        [Fact]
        public async Task GetAllTasks_ShouldReturnOkWithTasks_WhenTasksExist()
        {
            // Arrange
            var tasks = new List<TaskDTO>
        {
            new TaskDTO { Title = "Task 1", Description = "Test Description 1" },
            new TaskDTO { Title = "Task 2", Description = "Test Description 2" }
        };
            _mockTaskService.Setup(service => service.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _taskController.GetAllTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTasks = Assert.IsAssignableFrom<IEnumerable<TaskDTO>>(okResult.Value);
            Assert.Equal(2, returnedTasks.Count());
        }

        [Fact]
        public async Task GetAllTasks_ShouldReturnNotFound_WhenNoTasksExist()
        {
            // Arrange
            _mockTaskService.Setup(service => service.GetAllAsync()).ReturnsAsync(Enumerable.Empty<TaskDTO>());

            // Act
            var result = await _taskController.GetAllTasks();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No tasks found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetTaskById_ShouldReturnOkWithTask_WhenTaskExists()
        {
            // Arrange
            var task = new TaskDTO { Title = "Task 1", Description = "Test Description 1" };
            _mockTaskService.Setup(service => service.GetTaskByIdAsync(1)).ReturnsAsync(task);

            // Act
            var result = await _taskController.GetTaskById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTask = Assert.IsType<TaskDTO>(okResult.Value);
            Assert.Equal("Task 1", returnedTask.Title);
        }

        [Fact]
        public async Task GetTaskById_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            _mockTaskService.Setup(service => service.GetTaskByIdAsync(999)).ReturnsAsync((TaskDTO)null);

            // Act
            var result = await _taskController.GetTaskById(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Task with ID 999 not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task AddTask_ShouldReturnCreated_WhenTaskIsValid()
        {
            // Arrange
            var task = new TaskDTO { Title = "Task 1", Description = "Test Description 1", DueDate = DateTime.Now };

            // Act
            var result = await _taskController.AddTask(task);

            // Assert
            var createdResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            _mockTaskService.Verify(service => service.AddTask(It.IsAny<TaskEntity>()), Times.Once);
        }

        [Fact]
        public async Task AddTask_ShouldReturnBadRequest_WhenTitleIsMissing()
        {
            // Arrange
            var task = new TaskDTO { Description = "Test Description 1", DueDate = DateTime.Now };

            // Act
            var result = await _taskController.AddTask(task);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Title is a required field.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnNoContent_WhenTaskExists()
        {
            // Arrange
            _mockTaskService.Setup(service => service.GetTaskByIdAsync(1)).ReturnsAsync(new TaskDTO { Title = "Task 1" });

            // Act
            var result = await _taskController.DeleteTask(1);

            // Assert
            var noContentResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
            _mockTaskService.Verify(service => service.DeleteTask(1), Times.Once);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            _mockTaskService.Setup(service => service.GetTaskByIdAsync(999)).ReturnsAsync((TaskDTO)null);

            // Act
            var result = await _taskController.DeleteTask(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Task with ID 999 not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task ExportTasksAsCsv_ShouldReturnCsvFile()
        {
            // Arrange
            var tasks = new List<TaskDTO>
        {
            new TaskDTO { Title = "Task 1", Description = "Test Description 1", DueDate = DateTime.Now },
            new TaskDTO { Title = "Task 2", Description = "Test Description 2", DueDate = DateTime.Now }
        };
            _mockTaskService.Setup(service => service.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _taskController.ExportTasksAsCsv();

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("text/csv", fileResult.ContentType);
            Assert.Equal("tasks.csv", fileResult.FileDownloadName);
        }
    }
}
