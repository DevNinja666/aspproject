using Invoicer.Data;
using Invoicer.DTOs;
using Invoicer.Models;
using Invoicer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> Register(RegisterDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = dto.Password,
            Address = dto.Address,
            PhoneNumber = dto.PhoneNumber,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> Login(string email, string password)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
    }

    public async Task<User?> UpdateProfile(int id, UpdateProfileDto dto)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return null;

        user.Name = dto.Name;
        user.Email = dto.Email;
        user.Address = dto.Address;
        user.PhoneNumber = dto.PhoneNumber;
        user.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<bool> ChangePassword(int id, ChangePasswordDto dto)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return false;

        if (user.Password != dto.OldPassword)
            return false;

        user.Password = dto.NewPassword;
        user.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}
