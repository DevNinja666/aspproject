//надир мяллим это дополтиельное задание то что было релизовать удаление роли

using Microsoft.AspNetCore.Mvc;
using Invoicer.Services;

namespace Invoicer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly RoleService _service;

    public RolesController(RoleService service)
    {
        _service = service;
    }

    /// <summary>
    /// Удалить роль
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return Ok();
    }
}
