using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Dtos;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleRepository _repository;

        public RoleController(RoleRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var results = await _repository.GetAllRoles();
            if (results == null || results.Count == 0)
            {
                return NotFound("No roles found.");
            }

            return Ok(new Response(
                message: "Roles retrieved successfully.",
                success: true,
                data: results
            ));

        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleDto roleDto)
        {
            if (roleDto == null)
            {
                return BadRequest("Role data is null.");
            }

            if (string.IsNullOrEmpty(roleDto.Name))
            {
                return BadRequest("Role name is required.");
            }

            var result = await _repository.CreateRole(roleDto);
            if (result == null)
            {
                return BadRequest("Failed to create role.");

            }

            return Ok(new Response(
                message: "Role created successfully.",
                success: true,
                data: result
            ));
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var result = await _repository.GetRoleById(id);
            if (result == null)
            {
                return BadRequest($"No Role Found with this Id: {id} ");
            }

            return Ok(new Response(
                message: "Role retrieved successfully.",
                success: true,
                data: result
            ));
        }

        [HttpGet("name/{roleName}")]
        public async Task<IActionResult> GetRoleByName(string roleName)
        {
            var result = await _repository.GetRoleByName(roleName);
            if (result == null)
            {
                return BadRequest($"No Role Found with this name: {roleName} ");
            }

            return Ok(new Response(
                message: "Role retrieved successfully.",
                success: true,
                data: result
            ));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, RoleDto roleDto)
        {
            if (roleDto == null)
            {
                return BadRequest("Role data is null.");
            }

            var result = await _repository.UpdateRole(id, roleDto);
            if (result == null)
            {
                return BadRequest("Failed to update role.");
            }

            return Ok(new Response(
                message: "Role updated successfully.",
                success: true,
                data: result
            ));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _repository.DeleteRole(id);
            if (!result)
            {
                return BadRequest($"No Role found with this Id: {id}");
            }

            return Ok(new Response(
                message: "Role deleted successfully.",
                success: true,
                data: result
            ));
        }
    }
}