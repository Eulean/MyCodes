using System;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.Dtos;

namespace WebApplication1.Repositories;

public class RoleRepository
{
    private readonly MyContext _context;
    public RoleRepository(MyContext context)
    {
        this._context = context;

    }

    public async Task<List<RoleDto>> GetAllRoles()
    {
        var roles = await _context.Roles
            .Where(r => r.IsActive && !r.IsDeleted)
            .ToListAsync();

        if (roles == null || roles.Count == 0)
        {
            throw new KeyNotFoundException("No roles found.");
        }

        var roleDtos = new List<RoleDto>();
        foreach (var role in roles)
        {
            RoleDto roleDto = RoleMapping(role);
            roleDtos.Add(roleDto);
        }

        return roleDtos;
    }

    public async Task<RoleDto> GetRoleByName(string name)
    {
        var role = await _context.Roles
            .Where(r => r.IsActive && !r.IsDeleted && r.Name.ToLower() == name.ToLower())
            .FirstOrDefaultAsync();
        if (role == null)
        {
            throw new KeyNotFoundException($"Role with name {name} not found.");
        }

        RoleDto roleDto = RoleMapping(role);
        return roleDto;
    }

    public async Task<RoleDto> GetRoleById(int id)
    {
        var role = await _context.Roles
            .Where(r => r.IsActive && !r.IsDeleted && r.Id == id)
            .FirstOrDefaultAsync();

        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {id} not found.");
        }

        RoleDto roleDto = RoleMapping(role);

        return roleDto;
    }

    public async Task<RoleDto> CreateRole(RoleDto roleDto)
    {
        RoleDtoCheck(roleDto);

        var role = new Role
        {
            Name = roleDto.Name,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        };

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        RoleDto response = RoleMapping(role);

        return response;
    }

    public async Task<RoleDto> UpdateRole(int id, RoleDto roleDto)
    {
        RoleDtoCheck(roleDto);

        var role = await _context.Roles
            .Where(r => r.IsActive && !r.IsDeleted && r.Id == id)
            .FirstOrDefaultAsync();

        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {id} not found.");
        }

        role.Name = roleDto.Name;
        role.UpdatedAt = DateTime.UtcNow;

        _context.Roles.Update(role);
        await _context.SaveChangesAsync();

        RoleDto response = RoleMapping(role);

        return response;
    }

    public async Task<bool> DeleteRole(int id)
    {
        var role = await _context.Roles
            .Where(r => !r.IsDeleted && r.IsActive && r.Id == id)
            .FirstOrDefaultAsync();

        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {id} not found.");
        }

        role.IsDeleted = true;
        role.DeletedAt = DateTime.UtcNow;
        role.IsActive = false;

        _context.Roles.Update(role);
        await _context.SaveChangesAsync();

        return true;
    }

    private static void RoleDtoCheck(RoleDto roleDto)
    {
        if (roleDto == null)
        {
            throw new ArgumentNullException(nameof(roleDto), "Role cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(roleDto.Name))
        {
            throw new ArgumentException("Role name cannot be empty or whitespace.");
        }

    }

    private static RoleDto RoleMapping(Role role)
    {
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name
        };
    }
}
