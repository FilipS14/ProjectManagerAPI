using DataBase.Context;
using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Repositories
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        private readonly ProjectDbContext _context;

        public TaskRepository(ProjectDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TaskEntity> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksByProjectIdAsync(int projectId)
        {
            return await _context.Tasks.Where(t => t.ProjectId == projectId).ToListAsync();
        }

        public async Task AddTaskAsync(TaskEntity task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(TaskEntity task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
