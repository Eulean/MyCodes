using System;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Projects> Projects { get; set; } // Add this line to include the Projects DbSet

}
