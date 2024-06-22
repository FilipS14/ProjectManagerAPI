using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase.Entities;

namespace DataBase.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskEntity> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskEntity>> GetTasksByProjectIdAsync(int projectId);
        Task AddTaskAsync(TaskEntity task);
        Task UpdateTaskAsync(TaskEntity task);
        Task DeleteTaskAsync(int id);
    }
}
