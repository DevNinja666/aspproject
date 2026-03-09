using Invoicer.DTOs;
using Invoicer.Models;

namespace Invoicer.Services.Interfaces;

public interface IUserService
{
    Task<User> Register(RegisterDto dto);
    Task<User?> Login(string email, string password);
    Task<User?> UpdateProfile(int id, UpdateProfileDto dto);
    Task<bool> ChangePassword(int id, ChangePasswordDto dto);
}
