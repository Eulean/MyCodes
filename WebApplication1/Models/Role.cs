using System;

namespace WebApplication1.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;

}
