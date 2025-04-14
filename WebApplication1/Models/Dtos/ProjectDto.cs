using System;

namespace WebApplication1.Models.Dtos;

public class ProjectDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? ImageFile { get; set; }
    public string? ImageAlt { get; set; }
    public string? Link { get; set; }
    public string? RepositoryUrl { get; set; }
    public string? Category { get; set; }
    public string? Technology { get; set; }
    public string? Status { get; set; }
    public string? Challenges { get; set; }
    public string? Outcomes { get; set; }
    public string? Summary { get; set; }
    public string? TeamMembers { get; set; }
    public bool IsFeatured { get; set; }

}
