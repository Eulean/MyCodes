using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Projects
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string Image { get; set; }
    public string? ImageAlt { get; set; }
    public string Link { get; set; }
    public string? RepositoryUrl { get; set; }
    public string Category { get; set; }
    public string Technology { get; set; }
    public string Status { get; set; }
    public string? Challenges { get; set; }
    public string? Outcomes { get; set; }
    public string? Summary { get; set; }
    public string? TeamMembers { get; set; }
    public bool IsFeatured { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

}
