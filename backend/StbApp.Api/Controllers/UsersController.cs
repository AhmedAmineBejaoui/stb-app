using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StbApp.Api.Data;
using StbApp.Api.Dtos.Users;
using StbApp.Api.Helpers;
using StbApp.Api.Models;

namespace StbApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly StbContext _db;
        public UsersController(StbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _db.Users.AsNoTracking().Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Userfirstname = u.Userfirstname,
                Email = u.Email,
                Mobile = u.Mobile,
                Role = u.Role
            }).ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var currentId = User.GetUserId();
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && currentId != id) return Forbid();

            var user = await _db.Users.AsNoTracking().Where(u => u.Id == id).Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Userfirstname = u.Userfirstname,
                Email = u.Email,
                Mobile = u.Mobile,
                Role = u.Role
            }).FirstOrDefaultAsync();
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
                return Conflict("Nom d'utilisateur déjà utilisé.");

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

            var result = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Userfirstname = user.Userfirstname,
                Email = user.Email,
                Mobile = user.Mobile,
                Role = user.Role
            };
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, result);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            var currentId = User.GetUserId();
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && currentId != id) return Forbid();

            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();

            if (dto.Userfirstname is not null) user.Userfirstname = dto.Userfirstname;
            if (dto.Email is not null) user.Email = dto.Email;
            if (dto.Mobile is not null) user.Mobile = dto.Mobile;
            if (dto.Password is not null) user.Password = dto.Password;
            if (dto.Role is not null && isAdmin) user.Role = dto.Role;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

