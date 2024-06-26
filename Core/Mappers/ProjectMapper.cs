using DataBase.Dtos;
using DataBase.Entities;

namespace Core.Mappers
{
    public static class ProjectMapper
    {
        public static ProjectDto ToDto(ProjectEntity project)
        {
            #pragma warning disable CS8601 // Possible null reference assignment.
            var projectDto = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                AsigneeID = project.AsigneeID,
                CreatedDate = project.CreatedDate
            };

            if (project.Tasks.Count > 0)
            {
                projectDto.Tasks = project.Tasks?.Select(t => new TaskDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    Finished = t.Finished,
                    ProjectId = t.ProjectId,
                    AsigneeID = t.AsigneeID
                }).ToList();
            }

            return projectDto;
        }

        public static ProjectEntity ToEntity(ProjectDto projectDto)
        {
            #pragma warning disable CS8601 // Possible null reference assignment.
            return new ProjectEntity
            {
                Id = projectDto.Id,
                Name = projectDto.Name,
                Description = projectDto.Description,
                CreatedDate = projectDto.CreatedDate.ToUniversalTime(),
                AsigneeID = projectDto.AsigneeID,
                Tasks = projectDto.Tasks?.Select(t => new TaskEntity
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    Finished = t.Finished,
                    ProjectId = t.ProjectId,
                    AsigneeID = t.AsigneeID
                }).ToList()
            };
#pragma warning restore CS8601 // Possible null reference assignment.
        }
    }
}
