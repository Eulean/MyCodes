using System;
using System.Text.Json.Serialization;

namespace WebApplication1.Models.Dtos;

public class RoleDto
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; }

}
