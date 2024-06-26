using DataBase.Repositories;
using DataBase.Entities;
using Core.Validation;
using DataBase.Context;
using Microsoft.Extensions.Configuration;
using System.Data.Entity;

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
            if (!TaskValidation.IsTaskValid(task, _context))
                throw new Exception("The data provided for the new task is not valid. Check internal console for more details about the error.");
            await _taskRepository.AddTaskAsync(task);
        }

        public async Task UpdateTaskAsync(TaskEntity task)
        {
            if (!TaskValidation.TaskExists(task.Id, _context))
                throw new Exception("There is no project with this task Id.");

            var oldTask = await GetTaskByIdAsync(task.Id);
            task.DueDate = oldTask.DueDate;

            if (!TaskValidation.IsTaskValid(task, _context))
                throw new Exception("The data provided for the edited task is not valid. Check internal console for more details about the error.");

            _context.Entry(oldTask).State = (Microsoft.EntityFrameworkCore.EntityState)EntityState.Detached;
            await _taskRepository.UpdateTaskAsync(task);
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            if (!TaskValidation.TaskExists(taskId, _context))
                throw new Exception("There is no project with this task Id.");
                
            await _taskRepository.DeleteTaskAsync(taskId);
        }
    }
}
