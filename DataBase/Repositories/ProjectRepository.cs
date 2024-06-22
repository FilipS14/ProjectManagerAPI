using DataBase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Database.Repositories;

namespace DataBase.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectDbContext _context;

        public ProjectRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectEntity> GetProjectByIdAsync(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task<IEnumerable<ProjectEntity>> GetProjectsByUserIdAsync(int userId)
        {
            return await _context.Projects.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task AddProjectAsync(ProjectEntity project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(ProjectEntity project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }
    }
}
