using DataBase.Context;
using DataBase.Entities;
using System;
using Utils.Middleware.Exceptions;

namespace Core.Validation
{
    public class TaskValidation
    {
        public static void ValidateTask(TaskEntity task, ProjectDbContext _context)
        {
            if (!IsTaskNameValid(task.Name))
            {
                throw new ValidationException("Task name is not valid", new List<string> { "The task name cannot be null or longer than 100 characters and must start with a letter." });
            }
            
            if (!IsTaskDescriptionValid(task.Description))
            {
                throw new ValidationException("Task description is not valid", new List<string> { "The task description cannot be null or longer than 100 characters." });
            }

            if (!IsTaskDueDateValid(task.DueDate))
            {
                throw new ValidationException("Task due date is not valid", new List<string> { "The due date must be in the future." });
            }

            if (!AsigneeWithIdExists((int)task.AsigneeID, _context))
            {
                throw new NotFoundException("There is no user with the asignee id provided.");
            }

            if (!ProjectWithIdExists(task.ProjectId, _context))
            {
                throw new NotFoundException("There is no project with the project id provided.");
            }
        }

        public static bool TaskExists(int taskId, ProjectDbContext _context)
        {
            return _context.Tasks.Any(u => u.Id.Equals(taskId));
        }


        private static bool IsTaskNameValid(string taskName)
        {
            const int maxNameLength = 100;
            if (string.IsNullOrEmpty(taskName))
            {
                throw new ValidationException("The task name cannot be null or empty.", new List<string> { "Task name is empty" });
            }

            if (!char.IsLetter(taskName[0]))
            {
                throw new ValidationException("Task name must start with a letter.", new List<string> { "Task name doesn't start with a letter" });
            }

            if (taskName.Length > maxNameLength)
            {
                throw new ValidationException("The task name must be of maximum 100 characters.", new List<string> { "Task name must be of maximum 100 characters" });
            }
            
            return true;
        }

        private static bool IsTaskDescriptionValid(string taskDescription)
        {
            const int maxDescriptionLength = 100;
            if (string.IsNullOrEmpty(taskDescription))
            {
                throw new ValidationException("There is no description provided for the task.", new List<string> { "There is no description provided for the task" });
            }
            
            if (taskDescription.Length > maxDescriptionLength)
            {
                throw new ValidationException("The description provided must be of maximum 100 characters.", new List<string> { "The description provided must be of maximum 100 characters." });
            }
            return true;
        }

        private static bool IsTaskDueDateValid(DateTime dueDate)
        {
            if (dueDate < DateTime.Now)
            {
                throw new ValidationException("The due date must be in the future.", new List<string> { "The due date must be in the future" });
            }
            return true;
        }

        private static bool AsigneeWithIdExists(int asigneeID, ProjectDbContext _context)
        {
            if (!_context.Users.Any(u => u.Id.Equals(asigneeID)))
            {
                throw new NotFoundException("There is no user with the asignee id provided.");
            }
            return true;
        }
        
        private static bool ProjectWithIdExists(int projectId, ProjectDbContext _context)
        {
            if (!_context.Projects.Any(p => p.Id.Equals(projectId)))
            {
                throw new NotFoundException("There is no project with the project id provided.");
            }
            return true;
        }  
    }
}
