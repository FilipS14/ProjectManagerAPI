using DataBase.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Entities;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public Task<TaskEntity> GetTaskByIdAsync(int id)
        {
            return _taskRepository.GetTaskByIdAsync(id);
        }

        public Task<IEnumerable<TaskEntity>> GetTasksByProjectIdAsync(int projectId)
        {
            return _taskRepository.GetTasksByProjectIdAsync(projectId);
        }

        public Task AddTaskAsync(TaskEntity task)
        {
            return _taskRepository.AddTaskAsync(task);
        }

        public Task UpdateTaskAsync(TaskEntity task)
        {
            return _taskRepository.UpdateTaskAsync(task);
        }

        public Task DeleteTaskAsync(int id)
        {
            return _taskRepository.DeleteTaskAsync(id);
        }
    }
}
