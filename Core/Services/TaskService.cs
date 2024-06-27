using DataBase.Repositories;
using DataBase.Entities;
using Core.Validation;
using DataBase.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Utils.Middleware.Exceptions;

namespace Core.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IConfiguration _configuration;
        private readonly ProjectDbContext _context;

        public TaskService(ITaskRepository taskRepository, IConfiguration configuration, ProjectDbContext context)
        {
            _taskRepository = taskRepository;
            _configuration = configuration;
            _context = context;
        }

        public async Task<TaskEntity> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetTaskByIdAsync(id);
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksByProjectIdAsync(int projectId)
        {
            return await _taskRepository.GetTasksByProjectIdAsync(projectId);
        }

        public async Task AddTaskAsync(TaskEntity task)
        {
            TaskValidation.ValidateTask(task, _context);
            await _taskRepository.AddTaskAsync(task);
        }

        public async Task UpdateTaskAsync(TaskEntity task)
        {
            try
            {
                if (!TaskValidation.TaskExists(task.Id, _context))
                    throw new ValidationException("There is no project with this task Id.", new List<string> {"Invalid project id."});

                var oldTask = await GetTaskByIdAsync(task.Id);
                task.DueDate = oldTask.DueDate;

                TaskValidation.ValidateTask(task, _context);

                _context.Entry(oldTask).State = EntityState.Detached;
                await _taskRepository.UpdateTaskAsync(task);
            }
            catch (Exception e)
            {
                throw new Exception($"Couldn't update the task provided: {e}");
            }
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            if (!TaskValidation.TaskExists(taskId, _context))
                throw new ValidationException("There is no project with this task Id.", new List<string> {"Invalid project id."});
                
            await _taskRepository.DeleteTaskAsync(taskId);
        }
    }
}
