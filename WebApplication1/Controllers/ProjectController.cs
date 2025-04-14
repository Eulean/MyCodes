using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Dtos;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectRepository _repository;

        public ProjectController(ProjectRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProject([FromForm] ProjectDto projectDto)
        {
            if (projectDto == null)
            {
                return BadRequest("Project data is required.");
            }

            var result = await _repository.CreateProject(projectDto);
            if (result == null)
            {
                return BadRequest("Failed to create project.");
            }

            return Ok(new Response(
                message: "Project created successfully.",
                success: true,
                data: result
            ));
            // return CreatedAtAction(nameof(GetProjectById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var result = await _repository.GetById(id);

            if (result == null)
            {
                return NotFound(new Response(
                    message: $"Project with ID {id} not found.",
                    success: false,
                    data: new object()
                ));
            }

            return Ok(new Response(
                message: "Project retrieved successfully.",
                success: true,
                data: result
            ));
        }
    }
}
