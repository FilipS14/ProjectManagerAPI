using Database.Repositories;
using System;
using DataBase.Entities;
using Database.Repositories;
using DataBase.Context;
using Microsoft.Extensions.Configuration;
using Project.Database.Repositories;
using Core.Validation;
using Microsoft.EntityFrameworkCore;

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
                throw new Exception("There is no project with this project Id.");

            return await _projectRepository.GetProjectByIdAsync(id);
        }

        public async Task<IEnumerable<ProjectEntity>> GetProjectsByUserIdAsync(int userId)
        {
            if (!ProjectValidation.ProjectWithThisIdExists(userId, _context))
                throw new Exception("There is no user with this user Id.");

            return await _projectRepository.GetProjectsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync(int orderBy)
        {
            if (orderBy < 0 || orderBy > 2)
                throw new Exception("The order criteria given is not right. Criterias: 0 - do not order, 1 - order by created date, 2 - order by project name.");

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
            Console.WriteLine("DEBUG: Entered Project Service Add.\n");
            if (!ProjectValidation.IsProjectDataValid(project, _context))
                throw new Exception("The data provided for the new project is not valid. Check internal console for more details about the error.");

            Console.WriteLine("DEBUG: Adding the project to context\n");
            await _projectRepository.AddProjectAsync(project);
        }

        public async Task UpdateProjectAsync(ProjectEntity project)
        {
            try
            {
                Console.WriteLine("DEBUG: Entered Project Service Update.\n");
                if (!ProjectValidation.ProjectWithThisIdExists(project.Id, _context))
                    throw new Exception("There is no project with this project Id.");

                var oldProject = await GetProjectByIdAsync(project.Id);
                project.CreatedDate = oldProject.CreatedDate;

                if (!ProjectValidation.IsProjectDataValid(project, _context))
                    throw new Exception("The data provided for the new project is not valid. Check internal console for more details about the error.");

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
