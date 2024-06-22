using Database.Repositories;
using System;
using System.Collections.Generic;
using DataBase.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Repositories;


namespace Core.Services
{
    public class ProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public Task<ProjectEntity> GetProjectByIdAsync(int id)
        {
            return _projectRepository.GetProjectByIdAsync(id);
        }

        public Task<IEnumerable<ProjectEntity>> GetProjectsByUserIdAsync(int userId)
        {
            return _projectRepository.GetProjectsByUserIdAsync(userId);
        }

        public Task AddProjectAsync(ProjectEntity project)
        {
            return _projectRepository.AddProjectAsync(project);
        }

        public Task UpdateProjectAsync(ProjectEntity project)
        {
            return _projectRepository.UpdateProjectAsync(project);
        }

        public Task DeleteProjectAsync(int id)
        {
            return _projectRepository.DeleteProjectAsync(id);
        }
    }
}
