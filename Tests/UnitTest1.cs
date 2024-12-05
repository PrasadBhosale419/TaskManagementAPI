using Xunit;
using DotnetTask.Model;
using DotnetTask.Model.DTOs;
using Moq;
using DotnetTask.Interfaces;
using DotnetTask.Controllers;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DotnetTask.Repository;
using Backend.Data;
using Microsoft.EntityFrameworkCore;

public class TaskRepositoryTests
{
    private readonly Mock<TaskRepository> _mockTaskRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly TaskController _controller;
    private readonly Mock<TaskDbContext> _mockDbContext; // Mock DbContext

    public TaskRepositoryTests()
    {
        // Mocking DbContextOptions (DbContextOptions<TaskDbContext>)
        var options = new DbContextOptionsBuilder<TaskDbContext>()
            .UseInMemoryDatabase("TestDatabase") // Use an in-memory database for testing
            .Options;

        // Mocking the TaskDbContext
        _mockDbContext = new Mock<TaskDbContext>(options);

        // Mocking the TaskRepository
        _mockTaskRepository = new Mock<TaskRepository>();

        // Mocking the UnitOfWork
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        // Mocking IMapper
        _mockMapper = new Mock<IMapper>();

        // Setting up the UnitOfWork to return the mock TaskRepository
        _mockUnitOfWork.Setup(uow => uow.taskRepository).Returns(_mockTaskRepository.Object);

        // Passing the mock dependencies to the TaskController constructor
        _controller = new TaskController(_mockDbContext.Object, _mockTaskRepository.Object, _mockUnitOfWork.Object, _mockMapper.Object);
    }

    // Test cases go here...

    [Fact]
    public async Task AddTask_ValidTask_ReturnsStatusCode201()
    {
        var taskDto = new TaskDTO
        {
            Title = "Test Task",
            Description = "Test Description",
            Status = DotnetTask.Model.TaskStatus.Pending,
            DueDate = DateTime.Now.AddDays(2)
        };

        var taskEntity = new TaskEntity
        {
            Title = taskDto.Title,
            Description = taskDto.Description,
            Status = taskDto.Status,
            DueDate = taskDto.DueDate
        };

        _mockTaskRepository.Setup(repo => repo.AddTask(It.IsAny<TaskEntity>()));
        _mockUnitOfWork.Setup(uow => uow.SaveAsync()).ReturnsAsync(true);

        var result = await _controller.AddTask(taskDto);

        // Change this to ObjectResult instead of StatusCodeResult
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(201, objectResult.StatusCode);
    }


}
