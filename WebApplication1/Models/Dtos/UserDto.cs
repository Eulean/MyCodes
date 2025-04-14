using System;
using System.Text.Json.Serialization;

namespace WebApplication1.Models.Dtos;

public class UserDto
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public string RoleName { get; set; } = "Guest"; // Default role name

}
