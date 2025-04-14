using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.Dtos;

namespace WebApplication1.Repositories;

public class UserRepository
{
    private readonly MyContext _context;

    public UserRepository(MyContext context)
    {
        _context = context;
    }

    public async Task<UserDto> CreateUser(UserDto userDto)
    {
        if (userDto == null)
        {
            throw new ArgumentNullException(nameof(userDto));
        }

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == userDto.RoleName.ToLower());
        if (role == null)
        {
            throw new KeyNotFoundException($"Role '{userDto.RoleName}' not found.");
        }

        var passwordHasher = new PasswordHasher<User>();
        var hashedPassword = passwordHasher.HashPassword(new User(), userDto.Password);

        var user = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            Password = hashedPassword,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false,

            RoleId = role.Id,
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        UserDto response = UserMapping(role, user);

        return response;


    }


    public async Task<UserDto> GetUserById(int id)
    {
        var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }

        if (user.Role == null)
        {
            throw new InvalidOperationException("User role is null.");
        }

        UserDto response = UserMapping(user.Role, user);

        return response;
    }

    public async Task<List<UserDto>> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();

        if (users == null || users.Count == 0)
        {
            throw new KeyNotFoundException("No users found.");
        }

        var userDtos = new List<UserDto>();
        foreach (var user in users)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with id {user.RoleId} not found.");
            }

            var userDto = UserMapping(role, user);
            userDtos.Add(userDto);
        }

        return userDtos;
    }

    public async Task<UserDto> UpdateUser(int id, UserDto userDto)
    {
        if (userDto == null)
        {
            throw new ArgumentNullException(nameof(userDto));
        }

        var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }

        bool isSameUser = user.Name == userDto.Name
            && user.Email == userDto.Email
            && user.Password == userDto.Password;
        if (isSameUser)
        {
            if (user.Role == null)
            {
                throw new InvalidOperationException("User role is null.");
            }

            return UserMapping(user.Role, user);
        }

        bool hasChanges = false;

        if (user.Name != userDto.Name)
        {
            user.Name = userDto.Name;
            hasChanges = true;
        }
        if (user.Email != userDto.Email)
        {
            user.Email = userDto.Email;
            hasChanges = true;
        }
        if (!string.IsNullOrEmpty(userDto.Password))
        {
            var passwordHasher = new PasswordHasher<User>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, userDto.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                user.Password = passwordHasher.HashPassword(user, userDto.Password);
                hasChanges = true;
            }
        }

        if (hasChanges)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        if (user.Role == null)
        {
            throw new InvalidOperationException("User role is null.");
        }

        return UserMapping(user.Role, user);
    }
    public async Task<bool> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        user.IsActive = false;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return true;
    }

    private static UserDto UserMapping(Role role, User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            RoleName = role.Name,

        };
    }
}
