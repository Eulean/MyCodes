using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Dtos;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            var result = await _userRepository.CreateUser(user);
            if (result == null)
            {
                return BadRequest(new Response(
                    message: "User creation failed.",
                    success: false,
                    data: result ?? new object()
                ));
            }
            return Ok(new Response(
                message: "User created successfully.",
                success: true,
                data: result
            ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound(new Response(
                    message: $"User with id {id} not found.",
                    success: true,
                    data: user ?? new object()
                ));
            }
            return Ok(new Response(
                message: "User retrieved successfully.",
                success: true,
                data: user
                ));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            if (users == null || users.Count == 0)
            {
                return BadRequest(new Response(
                    message: "No users found.",
                    success: false,
                    data: new object()
                ));
            }
            return Ok(new Response(
                message: "Users retrieved successfully.",
                success: true,
                data: users
            ));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null.");
            }

            var result = await _userRepository.UpdateUser(id, user);
            if (result == null)
            {
                return NotFound($"User with id {id} not found.");
            }
            return Ok(new Response(
                message: "User updated successfully.",
                success: true,
                data: result
            ));

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userRepository.DeleteUser(id);
            if (!result)
            {
                return NotFound($"User with id {id} not found.");
            }

            return Ok(new Response(
                message: "User deleted successfully.",
                success: true,
                data: result
            ));
        }
    }
}