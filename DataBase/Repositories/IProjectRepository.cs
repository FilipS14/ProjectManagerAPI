using DataBase.Entities;

namespace DataBase.Repositories
{
    public interface IProjectRepository
    {
        Task<ProjectEntity> GetProjectByIdAsync(int id);
        Task<IEnumerable<ProjectEntity>> GetProjectsByUserIdAsync(int userId);
        Task<IEnumerable<ProjectEntity>> GetAllAsync();
        Task AddProjectAsync(ProjectEntity project);
        Task UpdateProjectAsync(ProjectEntity project);
        Task DeleteProjectAsync(int id);
    }
}