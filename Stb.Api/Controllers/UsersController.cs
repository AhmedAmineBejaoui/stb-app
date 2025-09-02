using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stb.Api.Data;
using Stb.Api.Models;
using Stb.Api.DTOs;

namespace Stb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly StbContext _db;
    public UsersController(StbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        => await _db.Users.AsNoTracking()
            .Select(u => new UserDto(u.Id, u.Username, u.Userfirstname, u.Email, u.Mobile, u.Role))
            .ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();
        return new UserDto(user.Id, user.Username, user.Userfirstname, user.Email, user.Mobile, user.Role);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
    {
        var user = new User
        {
            Username = dto.Username,
            Userfirstname = dto.Userfirstname,
            Email = dto.Email,
            Mobile = dto.Mobile,
            Password = dto.Password,
            Role = string.IsNullOrWhiteSpace(dto.Role) ? "User" : dto.Role
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        var response = new UserDto(user.Id, user.Username, user.Userfirstname, user.Email, user.Mobile, user.Role);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateUserDto dto)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();
        user.Username = dto.Username;
        user.Userfirstname = dto.Userfirstname;
        user.Email = dto.Email;
        user.Mobile = dto.Mobile;
        user.Role = string.IsNullOrWhiteSpace(dto.Role) ? user.Role : dto.Role;
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            user.Password = dto.Password;
        }
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
