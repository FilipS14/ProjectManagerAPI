using DataBase.Dtos;
using DataBase.Entities;

namespace Core.Mappers
{
    public static class TaskMapper
    {
        public static TaskDto ToDto(TaskEntity task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Finished = task.Finished,
                DueDate = task.DueDate,
                ProjectId = task.ProjectId,
                AsigneeID = task.AsigneeID
            };
        }

        public static TaskEntity ToEntity(TaskDto taskDto)
        {
            return new TaskEntity
            {
                Id = taskDto.Id,
                Name = taskDto.Name,
                Description = taskDto.Description,
                Finished = taskDto.Finished,
                DueDate = taskDto.DueDate,
                ProjectId = taskDto.ProjectId,
                AsigneeID = taskDto.AsigneeID
            };
        }
    }
}
