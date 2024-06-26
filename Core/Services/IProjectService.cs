using DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IProjectService
    {
        Task<ProjectEntity> GetProjectByIdAsync(int id);
        Task<IEnumerable<ProjectEntity>> GetProjectsByUserIdAsync(int userId);
        Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync(int orderBy);
        Task AddProjectAsync(ProjectEntity project);
        Task UpdateProjectAsync(ProjectEntity project);
        Task DeleteProjectAsync(int id);
    }
}
