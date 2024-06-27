using DataBase.Context;
using DataBase.Entities;
using DataBase.Repositories;
using Utils.Middleware.Exceptions;

namespace Core.Validation
{
    public class ProjectValidation
    {
        public static void ValidateProject(ProjectEntity project, ProjectDbContext _context)
        {
            if (!IsProjectNameValid(project.Name))
            {
                throw new ValidationException("Project name is invalid.", new List<string> { "The project name cannot be null and must start with a letter." });
            }
            
            if (!IsProjectDescriptionValid(project.Description))
            {
                throw new ValidationException("Project description is invalid because it is null.", new List<string> { "" });
            }

            if (!UserWithIdExists(project.AsigneeID, _context))
            {
                throw new NotFoundException("There is no user with the id provided.");
            }
        }

        public static bool ProjectWithThisIdExists(int projectId, ProjectDbContext _context)
        {
            Console.WriteLine("DEBUG: Checking if there's already a project with this Id.\n");
            return _context.Projects.Any(u => u.Id.Equals(projectId));
        }

        public static bool UserWithIdExists(int asigneeID, ProjectDbContext _context)
        {
            // Console.WriteLine("DEBUG: Checking if an user with this id exists.\n");
            return _context.Users.Any(u => u.Id.Equals(asigneeID));
        }


        private static bool IsProjectNameValid(string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                throw new ValidationException("Project name is null.", new List<string> { "Project name cannot be empty." });
            }

            if (!char.IsLetter(projectName[0]))
            {
                throw new ValidationException("Project name must begin with a letter.", new List<string> { "Project name does not begin with a letter." });
            }

            return true;
        }
        
        private static bool IsProjectDescriptionValid(string projectDescription)
        {
            if (string.IsNullOrEmpty(projectDescription))
            {
                throw new ValidationException("Project description cannot be null.", new List<string> { "Project description cannot be empty." });
            }
            return true;
        }
    }
}
