using DotnetTask.Model.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using TaskManagement.Repository.Entities;
using TaskManagement.Service.Interfaces;

namespace TaskManagement.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        //This API is to fetch all the records from the database
        [HttpGet("")]
        public async Task<ActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllAsync();
                if (tasks == null || !tasks.Any())
                {
                    return NotFound("No tasks found");
                }
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving tasks. Please try again later.");
            }
        }

        /*This API is to fetch all the records from the database with pagination applied on it with a logic that if there are 50 records in out db
          and if the pageSize is 10 then we'll get 5 records per page  */
        [HttpGet("Pagination")]
        public async Task<ActionResult> GetAllTasks(int pageNumber, int pageSize)
        {
            try
            {
               var tasks = await _taskService.GetAllTasks(pageNumber, pageSize);
                var totalTasks = await _taskService.GetTotalTaskCount();
                var totalPages = (int)Math.Ceiling(totalTasks / (double)pageSize);

                var pagedResult = new
                {
                    TotalCount = totalTasks,
                    TotalPages = totalPages,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    Tasks = tasks
                };

                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving tasks. Please try again later.");
            }
        }

        //This API pulls the the record by Id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTaskById(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);

                if (task == null)
                {
                    return NotFound($"Task with ID {id} not found.");
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the task. Please try again later.");
            }
        }

        //This API pulls the the record by Status
        [HttpGet("GetTasksByStatus/{status}")]
        public async Task<ActionResult> GetTaskByStatus(int status)
        {
            try
            {
                var tasks = await _taskService.GetTaskByStatusAsync(status);

                if (tasks == null || !tasks.Any())
                {
                    return NotFound($"No tasks found with status {status}.");
                }

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving tasks by status. Please try again later."); // Internal Server Error
            }
        }

        //This API is to add the record to the db
        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask(TaskDTO taskd)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(taskd.Title))
                {
                    return BadRequest("Title is a required field.");
                }

                var task = new TaskEntity
                {
                    Title = taskd.Title,
                    Description = taskd.Description,
                    Status = CustomTaskStatus.Pending,
                    DueDate = taskd.DueDate
                };

                _taskService.AddTask(task);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the task. Please try again later.");
            }
        }

        //This API is to delete the record from the db
        [HttpDelete("DeleteTask/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound($"Task with ID {id} not found.");
                }
                _taskService.DeleteTask(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the task. Please try again later.");
            }
        }

        //This API is to updates the record from the db based on Id
        [HttpPut("UpdateTask/{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskEntity taskd)
        {
            try
            {
                if (taskd == null)
                {
                    return BadRequest("Task data is required.");
                }

                if (string.IsNullOrWhiteSpace(taskd.Title) || string.IsNullOrWhiteSpace(taskd.Description))
                {
                    return BadRequest("Title and Description are required fields.");
                }

                var existingTask = await _taskService.GetTaskByIdAsync(id);
                if (existingTask == null)
                {
                    return NotFound($"Task with ID {id} not found.");
                }

                _taskService.UpdateTask(id, taskd);

                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the task. Please try again later."); // Internal Server Error
            }
        }

        //This Api is to get the records in the CSV file
        [HttpGet("ExportTasksAsCsv")]
        public async Task<IActionResult> ExportTasksAsCsv()
        {
            var tasks = _taskService.GetAllAsync();
            var taskEntities = await tasks;

            var csvContent = new StringBuilder();

            csvContent.AppendLine("Id,Title,Description,Status,DueDate");

            foreach (var task in taskEntities)
            {
                csvContent.AppendLine($"{task.Title},{task.Description},{task.Status},{task.DueDate}");
            }

            var byteArray = Encoding.UTF8.GetBytes(csvContent.ToString());
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/csv", "tasks.csv");
        }
    }
}
