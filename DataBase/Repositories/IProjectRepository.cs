using DataBase.Entities;

namespace Database.Repositories
{
    public interface IProjectRepository
    {
        Task<ProjectEntity> GetProjectByIdAsync(int id);
        Task<IEnumerable<ProjectEntity>> GetProjectsByUserIdAsync(int userId);
        Task AddProjectAsync(ProjectEntity project);
        Task UpdateProjectAsync(ProjectEntity project);
        Task DeleteProjectAsync(int id);
    }
}