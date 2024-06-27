using DataBase.Repositories;
using DataBase.Entities;
using DataBase.Context;
using Microsoft.Extensions.Configuration;
using Core.Validation;
using Microsoft.EntityFrameworkCore;
using Utils.Middleware.Exceptions;

namespace Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IConfiguration _configuration;
        private readonly ProjectDbContext _context;


        public ProjectService(IConfiguration configuration, ProjectDbContext context, IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
            _context = context;
            _configuration = configuration;
        }

        public async Task<ProjectEntity> GetProjectByIdAsync(int id)
        {
            if (!ProjectValidation.ProjectWithThisIdExists(id, _context))
                throw new ValidationException("There is no project with this project Id.", 
                                            new List<string> { "Invalid project id." });

            return await _projectRepository.GetProjectByIdAsync(id);
        }

        public async Task<IEnumerable<ProjectEntity>> GetProjectsByUserIdAsync(int userId)
        {
            if (!ProjectValidation.ProjectWithThisIdExists(userId, _context))
                throw new ValidationException("There is no user with this user Id.", 
                                            new List<string> { "Invalid user id." });

            return await _projectRepository.GetProjectsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync(int orderBy)
        {
            if (orderBy < 0 || orderBy > 2)
                throw new ValidationException("The order criteria given is not right. Criterias: 0 - do not order, 1 - order by created date, 2 - order by project name.",
                                            new List<string> { "Invalid criteria id." });

            var projects = await _projectRepository.GetAllAsync();

            switch (orderBy)
            {
                case 1: // Order by Created Date, ascending
                    return projects.OrderBy(p => p.CreatedDate);
                case 2: // Order by Name, alphabetically
                    return projects.OrderBy(p => p.Name);
                default:
                    return projects;
            }
        }

        public async Task AddProjectAsync(ProjectEntity project)
        {
            ProjectValidation.ValidateProject(project, _context);

            await _projectRepository.AddProjectAsync(project);
        }

        public async Task UpdateProjectAsync(ProjectEntity project)
        {
            try
            {
                if (!ProjectValidation.ProjectWithThisIdExists(project.Id, _context))
                    throw new ValidationException("There is no project with this project Id.", new List<string>{ "Invalid project id." });

                var oldProject = await GetProjectByIdAsync(project.Id);
                project.CreatedDate = oldProject.CreatedDate;

                ProjectValidation.ValidateProject(project, _context);
                
                _context.Entry(oldProject).State = EntityState.Detached;
                await _projectRepository.UpdateProjectAsync(project);
            }
            catch (Exception e)
            {
                throw new Exception($"Couldn't update the user provided: {e}");
            }
        }

        public async Task DeleteProjectAsync(int id)
        {
            Console.WriteLine("DEBUG: Entered Project Service Delete.\n");
            if (!ProjectValidation.ProjectWithThisIdExists(id, _context))
                throw new Exception("There is no project with this project Id.");
                
            await _projectRepository.DeleteProjectAsync(id);
        }
    }
}
