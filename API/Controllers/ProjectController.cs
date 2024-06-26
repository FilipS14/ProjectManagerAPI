using Core.Services;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataBase.Dtos;
using Core.Mappers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("getProjectById")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            try
            {
                var project = await _projectService.GetProjectByIdAsync(id);

                return Ok(project);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("getAllProjectsByUserId")]
        public async Task<IActionResult> GetAllProjects(int userId)
        {
            try
            {
                var projects = await _projectService.GetProjectsByUserIdAsync(userId);

                return Ok(projects);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("getAllProjects")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects(int orderBy = 0)
        {
            try
            {
                var projects = await _projectService.GetAllProjectsAsync();
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("addProject")]
        public async Task<IActionResult> AddProject(ProjectDto projectDto)
        {
            try
            {
                projectDto.CreatedDate = DateTime.UtcNow;
                await _projectService.AddProjectAsync(ProjectMapper.ToEntity(projectDto));

                return Ok(new { message = "Project added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("updateProject")]
        public async Task<IActionResult> UpdateProject(int ProjectId, ProjectDto projectDto)
        {
            try
            {
                projectDto.Id = ProjectId;
                await _projectService.UpdateProjectAsync(ProjectMapper.ToEntity(projectDto));


                return Ok(new { message = "Project updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("deleteProject")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                await _projectService.DeleteProjectAsync(id);

                return Ok(new { message = "Project deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
