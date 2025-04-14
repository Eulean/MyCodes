using System;

namespace WebApplication1.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;

    public int RoleId { get; set; }
    public Role? Role { get; set; } // Navigation property to Role entity
}
