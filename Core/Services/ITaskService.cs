using DataBase.Entities;
using DataBase.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface ITaskService
    {
        Task<TaskEntity> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskEntity>> GetTasksByProjectIdAsync(int projectId);
        Task AddTaskAsync(TaskEntity task);
        Task UpdateTaskAsync(TaskEntity task);
        Task DeleteTaskAsync(int id);
    }
}
