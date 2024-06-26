using DataBase.Context;
using DataBase.Entities;
using Project.Database.Repositories;
using Database.Repositories;

namespace Core.Validation
{
    public class ProjectValidation
    {
        public static bool IsProjectDataValid(ProjectEntity project, ProjectDbContext _context)
        {
            Console.WriteLine("DEBUG: Entering the verification\n");
            if (!IsProjectNameValid(project.Name))
            {
                Console.WriteLine("ERROR: Project name is invalid.\n");
                return false;
            }
            
            if (!IsProjectDescriptionValid(project.Description))
            {
                Console.WriteLine("ERROR: Project description is invalid because it is null.\n");
                return false;
            }

            if (!UserWithIdExists(project.AsigneeID, _context))
            {
                Console.WriteLine("ERROR: There is no user with the id provided.\n");
                return false;
            }

            Console.WriteLine("DEBUG: Verification passed.\n");
            return true;
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
                Console.WriteLine("ERROR: Project name is null.\n");
                return false;
            }

            if (!char.IsLetter(projectName[0]))
            {
                Console.WriteLine("ERROR: Project name must start with a letter.\n");
                return false;
            }

            return true;
        }
        
        private static  bool IsProjectDescriptionValid(string projectDescription)
        {
            if (string.IsNullOrEmpty(projectDescription))
            {
                return false;
            }
            return true;
        }
    }
}
