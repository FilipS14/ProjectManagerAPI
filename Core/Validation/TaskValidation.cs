using DataBase.Context;
using DataBase.Entities;
using System;

namespace Core.Valdation
{
    public class TaskValidation
    {
        public static bool IsTaskValid(TaskEntity task, ProjectDbContext _context)
        {
            if (!IsTaskNameValid(task.Name))
            {
                Console.WriteLine("ERROR: Task name is not valid\n");
                return false;
            }
            
            if (!IsTaskDescriptionValid(task.Description))
            {
                Console.WriteLine("ERROR: Task description is not valid.\n");
                return false;
            }

            if (!IsTaskDueDateValid(task.DueDate))
            {
                Console.WriteLine("ERROR: Task due date is not valid.\n");
                return false;
            }

            if (!AsigneeWithIdExists((int)task.AsigneeID, _context))
            {
                Console.WriteLine("ERROR: There is no user with the asignee id provided.\n");
                return false;
            }

            if (!ProjectWithIdExists(task.ProjectId, _context))
            {
                Console.WriteLine("ERROR: There is no project with the project id provided.\n");
                return false;
            }
            return true;
        }

        public static bool TaskExists(int taskId, ProjectDbContext _context)
        {
            return _context.Tasks.Any(u => u.Id.Equals(taskId));
        }


        private static bool IsTaskNameValid(string taskName)
        {
            const int minNameLength = 100;
            if (string.IsNullOrEmpty(taskName))
            {
                Console.WriteLine("ERROR: The task name cannot be null.\n");
                return false;
            }

            if (!char.IsLetter(taskName[0]))
            {
                Console.WriteLine("ERROR: Task name must start with a letter.\n");
                return false;
            }

            if (taskName.Length > minNameLength)
            {
                Console.WriteLine("ERROR: The task name must be of maximum 100 characters.\n");
                return false;
            }
            
            return true;
        }

        private static bool IsTaskDescriptionValid(string taskDescription)
        {
            const int maxDescriptionLength = 100;
            if (string.IsNullOrEmpty(taskDescription))
            {
                Console.WriteLine("ERROR: There is no description provided for the task.\n");
                return false;
            }
            
            if (taskDescription.Length > maxDescriptionLength)
            {
                Console.WriteLine("ERROR: The description provided must be of maximum 100 characters.\n");
                return false;
            }
            return true;
        }

        private static bool IsTaskDueDateValid(DateTime dueDate)
        {
            if (dueDate < DateTime.Now)
            {
                Console.WriteLine($"DueDate: {dueDate}, Now: {DateTime.Now}");
                Console.WriteLine("ERROR: The due date must be in the future.\n");
                return false;
            }
            return true;
        }

        private static bool AsigneeWithIdExists(int asigneeID, ProjectDbContext _context)
        {
            return _context.Users.Any(u => u.Id.Equals(asigneeID));
        }
        
        private static bool ProjectWithIdExists(int projectId, ProjectDbContext _context)
        {
            return _context.Projects.Any(p => p.Id.Equals(projectId));
        }  
    }
}
