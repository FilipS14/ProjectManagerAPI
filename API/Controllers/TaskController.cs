using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataBase.Dtos;
using Core.Mappers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("getTaskById")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);

                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("getTasksByProjectId")]
        public async Task<IActionResult> GetTasksByProjectId(int projectId)
        {
            try
            {
                var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("addTask")]
        public async Task<IActionResult> AddTask(TaskDto taskDto)
        {
            try
            {
                await _taskService.AddTaskAsync(TaskMapper.ToEntity(taskDto));

                return Ok(new { message = "Task added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("updateTask")]
        public async Task<IActionResult> UpdateTask(TaskDto taskDto)
        {
            try
            {
                await _taskService.UpdateTaskAsync(TaskMapper.ToEntity(taskDto));

                return Ok(new { message = "Task updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("deleteTask")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(id);

                return Ok(new { message = "Task deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
