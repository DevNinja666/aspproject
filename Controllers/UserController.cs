using Microsoft.AspNetCore.Mvc;
using Invoicer.DTOs;
using Invoicer.Services;

namespace Invoicer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _service;

    public UserController(UserService service)
    {
        _service = service;
    }

    /// <summary>
    /// Получить список пользователей
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _service.GetAll();
        return Ok(users);
    }

    /// <summary>
    /// Получить пользователя по Id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _service.GetById(id);
        return Ok(user);
    }

    /// <summary>
    /// Обновить профиль пользователя
    /// </summary>
    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
    {
        await _service.UpdateProfile(dto);
        return Ok("Profile updated");
    }

    /// <summary>
    /// Сменить пароль
    /// </summary>
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        await _service.ChangePassword(dto);
        return Ok("Password changed");
    }

    /// <summary>
    /// Удалить пользователя
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return Ok();
    }
}
